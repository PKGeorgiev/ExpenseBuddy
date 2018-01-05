using ExpenseBuddy.Services.Admin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseBuddy.Services.Admin
{
    public interface IAdminUserService
    {
        Task<IEnumerable<AdminUserListingServiceModel>> AllAsync();
        Task AddToRoleAsync(string userId, string role);
        Task RemoveFromRoleAsync(string userId, string role);
        Task UpdateUserAsync(string userId, string username, string email, bool isEnabled, string password = null);
        Task CreateUserAsync(string username, string email, bool isEnabled, string password = null);
        Task<AdminUserListingServiceModel> FindByIdAsync(string userId);
    }
}
