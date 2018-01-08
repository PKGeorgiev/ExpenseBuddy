using AutoMapper;
using ExpenseBuddy.Common.Mapping;
using ExpenseBuddy.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseBuddy.Web.Models.BankAccountsViewModels
{
    public class BankAccountEditViewModel : IMapFrom<BankAccount>, IHaveCustomMapping
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Number { get; set; }

        [MaxLength(100)]
        public string Notes { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public int BankId { get; set; }

        [Required]
        public string UserId { get; set; }

        public decimal Amount { get; set; }

        public IEnumerable<SelectListItem> Banks { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper
                .CreateMap<BankAccountEditViewModel, BankAccount>()
                .ForMember(m => m.User, cfg => cfg.Ignore())
                .ForMember(m => m.Bank, cfg => cfg.Ignore())
                .ForMember(m => m.Amount, cfg => cfg.Ignore());
        }
    }
}
