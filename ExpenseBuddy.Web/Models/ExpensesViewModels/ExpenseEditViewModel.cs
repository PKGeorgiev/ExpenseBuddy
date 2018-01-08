using AutoMapper;
using ExpenseBuddy.Common.Mapping;
using ExpenseBuddy.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseBuddy.Web.Models.ExpensesViewModels
{
    public class ExpenseEditViewModel : IMapFrom<Expense>, IHaveCustomMapping
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Expense date")]
        public DateTime ExpenseDate { get; set; }

        [Required]
        public ExpenseType Type { get; set; }

        [MaxLength(100)]
        public string Shop { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Fee { get; set; }

        public IEnumerable<SelectListItem> Elements { get; set; }

        [Required]
        public int ElementId { get; set; }

        public IEnumerable<SelectListItem> Owners { get; set; }

        [Required]
        public string OwnerId { get; set; }

        [MaxLength(100)]
        public string Notes { get; set; }

        [MaxLength(100)]
        public string Reference { get; set; }

        [Required]
        public ExpenseStatus Status { get; set; }

        public IList<SelectListItem> Payers { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper
                .CreateMap<Expense, ExpenseEditViewModel>()
                .ForMember(m => m.Elements, opt => opt.Ignore())
                .ForMember(m => m.Owners, opt => opt.Ignore())
                .ForMember(m => m.Payers, opt => opt.Ignore());

            mapper
                .CreateMap<ExpenseEditViewModel, Expense>()
                .ForMember(m => m.Payers, opt => opt.Ignore())
                .ForMember(m => m.Status, opt => opt.Ignore())
                .ForMember(m => m.Id, opt => opt.Ignore());

        }
    }
}
