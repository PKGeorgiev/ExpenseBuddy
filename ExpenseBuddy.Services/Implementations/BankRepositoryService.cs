using ExpenseBuddy.Data;
using ExpenseBuddy.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseBuddy.Services.Implementations
{
    public class BankRepositoryService : GenericRepository<ExpenseBuddyDbContext, Bank, int>, IBankRepositoryService
    {
        public BankRepositoryService(ExpenseBuddyDbContext ctx) : base(ctx)
        {

        }
    }
}
