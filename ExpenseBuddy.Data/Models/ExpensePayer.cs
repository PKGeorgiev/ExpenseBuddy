using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseBuddy.Data.Models
{
    public class ExpensePayer
    {
        public int ExpenseId { get; set; }

        public Expense Expense { get; set; }

        public string PayerId { get; set; }

        public ApplicationUser Payer { get; set; }        
    }
}
