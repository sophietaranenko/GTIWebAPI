using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Repository.Organization;
using GTIWebAPI.Tests.TestContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace GTIWebAPI.Tests.TestControllers
{
    [TestClass]
    public class TestOrganizationContactPersonContactsController
    {
        private IDbContextFactory factory;
        private IOrganizationContactPersonContactsRepository repo;

        public TestOrganizationContactPersonContactsController()
        {
            factory = new TestDbContextFactory();
            repo = new OrganizationContactPersonContactsRepository(factory);
            GetFewDemo();
        }

        [TestMethod]
        public void GetByOrganizationContactId_ShouldReturnNotDeletedContactPersonContacts()
        {
            var controller = new OrganizationContactPersonContactsController(repo);
            var result = controller.GetOrganizationContactPersonContactByOrganizationContactPersonId(1) as OkNegotiatedContentResult<List<OrganizationContactPersonContactDTO>>;
            Assert.AreEqual(1, result.Content.Count());
            Assert.AreEqual(1, result.Content.Select(d => d.OrganizationContactPersonId).Distinct().Count());
            Assert.AreEqual(1, result.Content.Select(d => d.OrganizationContactPersonId).Distinct().Take(1).FirstOrDefault());
        }

        [TestMethod]
        public void GetOrganizationContactsById_ShouldReturnObjectWithSameId()
        {
            var controller = new OrganizationContactPersonContactsController(repo);
            var result = controller.GetOrganizationContactPersonContact(1) as OkNegotiatedContentResult<OrganizationContactPersonContactDTO>;
            Assert.AreEqual(result.Content.Id, 1);
        }

        [TestMethod]
        public void PutOrganizationContact_ShouldReturnOk()
        {
            var controller = new OrganizationContactPersonContactsController(repo);
            OrganizationContactPersonContact contact = repo.Add(GetDemo());
            var result = controller.PutOrganizationContactPersonContact(contact.Id, contact) as OkNegotiatedContentResult<OrganizationContactPersonContactDTO>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PutOrganizationContact_ShouldFail_WhenDifferentID()
        {
            var controller = new OrganizationContactPersonContactsController(repo);
            OrganizationContactPersonContact contact = GetDemo();
            var badresult = controller.PutOrganizationContactPersonContact(999, contact);
            Assert.IsInstanceOfType(badresult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PostOrganizationContact_ShouldReturnSame()
        {
            var controller = new OrganizationContactPersonContactsController(repo);
            var item = GetDemo();
            var result = controller.PostOrganizationContactPersonContact(item) as CreatedAtRouteNegotiatedContentResult<OrganizationContactPersonContactDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "GetOrganizationContactPersonContact");
            Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
        }

        [TestMethod]
        public void DeleteOrganizationContact_ShouldReturnOK()
        {
            OrganizationContactPersonContact contact = repo.Add(GetDemo());

            var controller = new OrganizationContactPersonContactsController(repo);
            var result = controller.DeleteOrganizationContactPersonContact(contact.Id) as OkNegotiatedContentResult<OrganizationContactPersonContactDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(contact.Id, result.Content.Id);
        }

        private OrganizationContactPersonContact GetDemo()
        {
            OrganizationContactPersonContact contact = new OrganizationContactPersonContact
            {
                Id = 0,
                ContactTypeId = 1,
                OrganizationContactPersonId = 1
            };
            return contact;
        }

        private void GetFewDemo()
        {
            repo.Add(new OrganizationContactPersonContact { Id = 1, Deleted = true, OrganizationContactPersonId = 1 });
            repo.Add(new OrganizationContactPersonContact { Id = 2, Deleted = false, OrganizationContactPersonId = 1 });
            repo.Add(new OrganizationContactPersonContact { Id = 3, Deleted = false, OrganizationContactPersonId = 2 });
            repo.Add(new OrganizationContactPersonContact { Id = 4, Deleted = false, OrganizationContactPersonId = 2 });
        }
    }
}
