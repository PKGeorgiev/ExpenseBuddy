﻿using System;
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
    public class BankAccountsController : BaseController
    {
        private readonly IBankAccountRepositoryService _bankAccounts;
        private readonly IBankRepositoryService _banks;
        private readonly IApplicationUserRepositoryService _users;
        private readonly IUserBankAccountService _bankUserAccountsService;
        private readonly UserManager<ApplicationUser> _um;

        public BankAccountsController(
            IBankAccountRepositoryService bankAccounts,
            IBankRepositoryService banks,
            IApplicationUserRepositoryService users,
            IUserBankAccountService bankUserAccountsService,
            UserManager<ApplicationUser> um)
        {
            _bankAccounts = bankAccounts;
            _banks = banks;
            _users = users;
            _bankUserAccountsService = bankUserAccountsService;
            _um = um;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _um.FindByNameAsync(User.Identity.Name);

            if (await _um.IsInRoleAsync(user, "Administrator"))
            {
                var ba = await _bankUserAccountsService.All();
                return View(ba);
            }
            else
            {
                var ba = await _bankUserAccountsService.AllForUser(user.Id);
                return View(ba);
            }

        }

        public IActionResult Create()
        {

            var ba = new BankAccountCreateViewModel()
            {
                IsActive = true,
                Banks = _bankUserAccountsService.GetBanksList()
            };

            return View(ba);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BankAccountCreateViewModel bankAccount)
        {
            if (!ModelState.IsValid)
            {
                bankAccount.Banks = _bankUserAccountsService.GetBanksList();

                return View(bankAccount);
            }

            try
            {
                var user = (await _users.AllAsync(predicate: k => k.UserName == User.Identity.Name)).FirstOrDefault();

                var ba = Mapper.Map(bankAccount, new BankAccount(), opt => opt.AfterMap((src, dst) => dst.UserId = user.Id));

                await _bankUserAccountsService.Create(ba);
            }
            catch (Exception e) {

                ModelState.AddModelError("", e.Message);

                bankAccount.Banks = _bankUserAccountsService.GetBanksList();

                return View(bankAccount);
            }

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Edit(int id)
        {
            var b = (await _bankAccounts.AllAsync<BankAccountEditViewModel>(predicate: k => k.Id == id)).FirstOrDefault();

            b.Banks = _bankUserAccountsService.GetBanksList();


            return View(b);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BankAccountEditViewModel bankAccount)
        {
            if (!ModelState.IsValid)
            {
                bankAccount.Banks = _bankUserAccountsService.GetBanksList();
                return View(bankAccount);
            }

            try
            {

                var b = (await _bankAccounts.AllAsync(predicate: k => k.Id == bankAccount.Id)).FirstOrDefault();

                if (b == null)
                {
                    return NotFound();
                }

                Mapper.Map(bankAccount, b);

                await _bankAccounts.SaveChangesAsync();

            }
            catch (Exception e) {

                ModelState.AddModelError("", e.Message);

                bankAccount.Banks = _bankUserAccountsService.GetBanksList();

                return View(bankAccount);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddFunds(int id)
        {
            var ba = await _bankUserAccountsService.FindById(id);

            var afba = Mapper.Map(ba, new BankAccountAddFundsViewModel());

            return View(afba);
        }

        [HttpPost]
        public async Task<IActionResult> AddFunds(int id, BankAccountAddFundsViewModel funds)
        {

            if (!ModelState.IsValid) {
                return View(funds);
            }

            try
            {

                var ba = await _bankUserAccountsService.FindById(id);

                var user = await _um.FindByNameAsync(User.Identity.Name);

                await _bankUserAccountsService.AddFunds(user.Id, ba.UserId, ba.Number, funds.AddAmount);

            }
            catch (Exception e) {
                ModelState.AddModelError("", e.Message);
                return View(funds);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}