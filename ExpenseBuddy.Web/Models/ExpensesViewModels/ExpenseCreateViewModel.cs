using AutoMapper;
using ExpenseBuddy.Common.Mapping;
using ExpenseBuddy.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseBuddy.Web.Models.ExpensesViewModels
{
    public class ExpenseCreateViewModel : IMapFrom<Expense>
    {
        public DateTime ExpenseDate { get; set; }

        public ExpenseType Type { get; set; }

        public string Shop { get; set; }

        public decimal Amount { get; set; }

        public decimal Fee { get; set; }

        public IEnumerable<SelectListItem> Elements { get; set; }

        public int ElementId { get; set; }

        public IEnumerable<SelectListItem> Owners { get; set; }

        public string OwnerId { get; set; }

        public string Notes { get; set; }

        public string Reference { get; set; }

        public ExpenseStatus Status { get; set; }

        public IList<SelectListItem> Payers { get; set; }

    }
}
