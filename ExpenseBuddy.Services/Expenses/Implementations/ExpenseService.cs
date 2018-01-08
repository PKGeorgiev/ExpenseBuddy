using ExpenseBuddy.Data;
using ExpenseBuddy.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseBuddy.Services.Expenses.Implementations
{
    public class ExpenseService : IExpenseService
    {
        private readonly ExpenseBuddyDbContext _ctx;

        public ExpenseService(
            ExpenseBuddyDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<Expense>> All() {
            return await _ctx
                .Expenses
                .Include(k => k.Payers)
                    .ThenInclude(p => p.Payer)
                .Include(k => k.Owner)
                .Include(k => k.Element)
                .OrderBy(k => k.Status)
                .ThenByDescending(k => k.ExpenseDate)

                .ToListAsync();
        }

        public IEnumerable<SelectListItem> GetOwnersList(string selectedUserId) {
            return _ctx
                .Users
                .Select(k => new SelectListItem()
                {
                    Selected = selectedUserId == k.Id,
                    Text = k.UserName,
                    Value = k.Id
                }).ToList();
        }

        public async Task<IEnumerable<SelectListItem>> GetPayersList(int expenseId, string userId)
        {
            if (expenseId > 0)
            {

                var ex = await _ctx
                    .Expenses
                    .Include(k => k.Owner)
                    .Include(k => k.Payers)
                    .FirstOrDefaultAsync(k => k.Id == expenseId);

                return await _ctx
                    .Users
                    .Select(k => new SelectListItem()
                    {
                        Selected = ex.Payers.Any(p => p.PayerId == k.Id),
                        Text = k.UserName,
                        Value = k.Id
                    }).ToListAsync();
            }
            else {
                return _ctx
                    .Users
                    .Select(k => new SelectListItem()
                    {
                        Selected = userId == k.Id,
                        Text = k.UserName,
                        Value = k.Id
                    }).ToList();
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetElementsList() {
            return await _ctx.ExpenseElements
                    .Select(k => new SelectListItem()
                    {
                        Text = k.Name,
                        Value = k.Id.ToString()
                    }).ToListAsync();
        }

        public async Task CreateAsync(Expense expense, IEnumerable<SelectListItem> selectedPayers)
        {
            _ctx.Expenses.Add(expense);
            await _ctx.SaveChangesAsync();

            var sp = selectedPayers.Where(k => k.Selected == true).Select(k => k.Value).ToList();

            var payers = _ctx
                .Users
                .Where(k => sp.Any(p => k.Id == p))
                .Select(k => new ExpensePayer()
                {
                    PayerId = k.Id,
                    ExpenseId = expense.Id
                });

            await _ctx.Payers.AddRangeAsync(payers);

            decimal part = Math.Round((expense.Amount + expense.Fee) / expense.Payers.Count, 3);

            foreach (var p in expense.Payers)
            {
                p.Amount = part;
            };


            await _ctx.SaveChangesAsync();
        }

    }
}
