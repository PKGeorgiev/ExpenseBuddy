using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseBuddy.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseBuddy.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        //protected UserManager<ApplicationUser> _userManager;
        //protected ApplicationUser _user;

        public BaseController()
        {
            
        }
    }
}