using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExpenseBuddy.Data.Models;
using ExpenseBuddy.Services;
using ExpenseBuddy.Services.BankAccounts;
using ExpenseBuddy.Web.Models.BankAccountsViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpenseBuddy.Web.Controllers
{
    public class BankAccountsController : Controller
    {
        private readonly IBankAccountRepositoryService _bankAccounts;
        private readonly IBankRepositoryService _banks;
        private readonly IApplicationUserRepositoryService _users;
        private readonly IUserBankAccountService _bankAccountsService;
        private readonly UserManager<ApplicationUser> _um;

        public BankAccountsController(
            IBankAccountRepositoryService bankAccounts,
            IBankRepositoryService banks,
            IApplicationUserRepositoryService users,
            IUserBankAccountService bankAccountsService,
            UserManager<ApplicationUser> um)
        {
            _bankAccounts = bankAccounts;
            _banks = banks;
            _users = users;
            _bankAccountsService = bankAccountsService;
            _um = um;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _um.FindByNameAsync(User.Identity.Name);

            if (await _um.IsInRoleAsync(user, "Administrator")) {
                var ba = await _bankAccountsService.All();
                return View(ba);
            } else
            {
                var ba = await _bankAccountsService.AllForUser(user.Id);
                return View(ba);
            }



        }

        public async Task<IActionResult> Create()
        {

            var ba = new BankAccountCreateViewModel()
            {
                IsActive = true,
                Banks = (await _banks.AllAsync(predicate: w => w.Name != "Internal Bank"))
                    .Select(k => new SelectListItem()
                    {
                        Text = k.Name,
                        Value = k.Id.ToString()
                    }).ToList()
            };

            return View(ba);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BankAccountCreateViewModel bankAccount)
        {
            if (!ModelState.IsValid)
            {
                return View(bankAccount);
            }

            var user = (await _users.AllAsync(predicate: k => k.UserName == User.Identity.Name)).FirstOrDefault();
           
            var ba = Mapper.Map(bankAccount, new BankAccount(), opt => opt.AfterMap((src, dst) => dst.UserId = user.Id));

            await _bankAccounts.CreateAsync(ba);
            await _bankAccounts.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var b = (await _bankAccounts.AllAsync<BankAccountEditViewModel>(predicate: k => k.Id == id)).FirstOrDefault();

            b.Banks = (await _banks.AllAsync(predicate: w => w.Name != "Internal Bank"))
                    .Select(k => new SelectListItem()
                    {
                        Text = k.Name,
                        Value = k.Id.ToString()
                    }).ToList();


            return View(b);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BankAccountEditViewModel bankAccount)
        {
            if (!ModelState.IsValid) {
                return View(bankAccount);
            }

            var b = (await _bankAccounts.AllAsync(predicate: k => k.Id == bankAccount.Id)).FirstOrDefault();

            if (b == null) {
                return NotFound();
            }

            Mapper.Map(bankAccount, b);

            await _bankAccounts.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }

}