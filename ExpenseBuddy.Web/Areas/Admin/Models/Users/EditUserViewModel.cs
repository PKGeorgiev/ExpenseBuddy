using ExpenseBuddy.Common.Mapping;
using ExpenseBuddy.Services.Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseBuddy.Web.Areas.Admin.Models.Users
{
    public class EditUserViewModel : IMapFrom<AdminUserListingServiceModel>
    {
        public string Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool IsEnabled { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
