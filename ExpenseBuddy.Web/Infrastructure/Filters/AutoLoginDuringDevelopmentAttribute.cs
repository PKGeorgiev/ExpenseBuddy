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
    public class AutoLoginDuringDevelopmentAttribute : ActionFilterAttribute
    {
        // Works together with AccountController.CompleteLogout()
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
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

                    if (!string.IsNullOrWhiteSpace(username))
                    {
                        var user = await userManager.FindByNameAsync(username);

                        if (user == null)
                        {
                            throw new Exception($"The user for autologin {username} was not found!");
                        }

                        await signInManager.SignInAsync(user, true);
                    }
                }
            }

            await next();
        } 

    }
}
