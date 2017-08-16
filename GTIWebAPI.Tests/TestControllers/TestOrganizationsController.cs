using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Tests.TestContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace GTIWebAPI.Tests.TestControllers
{
    [TestClass]
    public class TestOrganizationsController
    {
        [TestMethod]
        public void SearchOrganization_ShouldSearch()
        {
            var organizationsTestData = new List<Organization>()
            {
                new Organization { Id = 1 },
                new Organization { Id = 2, Deleted = true },
                new Organization { Id = 3 }
            };
            var organizations = MockHelper.MockDbSet(organizationsTestData);

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.Organizations).Returns(organizations.Object);
            dbContext.Setup(d => d.Set<Organization>()).Returns(organizations.Object);
            dbContext.Setup(d => d.ExecuteStoredProcedure<OrganizationSearchDTO>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<OrganizationSearchDTO> list = new List<OrganizationSearchDTO>();
                   if (query.Contains("SearchOrganization"))
                   {
                       list.Add(new OrganizationSearchDTO { Id = 1, NativeName = "ss" });
                       list.Add(new OrganizationSearchDTO { Id = 2, NativeName = "ss" });
                       list.Add(new OrganizationSearchDTO { Id = 3, NativeName = "ss" });
                       list.Add(new OrganizationSearchDTO { Id = 4, NativeName = "ss" });
                       list.Add(new OrganizationSearchDTO { Id = 5, NativeName = "ss" });
                       list.Add(new OrganizationSearchDTO { Id = 6, NativeName = "ss" });
                       list.Add(new OrganizationSearchDTO { Id = 7, NativeName = "ss" });
                   }
                   else
                   {
                       list.Add(new OrganizationSearchDTO { Id = 1, NativeName = "ss" });
                   }
                   return list;
               });

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new OrganizationsController(factory.Object);

            var result = controller.SearchOrganization(1, "test") as OkNegotiatedContentResult<IEnumerable<OrganizationSearchDTO>>;
            Assert.AreEqual(7, result.Content.Count());
        }

        //OrganizationByOfficeIds
        [TestMethod]
        public void GetOrganizatioByOfficeIds_ShoulReturn()
        {
            var organizationsTestData = new List<Organization>()
            {
                new Organization { Id = 1 },
                new Organization { Id = 2, Deleted = true },
                new Organization { Id = 3 }
            };
            var organizations = MockHelper.MockDbSet(organizationsTestData);

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.Organizations).Returns(organizations.Object);
            dbContext.Setup(d => d.Set<Organization>()).Returns(organizations.Object);
            dbContext.Setup(d => d.ExecuteStoredProcedure<OrganizationView>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<OrganizationView> list = new List<OrganizationView>();
                   if (query.Contains("OrganizationByOfficeIds"))
                   {
                       list.Add(new OrganizationView { Id = 1, NativeName = "ss" });
                       list.Add(new OrganizationView { Id = 2, NativeName = "ss" });
                       list.Add(new OrganizationView { Id = 3, NativeName = "ss" });
                       list.Add(new OrganizationView { Id = 4, NativeName = "ss" });
                       list.Add(new OrganizationView { Id = 5, NativeName = "ss" });
                       list.Add(new OrganizationView { Id = 6, NativeName = "ss" });
                   }
                   else
                   {
                       list.Add(new OrganizationView { Id = 1, NativeName = "ss" });
                   }
                   return list;
               });

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new OrganizationsController(factory.Object);

            var result = controller.GetOrganizationByOfficeIds("1,2") as OkNegotiatedContentResult<IEnumerable<OrganizationView>>;
            Assert.AreEqual(6, result.Content.Count());
        }

        [TestMethod]
        public void GetOrganizationView_ShouldReturnView()
        {
            //жопа
            var addrTestData = new List<OrganizationAddress>()
            {
                new OrganizationAddress { Id = 1}
            };
            var addresses = MockHelper.MockDbSet(addrTestData);

            var cpTestData = new List<OrganizationContactPerson>()
            {
                new OrganizationContactPerson { Id =1  }
            };
            var contactPersons = MockHelper.MockDbSet(cpTestData);


            var contactsTestData = new List<OrganizationContactPersonContact>()
            {
                new OrganizationContactPersonContact { Id = 1 }
            };
            var contacts = MockHelper.MockDbSet(contactsTestData);

            var linksTestData = new List<OrganizationGTILink>()
            {
                new OrganizationGTILink { Id = 1 }
            };
            var links = MockHelper.MockDbSet(linksTestData);

            var ogsgtiTestData = new List<OrganizationGTI>()
            {
                new OrganizationGTI { Id = 1 }
            };
            var orgs = MockHelper.MockDbSet(ogsgtiTestData);

            var officesTestData = new List<Office>()
            {
                new Office { Id = 1}
            };
            var offices = MockHelper.MockDbSet(officesTestData);

            var propsTestData = new List<OrganizationProperty>()
            {
                new OrganizationProperty { Id = 1 }
            };
            var properties = MockHelper.MockDbSet(propsTestData);


            var taxAddressesTestData = new List<OrganizationTaxAddress>()
            {
                new OrganizationTaxAddress { Id = 1}
            };
            var taxAddresses = MockHelper.MockDbSet(taxAddressesTestData);

            var namesTestData = new List<OrganizationLanguageName>()
            {
                new OrganizationLanguageName { Id = 1}
            };
            var names = MockHelper.MockDbSet(namesTestData);


            var cpViewsTestData = new List<OrganizationContactPersonView>()
            {
                new OrganizationContactPersonView { Id = 1}
            };
            var cpViews = MockHelper.MockDbSet(cpViewsTestData);


            var organizationsTestData = new List<Organization>()
            {
                new Organization { Id = 1 },
                new Organization { Id = 2, Deleted = true },
                new Organization { Id = 3 }
            };
            var organizations = MockHelper.MockDbSet(organizationsTestData);

            var dbContext = new Mock<IAppDbContext>();

            dbContext.Setup(m => m.Organizations).Returns(organizations.Object);
            dbContext.Setup(d => d.Set<Organization>()).Returns(organizations.Object);

            dbContext.Setup(m => m.OrganizationAddresses).Returns(addresses.Object);
            dbContext.Setup(d => d.Set<OrganizationAddress>()).Returns(addresses.Object);

            dbContext.Setup(m => m.OrganizationContactPersons).Returns(contactPersons.Object);
            dbContext.Setup(d => d.Set<OrganizationContactPerson>()).Returns(contactPersons.Object);

            dbContext.Setup(m => m.OrganizationContactPersonContacts).Returns(contacts.Object);
            dbContext.Setup(d => d.Set<OrganizationContactPersonContact>()).Returns(contacts.Object);

            dbContext.Setup(m => m.OrganizationGTILinks).Returns(links.Object);
            dbContext.Setup(d => d.Set<OrganizationGTILink>()).Returns(links.Object);

            dbContext.Setup(m => m.GTIOrganizations).Returns(orgs.Object);
            dbContext.Setup(d => d.Set<OrganizationGTI>()).Returns(orgs.Object);

            dbContext.Setup(m => m.Offices).Returns(offices.Object);
            dbContext.Setup(d => d.Set<Office>()).Returns(offices.Object);

            dbContext.Setup(m => m.OrganizationProperties).Returns(properties.Object);
            dbContext.Setup(d => d.Set<OrganizationProperty>()).Returns(properties.Object);

            dbContext.Setup(m => m.OrganizationTaxAddresses).Returns(taxAddresses.Object);
            dbContext.Setup(d => d.Set<OrganizationTaxAddress>()).Returns(taxAddresses.Object);

            dbContext.Setup(m => m.OrganizationLanguageNames).Returns(names.Object);
            dbContext.Setup(d => d.Set<OrganizationLanguageName>()).Returns(names.Object);

            dbContext.Setup(m => m.OrganizationContactPersonViews).Returns(cpViews.Object);
            dbContext.Setup(d => d.Set<OrganizationContactPersonView>()).Returns(cpViews.Object);

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new OrganizationsController(factory.Object);
            var result = controller.GetOrganizationView(2) as OkNegotiatedContentResult<OrganizationDTO>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetOrganizationEdit_ShouldReturnEdit()
        {
            var organizationsTestData = new List<Organization>()
            {
                new Organization { Id = 1 },
                new Organization { Id = 2, Deleted = true },
                new Organization { Id = 3 }
            };
            var organizations = MockHelper.MockDbSet(organizationsTestData);
            organizations.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return organizations.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.Organizations).Returns(organizations.Object);
            dbContext.Setup(d => d.Set<Organization>()).Returns(organizations.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new OrganizationsController(factory.Object);
            var result = controller.GetOrganizationEdit(1) as OkNegotiatedContentResult<OrganizationEditDTO>;
            Assert.AreEqual(1, result.Content.Id);
        }

        [TestMethod]
        public void PutOrganization_ShouldReturnOk()
        {
            var organizationsTestData = new List<Organization>()
            {
                new Organization { Id = 1},
                new Organization { Id = 2, Deleted = true },
                new Organization { Id = 3 }
            };
            var organizations = MockHelper.MockDbSet(organizationsTestData);
            organizations.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return organizations.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });
            organizations.Setup(d => d.Add(It.IsAny<Organization>())).Returns<Organization>((contact) =>
            {
                organizationsTestData.Add(contact);
                organizations = MockHelper.MockDbSet(organizationsTestData);
                return contact;
            });



            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.Organizations).Returns(organizations.Object);
            dbContext.Setup(d => d.Set<Organization>()).Returns(organizations.Object);


            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            Organization passport = new Organization { Id = 3 };
            var controller = new OrganizationsController(factory.Object);
            var result = controller.PutOrganization(3, passport.MapToEdit()) as OkNegotiatedContentResult<OrganizationEditDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
        }

        [TestMethod]
        public void PostProperty_ShoulAddProperty()
        {
            var organizationsTestData = new List<Organization>()
            {
                new Organization { Id = 1 },
                new Organization { Id = 2, Deleted = true },
                new Organization { Id = 3 }
            };
            var organizations = MockHelper.MockDbSet(organizationsTestData);
            organizations.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return organizations.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });
            organizations.Setup(d => d.Add(It.IsAny<Organization>())).Returns<Organization>((contact) =>
            {
                organizationsTestData.Add(contact);
                organizations = MockHelper.MockDbSet(organizationsTestData);
                return contact;
            });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.Organizations).Returns(organizations.Object);
            dbContext.Setup(d => d.Set<Organization>()).Returns(organizations.Object);

         


            dbContext.Setup(d => d.ExecuteStoredProcedure<int>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<int> list = new List<int>();
                   if (query.Contains("NewTableId"))
                   {
                       int i = organizations.Object.Max(d => d.Id) + 1;
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
            Organization passport = new Organization { Id = 0 };

            var identityHelper = new Mock<IIdentityHelper>();
            identityHelper.Setup(d => d.GetUserTableId(It.IsAny<IPrincipal>())).Returns(11);
            identityHelper.Setup(d => d.GetUserTableName(It.IsAny<IPrincipal>())).Returns("Employee");


            var controller = new OrganizationsController(factory.Object, identityHelper.Object);
            var result = controller.PostOrganization(passport.MapToEdit()) as CreatedAtRouteNegotiatedContentResult<OrganizationEditDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Content.Id);
        }

        [TestMethod]
        public void DeleteOrganization_ShouldDeleteAndReturnOk()
        {
            var organizationsTestData = new List<Organization>()
            {
                new Organization { Id = 1 },
                new Organization { Id = 2, Deleted = true },
                new Organization { Id = 3 }
            };

            var organizations = MockHelper.MockDbSet(organizationsTestData);
            organizations.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return organizations.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.Organizations).Returns(organizations.Object);
            dbContext.Setup(d => d.Set<Organization>()).Returns(organizations.Object);

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            Organization passport = new Organization { Id = 3 };
            var controller = new OrganizationsController(factory.Object);
            var result = controller.DeleteOrganization(3) as OkNegotiatedContentResult<OrganizationEditDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
        }
    }
}
