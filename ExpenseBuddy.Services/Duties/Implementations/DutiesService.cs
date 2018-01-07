using ExpenseBuddy.Data;
using ExpenseBuddy.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace ExpenseBuddy.Services.Duties.Implementations
{
    public class DutiesService : IDutiesService
    {
        private readonly ExpenseBuddyDbContext _ctx;
        private readonly UserManager<ApplicationUser> _um;

        public DutiesService(
            ExpenseBuddyDbContext ctx,
            UserManager<ApplicationUser> um)
        {
            _ctx = ctx;
            _um = um;
        }

        public async Task<IEnumerable<ExpensePayer>> AllForUser(string userId)
        {
            return
                await _ctx
                .Payers
                .Include(p => p.Expense)
                    .ThenInclude(e => e.Owner)
                .Include(p => p.Expense)
                    .ThenInclude(e => e.Element)
                .Include(p => p.Payer)
                .Where(p => p.PayerId == userId )//&& (p.Expense.Status == ExpenseStatus.AwaitingPayment || p.Expense.Status == ExpenseStatus.PayedPartially))
                .ToListAsync();
        }

        public async Task<IEnumerable<ExpensePayer>> All()
        {
            return
                await _ctx
                .Payers
                .Include(p => p.Expense)
                    .ThenInclude(e => e.Owner)
                .Include(p => p.Expense)
                    .ThenInclude(e => e.Element)
                .Include(p => p.Payer)
                .ToListAsync();
        }


        public async Task<ExpensePayer> FindById(string userId, int expenseId, string payerId)
        {
            var du = await _ctx
                .Payers
                .Include(p => p.Expense)
                    .ThenInclude(e => e.Owner)
                .Include(p => p.Expense)
                    .ThenInclude(e => e.Element)
                .Include(p => p.Expense)
                    .ThenInclude(e => e.Payers)
                    .ThenInclude(k => k.Payer)
                .Include(p => p.Payer)
                .FirstOrDefaultAsync(k => k.ExpenseId == expenseId && k.PayerId == payerId);

            if (du == null)
            {
                throw new Exception($"The duty does not exist!");
            }
            
            if (userId != payerId) {
                var user = await _um.FindByIdAsync(userId);
                if ((await _um.IsInRoleAsync(user, "Administrator")) || du.Expense.OwnerId == userId)
                {
                    return du;
                }
                else {
                    throw new Exception("Access Denied!");
                }
            }

            return du;
        }

        public async Task Pay(string userId, int expenseId, string payerId)
        {
            var du = await FindById(userId, expenseId, payerId);

            var ba = await _ctx
                .BankAccounts
                .FirstOrDefaultAsync(k => k.UserId == payerId && k.Number == "500");

            if (ba.Amount < du.Amount) {
                throw new Exception("Insufficient availability in Payer's Bank Account!");
            }

            ba.Amount -= du.Amount;

            _ctx.BankAccounts.Update(ba);

            du.Status = PaymentStatus.Payed;
            du.PaymentDate = DateTime.Now;

            if (du.Expense.Payers.Any(p => p.Status == PaymentStatus.Unpayed))
            {
                du.Expense.Status = ExpenseStatus.PayedPartially;
            }
            else {
                du.Expense.Status = ExpenseStatus.Payed;
            }

            _ctx.Expenses.Update(du.Expense);

            _ctx.Payers.Update(du);
            await _ctx.SaveChangesAsync();

        }

    }
}
