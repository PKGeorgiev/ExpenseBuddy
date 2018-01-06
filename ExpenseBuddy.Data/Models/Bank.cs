using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExpenseBuddy.Data.Models
{
    public class Bank
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(512)]
        public string Notes { get; set; }

        public IEnumerable<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
    }
}
