using ExpenseBuddy.Common.Mapping;
using ExpenseBuddy.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseBuddy.Web.Models.ExpensesViewModels
{
    public class ExpenseListingViewModel : IMapFrom<Expense>
    {
        public int Id { get; set; }

        [Display(Name = "Expense Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ExpenseDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Created On")]
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

        public ExpenseStatus Status { get; set; }

        public ICollection<ExpensePayer> Payers { get; set; }
    }
}
