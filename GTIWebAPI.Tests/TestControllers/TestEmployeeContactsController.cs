using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Personnel;
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Tests.TestContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
        [TestMethod]
        public void GetAllContacts_ShouldReturnNotDeleted()
        {
            var contactsTestData = new List<EmployeeContact>()
            {
                new EmployeeContact { Id = 1, EmployeeId = 2 },
                new EmployeeContact { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeContact { Id = 3, EmployeeId = 3 }
            };
            var contacts = MockHelper.MockDbSet(contactsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeContacts).Returns(contacts.Object);
            dbContext.Setup(d => d.Set<EmployeeContact>()).Returns(contacts.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            IDbContextFactory fac = factory.Object;
            var controller = new GTIWebAPI.Controllers.EmployeeContactsController(fac);
            var result = controller.GetEmployeeContactAll() as OkNegotiatedContentResult<IEnumerable<EmployeeContactDTO>>;
            Assert.AreEqual(2, result.Content.Count());
        }

        [TestMethod]
        public void GetContactsByEmployeeId_ShouldReturnNotDeletedEmployeesContact()
        {
            var contactsTestData = new List<EmployeeContact>()
            {
                new EmployeeContact { Id = 1, EmployeeId = 2 },
                new EmployeeContact { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeContact { Id = 3, EmployeeId = 3 }
            };
            var contacts = MockHelper.MockDbSet(contactsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeContacts).Returns(contacts.Object);
            dbContext.Setup(d => d.Set<EmployeeContact>()).Returns(contacts.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            IDbContextFactory fac = factory.Object;
            var controller = new GTIWebAPI.Controllers.EmployeeContactsController(fac);
            var result = controller.GetEmployeeContactByEmployee(2) as OkNegotiatedContentResult<IEnumerable<EmployeeContactDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }


        [TestMethod]
        public void GetContactsByEmployeeId_ShouldReturnZeroCount()
        {
            var contactsTestData = new List<EmployeeContact>()
            {
                new EmployeeContact { Id = 1, EmployeeId = 2 },
                new EmployeeContact { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeContact { Id = 3, EmployeeId = 3 }
            };
            var contacts = MockHelper.MockDbSet(contactsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeContacts).Returns(contacts.Object);
            dbContext.Setup(d => d.Set<EmployeeContact>()).Returns(contacts.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            IDbContextFactory fac = factory.Object;
            var controller = new GTIWebAPI.Controllers.EmployeeContactsController(fac);
            var badResult = controller.GetEmployeeContactByEmployee(4) as OkNegotiatedContentResult<IEnumerable<EmployeeContactDTO>>;
            Assert.AreEqual(0, badResult.Content.Count());
        }

        [TestMethod]
        public void GetContactById_ShouldReturnObjectWithSameId()
        {
            var contactsTestData = new List<EmployeeContact>()
            {
                new EmployeeContact { Id = 1, EmployeeId = 2 },
                new EmployeeContact { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeContact { Id = 3, EmployeeId = 3 }
            };
            var contacts = MockHelper.MockDbSet(contactsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeContacts).Returns(contacts.Object);
            dbContext.Setup(d => d.Set<EmployeeContact>()).Returns(contacts.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            IDbContextFactory fac = factory.Object;
            var controller = new GTIWebAPI.Controllers.EmployeeContactsController(fac);
            var result = controller.GetEmployeeContact(1) as OkNegotiatedContentResult<EmployeeContactDTO>;
            Assert.AreEqual(result.Content.Id, 1);
        }

        [TestMethod]
        public void GetContactById_ShouldReturnNotFoundWhenIdIsNotFound()
        {
            var contactsTestData = new List<EmployeeContact>()
            {
                new EmployeeContact { Id = 1, EmployeeId = 2 },
                new EmployeeContact { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeContact { Id = 3, EmployeeId = 3 }
            };
            var contacts = MockHelper.MockDbSet(contactsTestData);
            contacts.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return contacts.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeContacts).Returns(contacts.Object);
            dbContext.Setup(d => d.Set<EmployeeContact>()).Returns(contacts.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            IDbContextFactory fac = factory.Object;
            var controller = new GTIWebAPI.Controllers.EmployeeContactsController(fac);
            var badResult = controller.GetEmployeeContact(999);
            Assert.IsInstanceOfType(badResult, typeof(NotFoundResult));
        }

        //http://stackoverflow.com/questions/34611842/unit-test-for-update-the-entity-in-entity-framework
        //What you're doing here is testing your code, 
        //and not allowing your test to fail because of anything to do with Entity Framework, or the database. 
        //That is what a Unit Test is for.
        [TestMethod]
        public void PutContact_ShouldReturnOk()
        {
            var contactsTestData = new List<EmployeeContact>()
            {
                new EmployeeContact { Id = 1, EmployeeId = 2 },
                new EmployeeContact { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeContact { Id = 3, EmployeeId = 3 }
            };
            var contacts = MockHelper.MockDbSet(contactsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeContacts).Returns(contacts.Object);
            dbContext.Setup(d => d.Set<EmployeeContact>()).Returns(contacts.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            IDbContextFactory fac = factory.Object;
            var controller = new GTIWebAPI.Controllers.EmployeeContactsController(fac);
            EmployeeContact contact = new EmployeeContact { Id = 3, EmployeeId = 25 };
            var result = controller.PutEmployeeContact(3, contact) as OkNegotiatedContentResult<EmployeeContactDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
        }

        [TestMethod]
        public void PutContact_ShouldFail_WhenDifferentID()
        {
            var contactsTestData = new List<EmployeeContact>()
            {
                new EmployeeContact { Id = 1, EmployeeId = 2 },
                new EmployeeContact { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeContact { Id = 3, EmployeeId = 3 }
            };
            var contacts = MockHelper.MockDbSet(contactsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeContacts).Returns(contacts.Object);
            dbContext.Setup(d => d.Set<EmployeeContact>()).Returns(contacts.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            IDbContextFactory fac = factory.Object;
            var controller = new EmployeeContactsController(fac);
            EmployeeContact contact = new EmployeeContact { Id = 4, EmployeeId = 25 };
            var badresult = controller.PutEmployeeContact(999, contact);
            Assert.IsInstanceOfType(badresult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PostContact_ShouldReturnSameContact()
        {
            var contactsTestData = new List<EmployeeContact>()
            {
                new EmployeeContact { Id = 1, EmployeeId = 2 },
                new EmployeeContact { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeContact { Id = 3, EmployeeId = 3 }
            };
            var contacts = MockHelper.MockDbSet(contactsTestData);
            contacts.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return contacts.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });
            contacts.Setup(d => d.Add(It.IsAny<EmployeeContact>())).Returns<EmployeeContact>((contact) => 
            {
                contactsTestData.Add(contact);
                contacts = MockHelper.MockDbSet(contactsTestData);
                return contact;
            });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeContacts).Returns(contacts.Object);
            dbContext.Setup(d => d.Set<EmployeeContact>()).Returns(contacts.Object);
            dbContext.Setup(d => d.SaveChanges()).Returns(0);
            dbContext.Setup(d => d.ExecuteStoredProcedure<int>(It.IsAny<string>(), It.IsAny<object[]>()))
                .Returns<string, object[]>((query, parameters) =>
                {
                    List<int> list = new List<int>();
                    if (query.Contains("NewTableId"))
                    {
                        int i = contacts.Object.Max(d => d.Id) + 1;
                        list.Add(i);
                    }
                    else
                    {
                        list.Add(0);
                    }
                    return list;
                });

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            IDbContextFactory fac = factory.Object;

            var controller = new EmployeeContactsController(fac);
            var item = new EmployeeContact { Id = 4, EmployeeId = 25, ContactTypeId = 3 };
            var result = controller.PostEmployeeContact(item) as CreatedAtRouteNegotiatedContentResult<EmployeeContactDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "GetEmployeeContact");
            Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
            Assert.AreEqual(result.Content.ContactTypeId, item.ContactTypeId);
        }

        [TestMethod]
        public void DeleteContact_ShouldReturnOK()
        {
            var contactsTestData = new List<EmployeeContact>()
            {
                new EmployeeContact { Id = 1, EmployeeId = 2 },
                new EmployeeContact { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeContact { Id = 3, EmployeeId = 3 }
            };
            var contacts = MockHelper.MockDbSet(contactsTestData);
            contacts.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return contacts.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeContacts).Returns(contacts.Object);
            dbContext.Setup(d => d.Set<EmployeeContact>()).Returns(contacts.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            IDbContextFactory fac = factory.Object;
            var controller = new EmployeeContactsController(fac);
            var result = controller.DeleteEmployeeContact(3) as OkNegotiatedContentResult<EmployeeContactDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
        }
    }
}
