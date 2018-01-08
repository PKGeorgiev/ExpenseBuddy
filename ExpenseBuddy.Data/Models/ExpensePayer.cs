using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExpenseBuddy.Data.Models
{
    public class ExpensePayer
    {
        [Required]
        public int ExpenseId { get; set; }

        public Expense Expense { get; set; }

        public string PayerId { get; set; }

        public ApplicationUser Payer { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}
