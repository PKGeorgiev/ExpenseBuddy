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

        public async Task<IActionResult> Index()
        {
            var exp = await _expService.All();

            return View(exp);
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

            var ex = await _ctx
                .Expenses
                .Include(k => k.Owner)
                .Include(k => k.Payers)
                .FirstOrDefaultAsync(k => k.Id == id);

            if (ex == null) {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var owners = _expService.GetOwnersList(ex.OwnerId);

            var payers = await _expService.GetPayersList(ex.Id, user.Id);

            var elements = await _expService.GetElementsList();

            var m = Mapper
                .Map(ex, new ExpenseEditViewModel() { }, opt 
                    => opt.BeforeMap((s, d) => 
                    {
                        d.Elements = elements;
                        d.Payers = payers.ToList();
                        d.Owners = owners;
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

            if (selectedPayers.Count() == 0)
            {

                ModelState.AddModelError("", "You must select at least one Payer!");
                return View(expense);

            }

            var ex = await _ctx
                .Expenses
                .Include(k => k.Owner)
                .Include(k => k.Payers)
                .FirstOrDefaultAsync(k => k.Id == id);

            if (ex == null) {
                return NotFound();
            }

            var removedPayers =
                ex.Payers
                .Where(k => !selectedPayers.Contains(k.PayerId))
                .ToList();

            var addedPayers =
                selectedPayers
                .Where(k => !ex.Payers.Any(p => p.PayerId == k))
                .Select(k => new ExpensePayer()
                {
                    ExpenseId = ex.Id,
                    PayerId = k
                });

            foreach (var rp in removedPayers) {
                ex.Payers.Remove(rp);
            }

            foreach (var ap in addedPayers)
            {
                ex.Payers.Add(ap);
            }

            decimal part = Math.Round((expense.Amount + expense.Fee) / selectedPayers.Count(), 3);

            foreach (var p in ex.Payers)
            {
                p.Amount = part;
            };

            Mapper.Map(expense, ex);

            _ctx.Expenses.Update(ex);
            await _ctx.SaveChangesAsync();

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