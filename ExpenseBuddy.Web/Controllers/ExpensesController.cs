using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseBuddy.Data;
using ExpenseBuddy.Data.Models;
using ExpenseBuddy.Web.Models.ExpensesViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using ExpenseBuddy.Services.Expenses;

namespace ExpenseBuddy.Web.Controllers
{
    public class ExpensesController : BaseController
    {
        private readonly ExpenseBuddyDbContext _ctx;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IExpenseService _expService;

        public ExpensesController(
            ExpenseBuddyDbContext ctx,
            UserManager<ApplicationUser> userManager,
            IExpenseService expService)
        {
            _ctx = ctx;
            _userManager = userManager;
            _expService = expService;
        }

        public async Task<IActionResult> Index(int? page, string search, string opt)
        {
            //var exp = await _expService.All();

            if (opt == "clear-filter")
            {
                ViewData["filter"] = "";
                search = "";
            }
            else {
                ViewData["filter"] = search;
            }

            var expPaged = await _expService.AllPaginated<Expense>(search, page, 3);

            //IEnumerable<ExpenseListingViewModel> items = new List<ExpenseListingViewModel>();

            //var expVm = Mapper.Map(expPaged, items);

            return View(expPaged);
        }

        public async Task<IActionResult> Create()
        {

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var p = await _expService.GetPayersList(0, user.Id);
            var o = _expService.GetOwnersList(user.Id);
            var e = await _expService.GetElementsList();

            var m = new ExpenseCreateViewModel()
            {
                Elements = e,
                Owners = o,
                Payers = p.ToList(),
                ExpenseDate = DateTime.Now
            };

            return View(m);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExpenseCreateViewModel expense)
        {
            if (!ModelState.IsValid)
            {
                return View(expense);
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var selectedPayers = expense
                .Payers
                .Where(k => k.Selected)
                .Select(k => k.Value);

            if (selectedPayers.Count() == 0)
            {

                ModelState.AddModelError("", "You must select at least one Payer!");

                var p = await _expService.GetPayersList(0, user.Id);
                var o = _expService.GetOwnersList("");
                var e = await _expService.GetElementsList();

                expense.Payers = p.ToList();
                expense.Owners = o;
                expense.Elements = e;

                return View(expense);

            }

            var ex = Mapper.Map(expense, new Expense());

            await _expService.CreateAsync(ex, expense.Payers);


            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {

            var ex = await _expService.FindById(id);

            if (ex == null) {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var o = _expService.GetOwnersList(ex.OwnerId);
            var p = await _expService.GetPayersList(ex.Id, user.Id);
            var e = await _expService.GetElementsList();

            var m = Mapper
                .Map(ex, new ExpenseEditViewModel() { }, opt 
                    => opt.BeforeMap((s, d) => 
                    {
                        d.Elements = e;
                        d.Payers = p.ToList();
                        d.Owners = o;
                    }));

            return View(m);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ExpenseEditViewModel expense)
        {

            if (!ModelState.IsValid)
            {
                return View(expense);
            }

            var selectedPayers = expense
                .Payers
                .Where(k => k.Selected)
                .Select(k => k.Value);

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (selectedPayers.Count() == 0)
            {
                var o = _expService.GetOwnersList(expense.OwnerId);
                var p = await _expService.GetPayersList(expense.Id, user.Id);
                var e = await _expService.GetElementsList();

                expense.Owners = o;
                expense.Payers = p.ToList();
                expense.Elements = e;

                ModelState.AddModelError("", "You must select at least one Payer!");
                return View(expense);
            }


            var expUpdated = Mapper.Map(expense, new Expense());

            try
            {
                await _expService.Update(id, expUpdated, expense.Payers);
            }
            catch (Exception exeption) {
                var o = _expService.GetOwnersList(expense.OwnerId);
                var p = await _expService.GetPayersList(expense.Id, user.Id);
                var e = await _expService.GetElementsList();

                expense.Owners = o;
                expense.Payers = p.ToList();
                expense.Elements = e;

                ModelState.AddModelError("", exeption.Message);
                return View(expense);
            }


            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var ex = await _ctx
                .Expenses
                .Include(k => k.Payers)
                    .ThenInclude(p => p.Payer)
                .Include(k => k.Owner)
                .Include(k => k.Element)
                .FirstOrDefaultAsync(k => k.Id == id);

            if (ex == null)
            {
                return NotFound();
            }

            return View(ex);
        }

    }
}