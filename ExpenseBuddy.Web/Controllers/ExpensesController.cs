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

namespace ExpenseBuddy.Web.Controllers
{
    public class ExpensesController : BaseController
    {
        private readonly ExpenseBuddyDbContext _ctx;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExpensesController(
            ExpenseBuddyDbContext ctx,
            UserManager<ApplicationUser> userManager)
        {
            _ctx = ctx;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var exp = await _ctx
                .Expenses
                .Include(k => k.Payers)
                    .ThenInclude(p => p.Payer)
                .Include(k => k.Owner)
                .Include(k => k.Element)
                .ToListAsync();


            return View(exp);
        }

        public IActionResult Create()
        {

            var u = _ctx
                .Users
                .Select(k => new SelectListItem()
                {
                    Selected = User.Identity.Name == k.UserName,
                    Text = k.UserName,
                    Value = k.Id
                }).ToList();

            var m = new ExpenseCreateViewModel()
            {
                Elements = _ctx.ExpenseElements
                    .Select(k => new SelectListItem()
                    {
                        Text = k.Name,
                        Value = k.Id.ToString()
                    }),

                Owners = u,
                Payers = u,
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

            var ex = new Expense()
            {
                Amount = expense.Amount,
                CreatedOn = DateTime.Now,
                ElementId = expense.ElementId,
                ExpenseDate = expense.ExpenseDate,
                Fee = expense.Fee,
                Notes = expense.Notes,
                OwnerId = expense.OwnerId,
                Reference = expense.Reference,
                Shop = expense.Shop,
                Type = expense.Type,
                Status = ExpenseStatus.AwaitingPayment
            };

            _ctx.Expenses.Add(ex);
            

            var sp = expense.Payers.Where(k => k.Selected == true).Select(k => k.Value).ToList();

            var payers = _ctx
                .Users
                .Where(k => sp.Any(p => k.Id == p))
                .Select(k => new ExpensePayer()
                {
                    PayerId = k.Id,
                    ExpenseId = ex.Id
                });

            await _ctx.Payers.AddRangeAsync(payers);

            decimal part = Math.Round((expense.Amount + expense.Fee) / ex.Payers.Count, 3);

            foreach (var p in ex.Payers)
            {
                p.Amount = part;
            };


            await _ctx.SaveChangesAsync();


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

            var owners = _ctx
                .Users
                .Select(k => new SelectListItem()
                {
                    Selected = ex.OwnerId == k.Id,
                    Text = k.UserName,
                    Value = k.Id
                }).ToList();

            var payers = _ctx
                .Users
                .Select(k => new SelectListItem()
                {
                    Selected = ex.Payers.Any(p => p.PayerId == k.Id),
                    Text = $"{k.UserName}",
                    Value = k.Id
                }).ToList();

            var elements = _ctx.ExpenseElements
                    .Select(k => new SelectListItem()
                    {
                        Text = k.Name,
                        Value = k.Id.ToString()
                    });

            var m = Mapper
                .Map(ex, new ExpenseEditViewModel() { }, opt 
                    => opt.BeforeMap((s, d) => 
                    {
                        d.Elements = elements;
                        d.Payers = payers;
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

            var ex = await _ctx
                .Expenses
                .Include(k => k.Owner)
                .Include(k => k.Payers)
                .FirstOrDefaultAsync(k => k.Id == id);

            if (ex == null) {
                return NotFound();
            }


            var selectedPayers = expense
                .Payers
                .Where(k => k.Selected)
                .Select(k => k.Value);

            if (selectedPayers.Count() == 0) {

                ModelState.AddModelError("", "You must select at least one Payer!");
                return View(expense);

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