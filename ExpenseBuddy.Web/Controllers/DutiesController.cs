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

        public DutiesController(
            IDutiesService duties,
            UserManager<ApplicationUser> userManager)
        {
            _duties = duties;
            _userManager = userManager;            
        }

        public async Task<IActionResult> Index()
        {
            _user = await _userManager.FindByNameAsync(User.Identity.Name);
            var tmp = await _duties.AllForUser(_user.Id);

            return View(tmp);
        }
    }
}