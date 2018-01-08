using ExpenseBuddy.Data;
using ExpenseBuddy.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        private async Task CheckOwnerOrAdmin(string userId, string ownerId)
        {
            if (userId != ownerId)
            {
                if (!(await _um.IsInRoleAsync(await _um.FindByIdAsync(userId), "Administrator")))
                {
                    throw new Exception("Access Denied!");
                }
            }
        }

        public async Task<BankAccount> FindById(int bankAccountId)
        {
            var ba = await _ctx
                .BankAccounts
                .Include(p => p.Bank)
                .Include(p => p.User)
                .FirstOrDefaultAsync(k => k.Id == bankAccountId);

            if (ba == null)
            {
                throw new Exception("");
            }

            return ba;
        }

        public async Task<BankAccount> FindByNumber(string ownerId, string bankAccountNumber)
        {
            var ba = await _ctx
                .BankAccounts
                .Include(p => p.Bank)
                .Include(p => p.User)
                .FirstOrDefaultAsync(k => k.UserId == ownerId && k.Number == bankAccountNumber);

            if (ba == null)
            {
                throw new Exception("");
            }

            return ba;
        }

        public async Task AddFunds(string userId, string ownerId, string bankAccountNumber, decimal amount)
        {
            var ba = await FindByNumber(ownerId, bankAccountNumber);

            await CheckOwnerOrAdmin(userId, ba.UserId);

            ba.Amount += amount;

            await _ctx.SaveChangesAsync();
        }

        public async Task GetFunds(string userId, string ownerId, string bankAccountNumber, decimal amount)
        {
            var ba = await FindByNumber(ownerId, bankAccountNumber);

            await CheckOwnerOrAdmin(userId, ba.UserId);

            if (ba.Amount < amount)
            {
                throw new Exception("Insufficient funds!");
            }

            ba.Amount -= amount;

            await _ctx.SaveChangesAsync();
        }

        public async Task Create(BankAccount ba)
        {

            await _ctx.BankAccounts.AddAsync(ba);
            await _ctx.SaveChangesAsync();
        }

        public IEnumerable<SelectListItem> GetBanksList()
        {
            return _ctx
                .Banks
                .Where(w => w.Name != "Internal Bank")
                .Select(k => new SelectListItem()
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                }).ToList();
        }

    }
}
