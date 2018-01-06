using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseBuddy.Data.Models
{
    public class Expense
    {
        public int Id { get; set; }

        public DateTime ExpenseDate { get; set; }

        public DateTime CreatedOn { get; set; }

        public ExpenseType Type { get; set; }

        public string Shop { get; set; }

        public decimal Amount { get; set; }

        public decimal Fee { get; set; }

        public ExpenseElement Element { get; set; }

        public int ElementId { get; set; }

        public ApplicationUser Owner { get; set; }

        public string OwnerId { get; set; }

        public string Notes { get; set; }

        public string Reference { get; set; }

        public ICollection<ExpensePayer> Payers { get; set; }
    }
}
