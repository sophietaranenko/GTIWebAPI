using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Personnel;
using GTIWebAPI.Models.Repository;
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
    public class TestEmployeeContactsController
    {
        private IDbContextFactory factory;
        private IRepository<EmployeeContact> repo;

        public TestEmployeeContactsController()
        {
            factory = new TestDbContextFactory();
            repo = new EmployeeContactsRepository(factory);
            GetFewDemo();
        }

        [TestMethod]
        public void GetAllContacts_ShouldReturnNotDeleted()
        {
            var controller = new GTIWebAPI.Controllers.EmployeeContactsController(repo);
            var result = controller.GetEmployeeContactAll() as OkNegotiatedContentResult<IEnumerable<EmployeeContactDTO>>;
            Assert.AreEqual(3, result.Content.Count());
        }

        [TestMethod]
        public void GetContactsByEmployeeId_ShouldReturnNotDeletedEmployeesContact()
        {
            var controller = new GTIWebAPI.Controllers.EmployeeContactsController(repo);
            var result = controller.GetEmployeeContactByEmployee(1) as OkNegotiatedContentResult<IEnumerable<EmployeeContactDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }


        [TestMethod]
        public void GetContactsByEmployeeId_ShouldReturnZeroCount()
        {
            var controller = new GTIWebAPI.Controllers.EmployeeContactsController(repo);
            var badResult = controller.GetEmployeeContactByEmployee(999) as OkNegotiatedContentResult<IEnumerable<EmployeeContactDTO>>;
            Assert.AreEqual(0, badResult.Content.Count());
        }

        [TestMethod]
        public void GetContactById_ShouldReturnObjectWithSameId()
        {
            var controller = new GTIWebAPI.Controllers.EmployeeContactsController(repo);
            var result = controller.GetEmployeeContact(1) as OkNegotiatedContentResult<EmployeeContactDTO>;
            Assert.AreEqual(result.Content.Id, 1);
        }

        [TestMethod]
        public void GetContactById_ShouldReturnBadRequestWhenIdIsNotFound()
        {
            var controller = new GTIWebAPI.Controllers.EmployeeContactsController(repo);
            var badResult = controller.GetEmployeeContact(999);
            Assert.IsInstanceOfType(badResult, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void Put_ShouldReturnOk()
        {
            var controller = new GTIWebAPI.Controllers.EmployeeContactsController(repo);
            EmployeeContact contact = GetDemo();

            var addResult = controller.PostEmployeeContact(contact) as CreatedAtRouteNegotiatedContentResult<EmployeeContactDTO>;
            EmployeeContactDTO dto = addResult.Content;

            var result = controller.PutEmployeeContact(dto.Id, contact) as OkNegotiatedContentResult<EmployeeContactDTO>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Put_ShouldFail_WhenDifferentID()
        {
            var controller = new EmployeeContactsController(repo);
            EmployeeContact contact = GetDemo();
            var badresult = controller.PutEmployeeContact(999, contact);
            Assert.IsInstanceOfType(badresult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PostContact_ShouldReturnSameContact()
        {
            var controller = new EmployeeContactsController(repo);
            var item = GetDemo();
            var result = controller.PostEmployeeContact(item) as CreatedAtRouteNegotiatedContentResult<EmployeeContactDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "GetEmployeeContact");
            Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
            Assert.AreEqual(result.Content.ContactTypeId, item.ContactTypeId);
        }

        [TestMethod]
        public void DeleteContact_ShouldReturnOK()
        {
            EmployeeContact contact = GetDemo();
            contact = repo.Add(contact);

            var controller = new EmployeeContactsController(repo);
            var result = controller.DeleteEmployeeContact(contact.Id) as OkNegotiatedContentResult<EmployeeContactDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(contact.Id, result.Content.Id);
        }

        private EmployeeContact GetDemo()
        {
            EmployeeContact contact = new EmployeeContact
            {
                Id = 0,
                Value = "ddd",
                EmployeeId = 1
            };
            return contact;
        }

        private void GetFewDemo()
        {
            repo.Add(new EmployeeContact
            {
                Id = 1,
                Deleted = true,
                EmployeeId = 1,
                ContactTypeId = 1,
                ContactType = new ContactType
                {
                    Id = 1,
                    Name = "smth"
                }
            });
            repo.Add(new EmployeeContact
            {
                Id = 2,
                Deleted = false,
                EmployeeId = 1,
                ContactTypeId = 1,
                ContactType = new ContactType
                {
                    Id = 1,
                    Name = "smth"
                }
            });
            repo.Add(new EmployeeContact
            {
                Id = 3,
                Deleted = false,
                EmployeeId = 2,
                ContactTypeId = 1,
                ContactType = new ContactType
                {
                    Id = 1,
                    Name = "smth"
                }
            });
            repo.Add(new EmployeeContact
            {
                Id = 4,
                Deleted = false,
                EmployeeId = 2,
                ContactTypeId = 2,
                ContactType = new ContactType
                {
                    Id = 2,
                    Name = "smth_two"
                }
            });
        }
    }
}
