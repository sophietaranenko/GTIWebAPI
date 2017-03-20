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
    public class TestOrganizationsGTIController
    {
        private IDbContextFactory factory;
        private IOrganizationsGTIRepository repo;
        private TestDbContext db;

        public TestOrganizationsGTIController()
        {
            db = new TestDbContext();
            factory = new TestDbContextFactory(db);
            repo = new OrganizationsGTIRepository(factory);
            GetFewDemo();
        }

        [TestMethod]
        public void GetGTIByOrganizationId_ShouldReturnNotDeleted()
        {
            var controller = new OrganizationsGTIController(repo);
            var result = controller.SearchOrganizationsGTI("1", "111") as OkNegotiatedContentResult<List<OrganizationGTIDTO>>;
            Assert.AreEqual(1, result.Content.Count());
            Assert.AreEqual(1, result.Content.Select(d => d.RegistrationNumber).Distinct().Count());
            Assert.AreEqual("111", result.Content.Select(d => d.RegistrationNumber).Distinct().Take(1).FirstOrDefault());
        }

        public void GetFewDemo()
        {
            db.OrganizationsGTI.Add(new Models.Organizations.OrganizationGTI()
            {
                Id = 1,
                RegistrationNumber = "111",
                OfficeId = 1
            });
            db.OrganizationsGTI.Add(new Models.Organizations.OrganizationGTI()
            {
                Id = 2,
                RegistrationNumber = "111",
                OfficeId = 4
            });
            db.OrganizationsGTI.Add(new Models.Organizations.OrganizationGTI()
            {
                Id = 3,
                RegistrationNumber = "222",
                OfficeId = 1
            });
            db.OrganizationsGTI.Add(new Models.Organizations.OrganizationGTI()
            {
                Id = 4,
                RegistrationNumber = "222",
                OfficeId = 2
            });
        }
    }
}
