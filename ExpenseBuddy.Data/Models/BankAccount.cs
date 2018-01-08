using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExpenseBuddy.Data.Models
{
    public class BankAccount
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Number { get; set; }

        [MaxLength(100)]
        public string Notes { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public int BankId { get; set; }

        public Bank Bank { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
