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
                .OrderBy(k => k.UserName)
                .ProjectTo<AdminUserListingServiceModel>()
                .ToListAsync();

            var allRoles = _roleManager.Roles.OrderBy(r => r.Name).Select(r => r.Name).ToList();

            var list = users
                .Select(c => {
                    var usr = _userManager.FindByIdAsync(c.Id).Result;
                    
                    c.Roles = _userManager.GetRolesAsync(usr).Result.OrderBy(k => k).ToList();

                    c.AvailableRoles = allRoles.Where(r => !c.Roles.Any(k => k == r)).ToList();

                    return c;
                });

            return list;
        }

        public async Task AddToRoleAsync(string userId, string role) {

            var user = await _userManager.FindByIdAsync(userId);

            await _userManager.AddToRoleAsync(user, role);

        }

        public async Task RemoveFromRoleAsync(string userId, string role)
        {

            var user = await _userManager.FindByIdAsync(userId);

            await _userManager.RemoveFromRoleAsync(user, role);

        }

        public async Task UpdateUserAsync(string userId, string username, string email, bool isEnabled, string password)
        {
            var user = await _userManager.FindByIdAsync(userId);

            user.UserName = username;
            user.Email = email;
            user.IsEnabled = isEnabled;

            await _userManager.UpdateAsync(user);

            if (password != null) {
                await _userManager.ResetPasswordAsync(user, null, password);
            }
        
        }

        public async Task<AdminUserListingServiceModel> FindByIdAsync(string userId)
        {
            return await _userManager.Users.Where(u => u.Id == userId).ProjectTo<AdminUserListingServiceModel>().FirstAsync();
        }

        public async Task CreateUserAsync(string username, string email, bool isEnabled, string password = null)
        {

            var user = new ApplicationUser() {
                UserName = username,
                Email = email,
                IsEnabled = isEnabled
            };

            var ir = await _userManager.CreateAsync(user, password);
            if (!ir.Succeeded) {
                throw new Exception($"{ir.Errors.FirstOrDefault().Code}: {ir.Errors.FirstOrDefault().Description}");
            }


            // Create default Cash bank account (500)
            var ba = new BankAccount()
            {
                Amount = 0,
                BankId = 1,
                IsActive = true,
                Notes = "Cash",
                Number = "500",
                UserId = user.Id
            };

            await _db.BankAccounts.AddAsync(ba);
            await _db.SaveChangesAsync();

        }
    }
}
