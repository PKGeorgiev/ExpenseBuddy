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
    public class ExpenseCreateViewModel : IMapFrom<Expense>, IHaveCustomMapping
    {
        [Required]
        [Display(Name = "Expense Date")]
        public DateTime ExpenseDate { get; set; }

        [Required]
        public ExpenseType Type { get; set; }

        [Required]
        [MaxLength(100)]
        public string Shop { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Fee { get; set; }

        public IEnumerable<SelectListItem> Elements { get; set; }

        [Required]
        [Display(Name = "Element")]
        public int ElementId { get; set; }

        public IEnumerable<SelectListItem> Owners { get; set; }

        [Required]
        [Display(Name = "Owner")]
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
                .CreateMap<ExpenseCreateViewModel, Expense>()
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ForMember(m => m.Owner, opt => opt.Ignore())
                .ForMember(m => m.Payers, opt => opt.Ignore())
                .ForMember(m => m.CreatedOn, opt => opt.MapFrom(mp => DateTime.Now));


        }
    }
}
