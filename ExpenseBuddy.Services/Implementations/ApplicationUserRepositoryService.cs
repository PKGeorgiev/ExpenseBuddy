using ExpenseBuddy.Data;
using ExpenseBuddy.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseBuddy.Services.Implementations
{
    public class ApplicationUserRepositoryService : GenericRepository<ExpenseBuddyDbContext, ApplicationUser, int>, IApplicationUserRepositoryService
    {
        public ApplicationUserRepositoryService(ExpenseBuddyDbContext ctx) : base(ctx)
        {

        }
    }
}
