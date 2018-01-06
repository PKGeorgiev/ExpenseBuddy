using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseBuddy.Data.Models
{
    public class BankAccount
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public string Notes { get; set; }

        public decimal Amount { get; set; }

        public bool IsActive { get; set; }

        public int BankId { get; set; }

        public Bank Bank { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
