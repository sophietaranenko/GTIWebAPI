using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
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
    public class TestOrganizationContactPersonsController
    {
        [TestMethod]
        public void GetContactPersonsByOrganizationId_ShouldReturn()
        {
            var personsTestData = new List<OrganizationContactPersonView>()
            {
                new OrganizationContactPersonView { Id = 1, OrganizationId = 2 },
                new OrganizationContactPersonView { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationContactPersonView { Id = 3, OrganizationId = 3 }
            };
            var persons = MockHelper.MockDbSet(personsTestData);

            var contactsTestData = new List<OrganizationContactPersonContact>()
            {
                new OrganizationContactPersonContact { Id = 1}
            };
            var contacts = MockHelper.MockDbSet(contactsTestData);

            var dbContext = new Mock<IAppDbContext>();

            dbContext.Setup(m => m.OrganizationContactPersonContacts).Returns(contacts.Object);
            dbContext.Setup(d => d.Set<OrganizationContactPersonContact>()).Returns(contacts.Object);

            dbContext.Setup(m => m.OrganizationContactPersonViews).Returns(persons.Object);
            dbContext.Setup(d => d.Set<OrganizationContactPersonView>()).Returns(persons.Object);

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            var controller = new OrganizationContactPersonsController(factory.Object);
            var result = controller.GetOrganizationContactPersonByOrganizationId(2) as OkNegotiatedContentResult<IEnumerable<OrganizationContactPersonDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetContactPersonById_ShouldReturn()
        {
            var personsTestData = new List<OrganizationContactPersonView>()
            {
                new OrganizationContactPersonView { Id = 1, OrganizationId = 2 },
                new OrganizationContactPersonView { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationContactPersonView { Id = 3, OrganizationId = 3 }
            };
            var persons = MockHelper.MockDbSet(personsTestData);

            var contactsTestData = new List<OrganizationContactPersonContact>()
            {
                new OrganizationContactPersonContact { Id = 1}
            };
            var contacts = MockHelper.MockDbSet(contactsTestData);


            persons.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return persons.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationContactPersonViews).Returns(persons.Object);
            dbContext.Setup(d => d.Set<OrganizationContactPersonView>()).Returns(persons.Object);

            dbContext.Setup(m => m.OrganizationContactPersonContacts).Returns(contacts.Object);
            dbContext.Setup(d => d.Set<OrganizationContactPersonContact>()).Returns(contacts.Object);

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new OrganizationContactPersonsController(factory.Object);
            var result = controller.GetOrganizationContactPerson(1) as OkNegotiatedContentResult<OrganizationContactPersonDTO>;
            Assert.AreEqual(1, result.Content.Id);
            Assert.AreEqual(2, result.Content.OrganizationId);
        }

        [TestMethod]
        public void PutContactPerson_ShouldReturnOk()
        {
            var personsTestData = new List<OrganizationContactPersonView>()
            {
                new OrganizationContactPersonView { Id = 1, OrganizationId = 2 },
                new OrganizationContactPersonView { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationContactPersonView { Id = 3, OrganizationId = 3 }
            };
            var persons = MockHelper.MockDbSet(personsTestData);

            var personsvTestData = new List<OrganizationContactPerson>()
            {
                new OrganizationContactPerson { Id = 1, OrganizationId = 2 },
                new OrganizationContactPerson { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationContactPerson { Id = 3, OrganizationId = 3 }
            };
            var personsv = MockHelper.MockDbSet(personsvTestData);

            var contactsTestData = new List<OrganizationContactPersonContact>()
            {
                new OrganizationContactPersonContact { Id = 1}
            };
            var contacts = MockHelper.MockDbSet(contactsTestData);

            persons.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return persons.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationContactPersonViews).Returns(persons.Object);
            dbContext.Setup(d => d.Set<OrganizationContactPersonView>()).Returns(persons.Object);

            dbContext.Setup(m => m.OrganizationContactPersons).Returns(personsv.Object);
            dbContext.Setup(d => d.Set<OrganizationContactPerson>()).Returns(personsv.Object);

            dbContext.Setup(m => m.OrganizationContactPersonContacts).Returns(contacts.Object);
            dbContext.Setup(d => d.Set<OrganizationContactPersonContact>()).Returns(contacts.Object);


            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            OrganizationContactPerson passport = new OrganizationContactPerson { Id = 3, OrganizationId = 3 };
            var controller = new OrganizationContactPersonsController(factory.Object);
            var result = controller.PutOrganizationContactPerson(3, passport) as OkNegotiatedContentResult<OrganizationContactPersonDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
        }

        [TestMethod]
        public void PostContactPerson_ShoulAddContactPerson()
        {
            var personsvTestData = new List<OrganizationContactPerson>()
            {
                new OrganizationContactPerson { Id = 1, OrganizationId = 2 },
                new OrganizationContactPerson { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationContactPerson { Id = 3, OrganizationId = 3 }
            };
            var personsv = MockHelper.MockDbSet(personsvTestData);

            var personsTestData = new List<OrganizationContactPersonView>()
            {
                new OrganizationContactPersonView { Id = 1, OrganizationId = 2 },
                new OrganizationContactPersonView { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationContactPersonView { Id = 3, OrganizationId = 3 }
            };
            var persons = MockHelper.MockDbSet(personsTestData);

            var contactsTestData = new List<OrganizationContactPersonContact>()
            {
                new OrganizationContactPersonContact { Id = 1}
            };
            var contacts = MockHelper.MockDbSet(contactsTestData);


            persons.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return persons.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });
            persons.Setup(d => d.Add(It.IsAny<OrganizationContactPersonView>())).Returns<OrganizationContactPersonView>((contact) =>
            {
                personsTestData.Add(contact);
                personsvTestData.Add(new OrganizationContactPerson { Id = contact.Id });
                personsv = MockHelper.MockDbSet(personsvTestData);
                persons = MockHelper.MockDbSet(personsTestData);
                return contact;
            });

            personsv.Setup(d => d.Add(It.IsAny<OrganizationContactPerson>())).Returns<OrganizationContactPerson>((contact) =>
            {
                personsTestData.Add(new OrganizationContactPersonView { Id = contact.Id});
                personsvTestData.Add(contact);
                personsv = MockHelper.MockDbSet(personsvTestData);
                persons = MockHelper.MockDbSet(personsTestData);
                return contact;
            });

            var dbContext = new Mock<IAppDbContext>();

            dbContext.Setup(m => m.OrganizationContactPersonViews).Returns(persons.Object);
            dbContext.Setup(d => d.Set<OrganizationContactPersonView>()).Returns(persons.Object);

            dbContext.Setup(m => m.OrganizationContactPersonContacts).Returns(contacts.Object);
            dbContext.Setup(d => d.Set<OrganizationContactPersonContact>()).Returns(contacts.Object);

            dbContext.Setup(m => m.OrganizationContactPersons).Returns(personsv.Object);
            dbContext.Setup(d => d.Set<OrganizationContactPerson>()).Returns(personsv.Object);


            dbContext.Setup(d => d.ExecuteStoredProcedure<int>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<int> list = new List<int>();
                   if (query.Contains("NewTableId"))
                   {
                       int i = persons.Object.Max(d => d.Id) + 1;
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
            OrganizationContactPerson passport = new OrganizationContactPerson { Id = 0, OrganizationId = 3 };
            var controller = new OrganizationContactPersonsController(factory.Object);
            var result = controller.PostOrganizationContactPerson(passport) as CreatedAtRouteNegotiatedContentResult<OrganizationContactPersonDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Content.Id);
        }

        [TestMethod]
        public void DeleteContactPerson_ShouldDeleteAndReturnOk()
        {
            var personsTestData = new List<OrganizationContactPerson>()
            {
                new OrganizationContactPerson { Id = 1, OrganizationId = 2 },
                new OrganizationContactPerson { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationContactPerson { Id = 3, OrganizationId = 3 }
            };
            var persons = MockHelper.MockDbSet(personsTestData);

            var contactsTestData = new List<OrganizationContactPersonContact>()
            {
                new OrganizationContactPersonContact { Id = 1}
            };
            var contacts = MockHelper.MockDbSet(contactsTestData);

            persons.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return persons.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationContactPersons).Returns(persons.Object);
            dbContext.Setup(d => d.Set<OrganizationContactPerson>()).Returns(persons.Object);

            dbContext.Setup(m => m.OrganizationContactPersonContacts).Returns(contacts.Object);
            dbContext.Setup(d => d.Set<OrganizationContactPersonContact>()).Returns(contacts.Object);

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            OrganizationContactPerson passport = new OrganizationContactPerson { Id = 3, OrganizationId = 3 };
            var controller = new OrganizationContactPersonsController(factory.Object);
            var result = controller.DeleteOrganizationContactPerson(3) as OkNegotiatedContentResult<OrganizationContactPersonDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
            Assert.AreEqual(3, result.Content.OrganizationId);
        }
    }
}
