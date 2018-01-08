using ExpenseBuddy.Common.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExpenseBuddy.Data.Models
{
    public class Expense
    {
        public int Id { get; set; }

        [Required]
        public DateTime ExpenseDate { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public ExpenseType Type { get; set; }

        [MaxLength(50)]
        public string Shop { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Fee { get; set; }

        public ExpenseElement Element { get; set; }

        [Required]
        public int ElementId { get; set; }

        public ApplicationUser Owner { get; set; }

        public string OwnerId { get; set; }

        [MaxLength(100)]
        public string Notes { get; set; }

        [MaxLength(100)]
        public string Reference { get; set; }

        public ExpenseStatus Status { get; set; }

        public ICollection<ExpensePayer> Payers { get; set; }
    }
}
