using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ExpenseBuddy.Data.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public bool IsEnabled { get; set; }

        public IEnumerable<BankAccount> BankAccounts { get; set; }

        public IEnumerable<Expense> OwnedExpenses { get; set; }

        public IEnumerable<ExpensePayer> Expenses { get; set; }
    }
}
