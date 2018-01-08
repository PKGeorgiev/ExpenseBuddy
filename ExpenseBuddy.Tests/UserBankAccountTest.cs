using System;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExpenseBuddy.Data;
using ExpenseBuddy.Data.Models;
using Moq;
using Microsoft.AspNetCore.Identity;
using ExpenseBuddy.Services.Admin.Implementations;
using AutoMapper;
using LearningSystem.Web.Infrastructure.Mapping;
using System.Threading;
using System.Collections.Generic;
using ExpenseBuddy.Services.Expenses.Implementations;

namespace ExpenseBuddy.Tests
{
    public class UserBankAccountTest
    {

        public UserBankAccountTest()
        {
            Mapper.Initialize(a => a.AddProfile<AutoMapperProfile>());
        }

        [Fact]
        public async Task ExpenseService_ShouldHaveCount10() {

            var db = GetDbContext();

            await SeedDb(db);

            var es = new ExpenseService(db);

            var list = await es.All();

            list.Should().HaveCount(10);

        }

        [Fact]
        public async Task ExpenseService_ShouldHaveCount0()
        {

            var db = GetDbContext();

            var es = new ExpenseService(db);

            var list = await es.All();

            list.Should().BeEmpty();

        }

        private ExpenseBuddyDbContext GetDbContext() {
            var dbOptions = new DbContextOptionsBuilder<ExpenseBuddyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ExpenseBuddyDbContext(dbOptions);
        }

        private UserManager<ApplicationUser> GetUserManager()
        {
            var t = new Mock<UserManager<ApplicationUser>>(
                    Mock.Of<IUserStore<ApplicationUser>>(),
                    null, null, null, null, null, null, null, null
                );

            return t.Object;
        }

        private RoleManager<IdentityRole> GetRoleManager()
        {
            var ct = new CancellationToken();
            var store = new Mock<IUserStore<ApplicationUser>>(MockBehavior.Strict);

            store.As<IUserPasswordStore<ApplicationUser>>()
                    .Setup(x => x.FindByNameAsync("user1", ct))
                    .ReturnsAsync((ApplicationUser)null);

            var t = new Mock<RoleManager<IdentityRole>>(
                    Mock.Of<IRoleStore<IdentityRole>>(),
                    null, null, null, null
                );

            return t.Object;
        }

        private async Task SeedDb(ExpenseBuddyDbContext db) {
            for (int k = 1; k <= 10; k++)
            {
                var user = new ApplicationUser()
                {
                    Id = k.ToString(),
                    UserName = $"User{k}",
                    Email = $"User{k}@user.com"
                };

                db.Users.Add(user);
            }

            await db.SaveChangesAsync();

            var ir = new IdentityRole()
            {
                Id = "1",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            };

            await db.Roles.AddAsync(ir);
            await db.SaveChangesAsync();


            var elements = new List<ExpenseElement>()
            {
                new ExpenseElement(){
                    Id = 1,
                    Name = "Tax"
                },
                new ExpenseElement(){
                    Id = 2,
                    Name = "Heating"
                },
                new ExpenseElement(){
                    Id = 3,
                    Name = "Internet"
                }
            };

            await db.ExpenseElements.AddRangeAsync(elements);
            await db.SaveChangesAsync();

            for (var k = 1; k <= 10; k++) {
                var exp = new Expense()
                {
                    Id = k,
                    ElementId = 1,
                    ExpenseDate = DateTime.Now.AddDays(k),
                    OwnerId = "1",
                    Shop = "Shop 1",
                    Status = ExpenseStatus.AwaitingPayment,
                    Type = ExpenseType.Purchase
                };

                db.Expenses.Add(exp);
                await db.SaveChangesAsync();

                var ep = new ExpensePayer()
                {
                    ExpenseId = k,
                    PayerId = k.ToString(),
                    Status = PaymentStatus.Unpayed
                };

                db.Payers.Add(ep);
                await db.SaveChangesAsync();

            }

            




        }

    }
}
