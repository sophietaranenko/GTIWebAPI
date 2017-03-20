using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Account;
using GTIWebAPI.Models.Repository.Identity;
using GTIWebAPI.Novell;
using GTIWebAPI.Tests.Novell;
using GTIWebAPI.Tests.TestContext;
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
        private INovellManager novell;

        private TestDbContextFactory factory;

        private IAccountRepository repo;

        public TestAccountController()
        {
            novell = new TestNovellManager();
            factory = new TestDbContextFactory();
            repo = new AccountRepository(factory);
        }

        [TestMethod]
        public void GetUserInfoEmployee_ShouldRerurnUserInfo()
        {
            ApplicationUserManager manager = new ApplicationUserManager(new MockUserStore("Employee"));
            var controller = new AccountController(repo, novell, manager);
            var result = controller.GetUserInfo() as Task<UserInfoViewModel>;
            Assert.AreEqual(result.Result.TableName, "Employee");
            Assert.AreEqual(result.Result.EmployeeInformation, true);
        }

        [TestMethod]
        public void GetUserInfoOrganizationContactPerson_ShouldRerurnUserInfo()
        {
            TestDbContext db = new TestDbContext();
            db.OrganizationContactPersons.Add(new Models.Organizations.OrganizationContactPerson { Id = 1, OrganizationId = 222 });
            TestDbContextFactory factory = new TestDbContextFactory(db);

            ApplicationUserManager manager = new ApplicationUserManager(new MockUserStore("OrganizationContactPerson"));
            var controller = new AccountController(repo, novell, manager);
            var result = controller.GetUserInfo() as Task<UserInfoViewModel>;
            Assert.AreEqual(result.Result.TableName, "OrganizationContactPerson");
            Assert.AreEqual(result.Result.OrganizationId, 222);
        }

        [TestMethod]
        public void SimpleRegisterUser_ShouldReturnOkWhenRegistered()
        {

        }
    }

    public class MockUserStore : IUserStore<ApplicationUser> 
    {
        private string TableName { get; set; }
        public MockUserStore(string tableName)
        {
            TableName = tableName;
        }

        public ApplicationUser FindById(string userId)
        {
            return new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "test" };
        }

        public Task CreateAsync(ApplicationUser user)
        {
            return Task.Factory.StartNew(() => true);
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
            return Task.Factory.StartNew(() => new ApplicationUser { Id = userId, UserName = "sss", TableName = this.TableName, TableId = 1 });

        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailAsync(string userId, string subject, string body)
        {
            string Message = "";
            return Task.Factory.StartNew(() => Message = "This method imitates sending e-mail");
        }
    }
}
