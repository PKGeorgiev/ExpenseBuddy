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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAutoLoginDuringDevelopment(this IApplicationBuilder app, string username)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var env = serviceScope.ServiceProvider.GetService<IHostingEnvironment>();

                if (env.IsDevelopment())
                {

                    Task
                    .Run(async () =>
                    {

                        var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                        var signInManager = serviceScope.ServiceProvider.GetService<SignInManager<ApplicationUser>>();

                        var user = await userManager.FindByNameAsync(username);

                        if (user == null)
                        {
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
                var ctx = serviceScope.ServiceProvider.GetService<ExpenseBuddyDbContext>();

                ctx.Database.Migrate();

                var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var config = serviceScope.ServiceProvider.GetService<IConfiguration>();

                Task
                    .Run(async () =>
                    {
                        var adminName = WebConstants.AdministratorRole;

                        var roles = new[]
                        {
                            WebConstants.AdministratorRole,
                            WebConstants.EditorRole
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

                        Bank ibank = ctx.Banks.FirstOrDefault(b => b.Name == "Internal Bank");

                        if (ibank == null)
                        {

                            ibank = new Bank()
                            {
                                Name = "Internal Bank"
                            };

                            ctx.Banks.Add(ibank);
                            await ctx.SaveChangesAsync();
                        }

                        if (!ctx.Banks.Any(b => b.Name == "FI Bank"))
                        {
                            var bank = new Bank()
                            {
                                Name = "FI Bank"
                            };

                            ctx.Banks.Add(bank);
                            await ctx.SaveChangesAsync();
                        }

                        if (!ctx.Banks.Any(b => b.Name == "DSK Bank"))
                        {
                            var bank = new Bank()
                            {
                                Name = "DSK Bank"
                            };

                            ctx.Banks.Add(bank);
                            await ctx.SaveChangesAsync();
                        }


                        var users = userManager
                            .Users
                            .Include(ba => ba.BankAccounts)
                            .ToList();

                        foreach (var user in users)
                        {

                            if (!user.BankAccounts.Any())
                            {
                                var ba = new BankAccount()
                                {
                                    Amount = 0,
                                    Bank = ibank,
                                    User = user,
                                    IsActive = true,
                                    Number = "500",
                                    Notes = "Cash"
                                };

                                ctx.BankAccounts.Add(ba);
                                await ctx.SaveChangesAsync();
                            }

                        }

                        if (!ctx.ExpenseElements.Any())
                        {

                            var t = new List<ExpenseElement>()
                            {
                                new ExpenseElement()
                                {
                                    Name = "Food"
                                },
                                new ExpenseElement()
                                {
                                    Name = "Rent"
                                },
                                new ExpenseElement()
                                {
                                    Name = "Electricity"
                                },
                                new ExpenseElement()
                                {
                                    Name = "Water"
                                },
                                new ExpenseElement()
                                {
                                    Name = "Heating"
                                },
                                new ExpenseElement()
                                {
                                    Name = "Mobile Phone"
                                },
                                new ExpenseElement()
                                {
                                    Name = "Internet"
                                },
                                new ExpenseElement()
                                {
                                    Name = "Tax"
                                },
                                new ExpenseElement()
                                {
                                    Name = "Trash tax"
                                },
                                new ExpenseElement()
                                {
                                    Name = "Local tax"
                                },
                                new ExpenseElement()
                                {
                                    Name = "Cigarettes"
                                },
                                new ExpenseElement()
                                {
                                    Name = "Electronics"
                                },
                                new ExpenseElement()
                                {
                                    Name = "Other"
                                }
                            };

                            await ctx.ExpenseElements.AddRangeAsync(t);
                            await ctx.SaveChangesAsync();


                        }


                    })
                    .Wait();
            }

            return app;
        }
    }
}
