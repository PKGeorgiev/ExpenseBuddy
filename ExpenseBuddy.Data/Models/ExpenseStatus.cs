using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExpenseBuddy.Data.Models
{
    public enum ExpenseStatus
    {
        [Display(Name = "Awaiting payment")]
        AwaitingPayment = 0,

        [Display(Name = "Payed partially")]
        PayedPartially = 1,

        [Display(Name = "Payed")]
        Payed = 2,

        [Display(Name = "Cancelled")]
        Cancelled = 3
    }
}
