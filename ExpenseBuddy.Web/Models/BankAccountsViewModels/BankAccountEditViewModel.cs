using AutoMapper;
using ExpenseBuddy.Common.Mapping;
using ExpenseBuddy.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseBuddy.Web.Models.BankAccountsViewModels
{
    public class BankAccountEditViewModel : IMapFrom<BankAccount>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public int BankId { get; set; }

        public string UserId { get; set; }

        public decimal Amount { get; set; }

        public IEnumerable<SelectListItem> Banks { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper
                .CreateMap<BankAccountEditViewModel, BankAccount>()
                .ForMember(m => m.User, cfg => cfg.Ignore())
                .ForMember(m => m.Bank, cfg => cfg.Ignore());
        }
    }
}
