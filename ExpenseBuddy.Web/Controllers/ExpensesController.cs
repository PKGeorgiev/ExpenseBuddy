using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseBuddy.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseBuddy.Web.Controllers
{
    public class ExpensesController : Controller
    {
        public IActionResult Index()
        {
            return View(new List<Expense>());
        }

        public IActionResult Create()
        {
            return View(new Expense());
        }
    }
}