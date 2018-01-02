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
    // Works together with AccountController.CompleteLogout()
    public class AutoLoginDuringDevelopmentAttribute : ActionFilterAttribute
    {
        private readonly UserManager<ApplicationUser> _userManager;        
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _conf;

        public AutoLoginDuringDevelopmentAttribute(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IHostingEnvironment env,
            IConfiguration conf)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
            _conf = conf;
        }
        
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                if (_env.IsDevelopment())
                {
                    var username = _conf["AutoLoginDuringDevelopment"];

                    if (!string.IsNullOrWhiteSpace(username))
                    {
                        var user = await _userManager.FindByNameAsync(username);

                        if (user == null)
                        {
                            throw new Exception($"The user for autologin {username} was not found!");
                        }

                        await _signInManager.SignInAsync(user, true);
                    }
                }
            }

            await next();
        } 

    }
}
