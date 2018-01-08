using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExpenseBuddy.Data.Models
{
    public enum ExpenseType
    {
        [Display(Name = "Purchase")]
        Purchase = 0,

        [Display(Name = "Utility fee")]
        UtilityFee = 1,

        [Display(Name = "Other")]
        Other = 2
    }
}
