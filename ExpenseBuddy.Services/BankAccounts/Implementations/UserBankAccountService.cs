using ExpenseBuddy.Data;
using ExpenseBuddy.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseBuddy.Services.BankAccounts.Implementations
{
    public class UserBankAccountService : IUserBankAccountService
    {
        private readonly ExpenseBuddyDbContext _ctx;
        private readonly UserManager<ApplicationUser> _um;

        public UserBankAccountService(
            ExpenseBuddyDbContext ctx,
            UserManager<ApplicationUser> um)
        {
            _ctx = ctx;
            _um = um;
        }

        public async Task<IEnumerable<BankAccount>> AllForUser(string userId)
        {
            return
                await _ctx
                .BankAccounts
                .Include(p => p.Bank)
                .Include(p => p.User)
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<BankAccount>> All()
        {
            return
                await _ctx
                .BankAccounts
                .Include(p => p.Bank)
                .Include(p => p.User)
                .ToListAsync();
        }

    }
}
