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
using System.Web.Http;
using System.Web.Http.Results;
using System.Security.Claims;
using GTIWebAPI.Models.Organizations;




namespace GTIWebAPI.Tests.TestControllers
{
    [TestClass]
    public class TestAccountController
    {
        private INovellManager novell;

        private TestDbContextFactory factory;

        private IAccountRepository repo;

        private TestDbContext db;

        public TestAccountController()
        {
            novell = new TestNovellManager();
            db = new TestDbContext();
            factory = new TestDbContextFactory(db);
            repo = new AccountRepository(factory);
            FillDemo();
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
            ApplicationUserManager manager = new ApplicationUserManager(new MockUserStore("OrganizationContactPerson"));
            var controller = new AccountController(repo, novell, manager);
            var result = controller.GetUserInfo() as Task<UserInfoViewModel>;
            Assert.AreEqual(result.Result.TableName, "OrganizationContactPerson");
            Assert.AreEqual(result.Result.OrganizationId, 222);
        }

        [TestMethod]
        public void SimpleRegisterUser_ShouldReturnOkWhenRegistered()
        {
            ApplicationUserManager manager = new ApplicationUserManager(new MockUserStore("Employee"));
            var controller = new AccountController(repo, novell, manager);
            var result = controller.SimpleRegisterOrganizationContactPerson(22) as Task<IHttpActionResult>;
        }

        private void FillDemo()
        {
            db.OrganizationContactPersons.Add(new Models.Organizations.OrganizationContactPerson { Id = 1, OrganizationId = 222 });
            db.OrganizationContactPersonViews.Add(new Models.Organizations.OrganizationContactPersonView { Id = 22, FirstName = "Тестируем транслитерацию", LastName = "Wow, but if it is too long?", Email = "sometestemail@belongstoperson.com" });
        }
    }

    

    public class MockUserStore : IUserStore<ApplicationUser>, 
        IUserPasswordStore<ApplicationUser>, 
        IUserEmailStore<ApplicationUser>, 
        IUserClaimStore<ApplicationUser>,
        IUserLoginStore<ApplicationUser>,
        IUserRoleStore<ApplicationUser>
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

        public Task CreateAsync(ApplicationUser user, string password)
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
            return Task.Factory.StartNew(() =>
            new ApplicationUser
            {
                Id = userId,
                UserName = "sss",
                TableName = this.TableName,
                TableId = 1
            });
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return Task.Factory.StartNew(() =>  new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "sss",
                TableName = this.TableName,
                TableId = 1
            });
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

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            return Task.Factory.StartNew(() => "Something happens");
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            return Task.Factory.StartNew(() => "wow");
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return Task.Factory.StartNew(() => true);
        }

        public Task SetEmailAsync(ApplicationUser user, string email)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetEmailAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task AddClaimAsync(ApplicationUser user, Claim claim)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimAsync(ApplicationUser user, Claim claim)
        {
            throw new NotImplementedException();
        }

        public Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task AddToRoleAsync(ApplicationUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(ApplicationUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
