using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseBuddy.Data.Models
{
    public enum ExpenseStatus
    {
        AwaitingPayment = 0,
        PayedPartially = 1,
        Payed = 2,
        Cancelled = 3
    }
}
