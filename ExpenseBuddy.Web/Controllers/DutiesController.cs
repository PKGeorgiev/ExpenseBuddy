using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseBuddy.Data.Models;
using ExpenseBuddy.Services.Duties;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseBuddy.Web.Controllers
{
    public class DutiesController : BaseController
    {
        private readonly IDutiesService _duties;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser _user;

        public DutiesController(
            IDutiesService duties,
            UserManager<ApplicationUser> userManager)
        {
            _duties = duties;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            if (User.IsInRole("Administrator"))
            {
                var tmp = await _duties.All();
                return View(tmp);
            }
            else
            {
                _user = await _userManager.FindByNameAsync(User.Identity.Name);
                var tmp = await _duties.AllForUser(_user.Id);
                return View(tmp);
            }
        }

        public async Task<IActionResult> Pay(int expenseId, string payerId)
        {
            _user = await _userManager.FindByNameAsync(User.Identity.Name);
            var du = await _duties.FindById(_user.Id, expenseId, payerId);

            return View(du);
        }

        [HttpPost]
        public async Task<IActionResult> Pay(string payerId, int expenseId)
        {
            _user = await _userManager.FindByNameAsync(User.Identity.Name);

            var du = await _duties.FindById(_user.Id, expenseId, payerId);

            try
            {
                await _duties.Pay(_user.Id, expenseId, payerId);
            }
            catch (Exception e) {
                ModelState.AddModelError("", e.Message);
                return View(du);
            }

            return RedirectToAction(nameof(Index));
        }

    }
}