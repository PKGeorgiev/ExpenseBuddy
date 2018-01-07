using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseBuddy.Data.Models
{
    public enum ExpenseStatus
    {
        AwaitingPayment = 0,
        PayedPartially = 0,
        Payed = 1,
        Cancelled = 2
    }
}
