using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseBuddy.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseBuddy.Web.Controllers
{
    public class BankAccountsController : Controller
    {
        private readonly IBankAccountRepositoryService _bankAccounts;

        public BankAccountsController(IBankAccountRepositoryService bankAccounts)
        {
            _bankAccounts = bankAccounts;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}