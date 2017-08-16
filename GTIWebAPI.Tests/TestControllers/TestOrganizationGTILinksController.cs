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
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Results;

namespace GTIWebAPI.Tests.TestControllers
{
    [TestClass]
    public class TestOrganizationGTILinksController
    {
        [TestMethod]
        public void GetLinksByOrganizationId_ShouldReturn()
        {
            var linksTestData = new List<OrganizationGTILink>()
            {
                new OrganizationGTILink { Id = 1, OrganizationId = 2 },
                new OrganizationGTILink { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationGTILink { Id = 3, OrganizationId = 3 }
            };
            var links = MockHelper.MockDbSet(linksTestData);
            var orggtiTestData = new List<OrganizationGTI>()
            {
                new OrganizationGTI { Id = 1 }
            };
            var orgs = MockHelper.MockDbSet(orggtiTestData);
            var offTestData = new List<Office>()
            {
                new Office { Id = 1 }
            };
            var offices = MockHelper.MockDbSet(offTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationGTILinks).Returns(links.Object);
            dbContext.Setup(d => d.Set<OrganizationGTILink>()).Returns(links.Object);

            dbContext.Setup(m => m.GTIOrganizations).Returns(orgs.Object);
            dbContext.Setup(d => d.Set<OrganizationGTI>()).Returns(orgs.Object);

            dbContext.Setup(m => m.Offices).Returns(offices.Object);
            dbContext.Setup(d => d.Set<Office>()).Returns(offices.Object);


            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new OrganizationGTILinksController(factory.Object);
            var result = controller.GetOrganizationGTILinkByOrganizationId(2) as OkNegotiatedContentResult<IEnumerable<OrganizationGTILinkDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetLinkById_ShouldReturn()
        {
            var linksTestData = new List<OrganizationGTILink>()
            {
                new OrganizationGTILink { Id = 1, OrganizationId = 2 },
                new OrganizationGTILink { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationGTILink { Id = 3, OrganizationId = 3 }
            };
            var links = MockHelper.MockDbSet(linksTestData);
            var orggtiTestData = new List<OrganizationGTI>()
            {
                new OrganizationGTI { Id = 1 }
            };
            var orgs = MockHelper.MockDbSet(orggtiTestData);
            var offTestData = new List<Office>()
            {
                new Office { Id = 1 }
            };
            var offices = MockHelper.MockDbSet(offTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationGTILinks).Returns(links.Object);
            dbContext.Setup(d => d.Set<OrganizationGTILink>()).Returns(links.Object);

            dbContext.Setup(m => m.GTIOrganizations).Returns(orgs.Object);
            dbContext.Setup(d => d.Set<OrganizationGTI>()).Returns(orgs.Object);

            dbContext.Setup(m => m.Offices).Returns(offices.Object);
            dbContext.Setup(d => d.Set<Office>()).Returns(offices.Object);

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new OrganizationGTILinksController(factory.Object);
            var result = controller.GetOrganizationGTILink(1) as OkNegotiatedContentResult<OrganizationGTILinkDTO>;
            Assert.AreEqual(1, result.Content.Id);
            Assert.AreEqual(2, result.Content.OrganizationId);
        }


        [TestMethod]
        public void PostLink_ShoulAddLink()
        {
            var linksTestData = new List<OrganizationGTILink>()
            {
                new OrganizationGTILink { Id = 1, OrganizationId = 2 },
                new OrganizationGTILink { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationGTILink { Id = 3, OrganizationId = 3 }
            };
            var links = MockHelper.MockDbSet(linksTestData);
            var orggtiTestData = new List<OrganizationGTI>()
            {
                new OrganizationGTI { Id = 77, OfficeId = 1, NativeName = "Naive name of organization" }
            };
            var orgs = MockHelper.MockDbSet(orggtiTestData);
            var offTestData = new List<Office>()
            {
                new Office { Id = 1, ShortName = "Some short office name" }
            };
            var offices = MockHelper.MockDbSet(offTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationGTILinks).Returns(links.Object);
            dbContext.Setup(d => d.Set<OrganizationGTILink>()).Returns(links.Object);

            dbContext.Setup(m => m.GTIOrganizations).Returns(orgs.Object);
            dbContext.Setup(d => d.Set<OrganizationGTI>()).Returns(orgs.Object);

            dbContext.Setup(m => m.Offices).Returns(offices.Object);
            dbContext.Setup(d => d.Set<Office>()).Returns(offices.Object);

            links.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return links.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });
            links.Setup(d => d.Add(It.IsAny<OrganizationGTILink>())).Returns<OrganizationGTILink>((contact) =>
            {
                linksTestData.Add(contact);
                links = MockHelper.MockDbSet(linksTestData);
                return contact;
            });


            dbContext.Setup(d => d.ExecuteStoredProcedure<int>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<int> list = new List<int>();
                   if (query.Contains("NewTableId"))
                   {
                       int i = links.Object.Max(d => d.Id) + 1;
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



            OrganizationGTILink link = new OrganizationGTILink { Id = 33, OrganizationId = 55, GTIId = 77 } ;


            var identityHelper = new Mock<IIdentityHelper>();
           // identityHelper.Setup(d => d.GetUserTableName(It.IsAny<IPrincipal>())).Returns("Employee");
           // identityHelper.Setup(d => d.GetUserTableId(It.IsAny<IPrincipal>())).Returns(11);



            var controller = new OrganizationGTILinksController(factory.Object, identityHelper.Object);

            var result = controller.PostOrganizationGTILink(link.ToDTO()) as CreatedAtRouteNegotiatedContentResult<OrganizationGTILinkDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Content.Id);
            Assert.AreEqual(55, result.Content.OrganizationId);
            Assert.AreEqual(77, result.Content.GTIId);
        }

        [TestMethod]
        public void DeleteLink_ShouldDeleteAndReturnOk()
        {
            var linksTestData = new List<OrganizationGTILink>()
            {
                new OrganizationGTILink { Id = 1, OrganizationId = 2 },
                new OrganizationGTILink { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationGTILink { Id = 3, OrganizationId = 3 }
            };
            var links = MockHelper.MockDbSet(linksTestData);
            var orggtiTestData = new List<OrganizationGTI>()
            {
                new OrganizationGTI { Id = 1 }
            };
            var orgs = MockHelper.MockDbSet(orggtiTestData);
            var offTestData = new List<Office>()
            {
                new Office { Id = 1 }
            };
            var offices = MockHelper.MockDbSet(offTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationGTILinks).Returns(links.Object);
            dbContext.Setup(d => d.Set<OrganizationGTILink>()).Returns(links.Object);

            dbContext.Setup(m => m.GTIOrganizations).Returns(orgs.Object);
            dbContext.Setup(d => d.Set<OrganizationGTI>()).Returns(orgs.Object);

            dbContext.Setup(m => m.Offices).Returns(offices.Object);
            dbContext.Setup(d => d.Set<Office>()).Returns(offices.Object);

            links.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return links.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            OrganizationGTILink link = new OrganizationGTILink { Id = 3, OrganizationId = 3 };
            var controller = new OrganizationGTILinksController(factory.Object);
            var result = controller.DeleteOrganizationGTILink(3) as OkNegotiatedContentResult<OrganizationGTILinkDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
            Assert.AreEqual(3, result.Content.OrganizationId);
        }
    }
}
