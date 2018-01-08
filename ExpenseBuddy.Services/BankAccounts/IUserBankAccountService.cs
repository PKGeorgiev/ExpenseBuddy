using ExpenseBuddy.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseBuddy.Services.BankAccounts
{
    public interface IUserBankAccountService
    {
        Task<IEnumerable<BankAccount>> AllForUser(string userId);
        Task<IEnumerable<BankAccount>> All();
        Task<BankAccount> FindByNumber(string ownerId, string bankAccountNumber);
        Task<BankAccount> FindById(int bankAccountId);
        Task AddFunds(string userId, string ownerId, string bankAccountNumber, decimal amount);
        Task GetFunds(string userId, string ownerId, string bankAccountNumber, decimal amount);
        Task Create(BankAccount ba);
    }
}
