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
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace GTIWebAPI.Tests.TestControllers
{
    [TestClass]
    public class TestOrganizationsGTIController
    {
        [TestMethod]
        public void SearchOrganizationGTI_ShouldWork()
        {
            var organizationGTIsTestData = new List<OrganizationGTI>()
            {
                new OrganizationGTI { Id = 1 },
                new OrganizationGTI { Id = 2 },
                new OrganizationGTI { Id = 3 }
            };
            var organizationGTIs = MockHelper.MockDbSet(organizationGTIsTestData);

            var officesTestData = new List<Office>()
            {
                new Office { Id = 1 }
            };
            var offices = MockHelper.MockDbSet(officesTestData);

            var dbContext = new Mock<IAppDbContext>();

            dbContext.Setup(m => m.GTIOrganizations).Returns(organizationGTIs.Object);
            dbContext.Setup(d => d.Set<OrganizationGTI>()).Returns(organizationGTIs.Object);

            dbContext.Setup(m => m.Offices).Returns(offices.Object);
            dbContext.Setup(d => d.Set<Office>()).Returns(offices.Object);

            dbContext.Setup(d => d.ExecuteStoredProcedure<OrganizationGTI>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<OrganizationGTI> list = new List<OrganizationGTI>();
                   if (query.Contains("SearchOrganizationGTI"))
                   {
                       list.Add(new OrganizationGTI { Id = 4, Country = "AA", EnglishName = "fff", OfficeId = 1 });
                       list.Add(new OrganizationGTI { Id = 5, Country = "AA", EnglishName = "fff", OfficeId = 1 });
                       list.Add(new OrganizationGTI { Id = 6, Country = "AA", EnglishName = "fff", OfficeId = 1 });
                       list.Add(new OrganizationGTI { Id = 7, Country = "AA", EnglishName = "fff", OfficeId = 1 });
                   }
                   else
                   {
                       list.Add(new OrganizationGTI { Id = 4, Country = "AA", EnglishName = "fff" });
                   }
                   return list;
               });

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            var controller = new OrganizationsGTIController(factory.Object);
            var result = controller.SearchOrganizationsGTI("1,2", "test") as OkNegotiatedContentResult<IEnumerable<OrganizationGTIDTO>>;
            Assert.AreEqual(4, result.Content.Count());
        }
    }
}
