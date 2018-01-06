using ExpenseBuddy.Data;
using ExpenseBuddy.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseBuddy.Services.Implementations
{
    public class BankAccountRepositoryService : GenericRepository<ExpenseBuddyDbContext, BankAccount, int>, IBankAccountRepositoryService
    {
        public BankAccountRepositoryService(ExpenseBuddyDbContext ctx) : base(ctx)
        {

        }
    }
}
