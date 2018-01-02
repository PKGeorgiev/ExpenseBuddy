namespace LearningSystem.Web.Infrastructure.Extensions
{
    //using AutoMapper.Configuration;
    using ExpenseBuddy.Data;
    using ExpenseBuddy.Data.Models;
    using ExpenseBuddy.Web;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading.Tasks;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAutoLoginDuringDevelopment(this IApplicationBuilder app, string username)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var env = serviceScope.ServiceProvider.GetService<IHostingEnvironment>();

                if (env.IsDevelopment()) {

                    Task
                    .Run(async () =>
                    {

                        var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                        var signInManager = serviceScope.ServiceProvider.GetService<SignInManager<ApplicationUser>>();

                        var user = await userManager.FindByNameAsync(username);

                        if (user == null) {
                            throw new Exception($"The user {username} was not found!");
                        }

                        await signInManager.SignInAsync(user, true);

                    }).Wait();

                }

                return app;
            }

        }


                public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<ExpenseBuddyDbContext>().Database.Migrate();

                var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var config = serviceScope.ServiceProvider.GetService<IConfiguration>();
                
                Task
                    .Run(async () =>
                    {
                        var adminName = WebConstants.AdministratorRole;

                        var roles = new[]
                        {
                            WebConstants.AdministratorRole
                        };

                        foreach (var role in roles)
                        {
                            var roleExists = await roleManager.RoleExistsAsync(role);

                            if (!roleExists)
                            {
                                await roleManager.CreateAsync(new IdentityRole
                                {
                                    Name = role
                                });
                            }
                        }

                        var adminEmail = config["init:accounts:administrator:mail"];

                        var adminUser = await userManager.FindByEmailAsync(adminEmail);

                        if (adminUser == null)
                        {
                            adminUser = new ApplicationUser
                            {
                                Email = adminEmail,
                                UserName = adminName
                            };

                            var adminPwd = config["secrets:accounts:administrator"];

                            await userManager.CreateAsync(adminUser, adminPwd);

                            await userManager.AddToRoleAsync(adminUser, adminName);
                        } 
                    })
                    .Wait();
            }

            return app;
        }
    }
}
