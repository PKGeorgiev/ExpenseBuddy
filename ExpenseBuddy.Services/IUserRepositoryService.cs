using ExpenseBuddy.Data;
using ExpenseBuddy.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseBuddy.Services
{
    public interface IUserRepositoryService : IGenericRepository<ExpenseBuddyDbContext, ApplicationUser, int>
    {
    }
}
