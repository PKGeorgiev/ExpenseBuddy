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
    }
}
