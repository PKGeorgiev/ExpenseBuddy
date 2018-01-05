using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseBuddy.Services.Admin;
using ExpenseBuddy.Web.Areas.Admin.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ExpenseBuddy.Web.Areas.Admin.Controllers
{
    public class UsersController : BaseAdminController
    {

        private readonly IAdminUserService _users;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(IAdminUserService users, RoleManager<IdentityRole> roleManager)
        {
            _users = users;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {

            var users = await this._users.AllAsync();
            var roles = await this._roleManager
                .Roles
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                })
                .ToListAsync();

            return View(new UserListingsViewModel
            {
                Users = users,
                Roles = roles
            });

        }
    }
}