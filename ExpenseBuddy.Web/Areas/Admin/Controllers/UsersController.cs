using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExpenseBuddy.Services.Admin;
using ExpenseBuddy.Services.Admin.Models;
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

        public async Task<IActionResult> RemoveFromRole(string userId, string role)
        {

            await _users.RemoveFromRoleAsync(userId, role);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddToRole(string userId, string role)
        {

            await _users.AddToRoleAsync(userId, role);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string userId)
        {

            var user = await _users.FindByIdAsync(userId);

            var model = Mapper.Map<AdminUserListingServiceModel, EditUserViewModel>(user);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Edit));
            }

            string pass = null;

            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                if (user.Password == user.ConfirmPassword)
                {
                    pass = user.Password;
                }
                else
                {
                    ModelState.AddModelError("", "Passwords do not match!");
                    return View(user);
                }
            }

            await _users.UpdateUserAsync(user.Id, user.Username, user.Email, user.IsEnabled, pass);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create(string userId)
        {

            var user = new EditUserViewModel();

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EditUserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Edit));
            }

            string pass = null;

            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                if (user.Password == user.ConfirmPassword)
                {
                    pass = user.Password;
                }
                else
                {
                    ModelState.AddModelError("", "Passwords do not match!");
                    return View(user);
                }
            }

            await _users.CreateUserAsync(user.Username, user.Email, user.IsEnabled, pass);

            return RedirectToAction(nameof(Index));

        }
    }
}