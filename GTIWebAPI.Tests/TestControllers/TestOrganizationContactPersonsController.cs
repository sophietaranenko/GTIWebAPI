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
    public class TestOrganizationContactPersonsController
    {
        private IDbContextFactory factory;
        private IOrganizationContactPersonsRepository repo;
        private TestDbContext db;

        public TestOrganizationContactPersonsController()
        {
            db = new TestDbContext();
            factory = new TestDbContextFactory(db);
            repo = new OrganizationContactPersonsRepository(factory);
            GetFewDemo();
        }

        [TestMethod]
        public void GetPersonsByOrganizationId_ShouldReturnNotDeletedOrganizationIdsContactPersones()
        {
            var controller = new OrganizationContactPersonsController(repo);
            var result = controller.GetOrganizationContactPersonByOrganizationId(1) as OkNegotiatedContentResult<List<OrganizationContactPersonDTO>>;
            Assert.AreEqual(1, result.Content.Count());
            Assert.AreEqual(1, result.Content.Select(d => d.OrganizationId).Distinct().Count());
            Assert.AreEqual(1, result.Content.Select(d => d.OrganizationId).Distinct().Take(1).FirstOrDefault());
        }

        [TestMethod]
        public void GetPersonById_ShouldReturnObjectWithSameId()
        {
            var controller = new OrganizationContactPersonsController(repo);
            var result = controller.GetOrganizationContactPerson(1) as OkNegotiatedContentResult<OrganizationContactPersonDTO>;
            Assert.AreEqual(result.Content.Id, 1);
        }

        [TestMethod]
        public void PutPerson_ShouldReturnOk()
        {
            var controller = new OrganizationContactPersonsController(repo);
            OrganizationContactPerson person = GetDemo();
            OrganizationContactPersonView personDTO = repo.Add(GetDemo());
            person.Id = personDTO.Id;
            var result = controller.PutOrganizationContactPerson(personDTO.Id, person) as OkNegotiatedContentResult<OrganizationContactPersonDTO>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PutPerson_ShouldFail_WhenDifferentID()
        {
            var controller = new OrganizationContactPersonsController(repo);
            OrganizationContactPerson person = GetDemo();
            var badresult = controller.PutOrganizationContactPerson(999, person);
            Assert.IsInstanceOfType(badresult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Post_ShouldReturnSame()
        {
            var controller = new OrganizationContactPersonsController(repo);
            var item = GetDemo();
            var result = controller.PostOrganizationContactPerson(item) as CreatedAtRouteNegotiatedContentResult<OrganizationContactPersonDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "GetOrganizationContactPerson");
            Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
        }

        [TestMethod]
        public void DeletePerson_ShouldReturnOK()
        {
            OrganizationContactPersonView person = repo.Add(GetDemo());

            var controller = new OrganizationContactPersonsController(repo);
            var result = controller.DeleteOrganizationContactPerson(person.Id) as OkNegotiatedContentResult<OrganizationContactPersonDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(person.Id, result.Content.Id);
        }

        private OrganizationContactPerson GetDemo()
        {
            OrganizationContactPerson gun = new OrganizationContactPerson
            {
                Id = 0,
                OrganizationId = 1
            };
            return gun;
        }

        private void GetFewDemo()
        {
            db.OrganizationContactPersonViews.Add(new OrganizationContactPersonView { Id = 1, Deleted = true, OrganizationId = 1,  FirstName = "Req", LastName = "Req" });
            db.OrganizationContactPersonViews.Add(new OrganizationContactPersonView { Id = 2, Deleted = false, OrganizationId = 1,  FirstName = "Req", LastName = "Req" });
            db.OrganizationContactPersonViews.Add(new OrganizationContactPersonView { Id = 3, Deleted = false, OrganizationId = 2,  FirstName = "Req", LastName = "Req" });
            db.OrganizationContactPersonViews.Add(new OrganizationContactPersonView { Id = 4, Deleted = false, OrganizationId = 2,  FirstName = "Req", LastName = "Req" });
        }
    }
}
