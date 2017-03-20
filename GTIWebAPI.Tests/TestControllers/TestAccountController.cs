using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Account;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GTIWebAPI.Tests.TestControllers
{
    [TestClass]
    public class TestAccountController
    {

        [TestMethod]
        public void GetUserInfo_ShouldRerurnUserInfo()
        {
            ApplicationUserManager userManager = Owin
            var identity = new GenericIdentity("unit_testing");
            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);
            var controller = new AccountController();
            var result = controller.GetUserInfo() as UserInfoViewModel;
            Assert.AreEqual(result.UserName, identity.Name);
        }
    }
}
