using ExpenseBuddy.Data;
using ExpenseBuddy.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace ExpenseBuddy.Services.Duties.Implementations
{
    public class DutiesService : IDutiesService
    {
        private readonly ExpenseBuddyDbContext _ctx;

        public DutiesService(
            ExpenseBuddyDbContext ctx)
        {
            _ctx = ctx;
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
                .Where(p => p.PayerId == userId && (p.Expense.Status == ExpenseStatus.AwaitingPayment || p.Expense.Status == ExpenseStatus.PayedPartially) )
                .ToListAsync();
        }

    }
}
