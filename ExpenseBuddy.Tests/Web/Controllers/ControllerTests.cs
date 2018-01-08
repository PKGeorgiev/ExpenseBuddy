using System;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExpenseBuddy.Data;
using ExpenseBuddy.Data.Models;
using ExpenseBuddy.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace ExpenseBuddy.Tests.Web.Controllers
{
    public class ControllerTests
    {
        [Fact]
        public void BankAccountsControllerAuthorized()
        {
            var attr = typeof(BankAccountsController)
                .GetCustomAttributes(true);


            CheckAuthAttr(attr);
        }

        [Fact]
        public void BaseControllerAuthorized()
        {
            var attr = typeof(BaseController)
                .GetCustomAttributes(true);

            CheckAuthAttr(attr);

        }

        [Fact]
        public void DutiesControllerAuthorized()
        {
            var attr = typeof(DutiesController)
                .GetCustomAttributes(true);

            CheckAuthAttr(attr);

        }

        [Fact]
        public void ExpensesControllerAuthorized()
        {
            var attr = typeof(ExpensesController)
                .GetCustomAttributes(true);

            CheckAuthAttr(attr);

        }


        private void CheckAuthAttr(object[] attr)
        {
            attr
                .Should()
                .Match(a => a.Any(b => b.GetType() == typeof(AuthorizeAttribute)));
        }
    }
}
