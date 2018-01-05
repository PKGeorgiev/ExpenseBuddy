using ExpenseBuddy.Common.Mapping;
using ExpenseBuddy.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseBuddy.Services.Admin.Models
{
    public class AdminUserListingServiceModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsEnabled { get; set; }

        public IEnumerable<string> Roles { get; set; } = new List<string>();

        public IEnumerable<string> AvailableRoles { get; set; } = new List<string>();
    }
}
