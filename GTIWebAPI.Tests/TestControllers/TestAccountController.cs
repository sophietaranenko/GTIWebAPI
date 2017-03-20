using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Account;
using Microsoft.AspNet.Identity;
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
            //ApplicationUserManager userManager = Owin
            var identity = new GenericIdentity("unit_testing");
            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);
            ApplicationUserManager manager = new ApplicationUserManager(new MockUserStore());
            var controller = new AccountController(manager);
            var result = controller.GetUserInfo() as Task<UserInfoViewModel>;
            //Assert.AreEqual(result.UserName, identity.Name);
        }


    }

    public class TestUser : IUser
    {
        public string Id
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }

        public string UserName
        {
            get
            {
                return "Test User";
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }

    public class MockUserStore : IUserStore<ApplicationUser> //where TestUser : class
    {
        public ApplicationUser FindById(string userId)
        {
            if (userId == null)
            {
                return new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "test" };
            }
            else
            {
                return new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "test" };
            }
        }

        public Task CreateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return Task.Factory.StartNew(() => new ApplicationUser { Id = userId, UserName = "sss", TableName = "Employee" });
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}
