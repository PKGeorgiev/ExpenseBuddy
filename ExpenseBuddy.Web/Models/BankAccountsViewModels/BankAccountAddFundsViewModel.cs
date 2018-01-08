using AutoMapper;
using ExpenseBuddy.Common.Mapping;
using ExpenseBuddy.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseBuddy.Web.Models.BankAccountsViewModels
{
    public class BankAccountAddFundsViewModel : IMapFrom<BankAccount>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public string Notes { get; set; }

        public decimal Amount { get; set; }

        public bool IsActive { get; set; }

        public string Bank { get; set; }

        public string User { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal AddAmount { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper
                .CreateMap<BankAccount, BankAccountAddFundsViewModel>()
                .ForMember(m => m.Bank, opt => opt.MapFrom(k => k.Bank.Name))
                .ForMember(m => m.User, opt => opt.MapFrom(k => k.User.UserName))
                .ForMember(m => m.AddAmount, opt => opt.Ignore());
        }
    }
}
