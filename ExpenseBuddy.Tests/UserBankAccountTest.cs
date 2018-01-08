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

namespace ExpenseBuddy.Tests
{
    public class UserBankAccountTest
    {

        public UserBankAccountTest()
        {
            Mapper.Initialize(a => a.AddProfile<AutoMapperProfile>());
        }

        [Fact]
        public async Task Test1() {


            var db = GetDbContext();
            var um = GetUserManager();
            var rm = GetRoleManager();

            for (int k = 1; k < 10; k++) {
                var user = new ApplicationUser()
                {
                    Id = k.ToString(),
                    UserName = $"User{k}",
                    Email = $"User{k}@user.com"
                };

                db.Users.Add(user);
            }

            await db.SaveChangesAsync();

            var ir = new IdentityRole() {
                Id = "1",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            };

            await db.Roles.AddAsync(ir);
            await db.SaveChangesAsync();

            var aus = new AdminUserService(db, um, rm);

            var list = await aus.AllAsync();


            int a = 1;

            a.Should().BeGreaterOrEqualTo(3);
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

    }
}
