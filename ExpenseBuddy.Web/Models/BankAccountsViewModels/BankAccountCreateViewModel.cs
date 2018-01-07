﻿using AutoMapper;
using ExpenseBuddy.Common.Mapping;
using ExpenseBuddy.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseBuddy.Web.Models.BankAccountsViewModels
{
    public class BankAccountCreateViewModel : IMapFrom<BankAccount>, IHaveCustomMapping
    {
        public string Number { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public int BankId { get; set; }

        public ApplicationUser User { get; set; }

        public IEnumerable<SelectListItem> Banks { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper
                .CreateMap<BankAccountCreateViewModel, BankAccount>()
                .ForMember(m => m.Bank, opt => opt.Ignore());
        }
    }
}