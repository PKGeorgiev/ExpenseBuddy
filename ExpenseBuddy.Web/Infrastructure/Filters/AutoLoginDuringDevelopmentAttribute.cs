using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using ExpenseBuddy.Data.Models;
using Microsoft.Extensions.Configuration;

namespace ExpenseBuddy.Web.Infrastructure.Filters
{
    public class AutoLoginDuringDevelopmentAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                var env = context.HttpContext.RequestServices.GetService<IHostingEnvironment>();
                if (env.IsDevelopment())
                {
                    var userManager = context.HttpContext.RequestServices.GetService<UserManager<ApplicationUser>>();
                    var signInManager = context.HttpContext.RequestServices.GetService<SignInManager<ApplicationUser>>();
                    var config = context.HttpContext.RequestServices.GetService<IConfiguration>();

                    var username = config["AutoLoginDuringDevelopment"];

                    var user = userManager.FindByNameAsync(username).Result;

                    if (user == null)
                    {
                        throw new Exception($"The user for autologin {username} was not found!");
                    }

                    return signInManager.SignInAsync(user, true);
                }
            }

            return Task.FromResult(0);
        }
    }
}
