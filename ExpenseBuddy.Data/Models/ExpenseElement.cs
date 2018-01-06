using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseBuddy.Data.Models
{
    public class ExpenseElement
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Expense> Expenses { get; set; }
    }
}
