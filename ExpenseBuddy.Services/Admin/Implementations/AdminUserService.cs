using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using ExpenseBuddy.Data;
using ExpenseBuddy.Data.Models;
using ExpenseBuddy.Services.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExpenseBuddy.Services.Admin.Implementations
{
    public class AdminUserService : IAdminUserService
    {

        private readonly ExpenseBuddyDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminUserService(
            ExpenseBuddyDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this._db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<AdminUserListingServiceModel>> AllAsync()
        {
            var users = await this._db
                .Users
                .ProjectTo<AdminUserListingServiceModel>()
                .ToListAsync();          

            var list = users
                .Select(c => {
                    c.Roles = _userManager.GetRolesAsync(_userManager.FindByIdAsync(c.Id).Result).Result.ToList();
                    return c;
                });

            return list;
        }
    }
}
