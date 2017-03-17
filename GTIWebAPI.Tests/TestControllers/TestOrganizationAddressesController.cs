using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Repository;
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
    public class TestOrganizationAddressesController
    {
        private IDbContextFactory factory;
        private IOrganizationRepository<OrganizationAddress> repo;

        public TestOrganizationAddressesController()
        {
            factory = new TestDbContextFactory();
            repo = new OrganizationAddressesRepository(factory);
            GetFewDemo();
        }

        [TestMethod]
        public void GetByOrganizationId_ShouldReturnNotDeletedOrganizationIdsAddresses()
        {
            var controller = new OrganizationAddressesController(repo);
            var result = controller.GetOrganizationAddressByOrganizationId(1) as OkNegotiatedContentResult<List<OrganizationAddressDTO>>;
            Assert.AreEqual(1, result.Content.Count());
            Assert.AreEqual(1, result.Content.Select(d => d.OrganizationId).Distinct().Count());
            Assert.AreEqual(1, result.Content.Select(d => d.OrganizationId).Distinct().Take(1).FirstOrDefault());
        }

        [TestMethod]
        public void GetById_ShouldReturnObjectWithSameId()
        {
            var controller = new OrganizationAddressesController(repo);
            var result = controller.GetOrganizationAddress(1) as OkNegotiatedContentResult<OrganizationAddressDTO>;
            Assert.AreEqual(result.Content.Id, 1);
        }

        [TestMethod]
        public void Put_ShouldReturnOk()
        {
            var controller = new OrganizationAddressesController(repo);
            OrganizationAddress address = repo.Add(GetDemo());
            var result = controller.PutOrganizationAddress(address.Id, address) as OkNegotiatedContentResult<OrganizationAddressDTO>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Put_ShouldFail_WhenDifferentID()
        {
            var controller = new OrganizationAddressesController(repo);
            OrganizationAddress address = GetDemo();
            var badresult = controller.PutOrganizationAddress(999, address);
            Assert.IsInstanceOfType(badresult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Post_ShouldReturnSame()
        {
            var controller = new OrganizationAddressesController(repo);
            var item = GetDemo();
            var result = controller.PostOrganizationAddress(item) as CreatedAtRouteNegotiatedContentResult<OrganizationAddressDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "GetOrganizationAddress");
            Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
        }

        [TestMethod]
        public void Delete_ShouldReturnOK()
        {
            OrganizationAddress address = repo.Add(GetDemo());

            var controller = new OrganizationAddressesController(repo);
            var result = controller.DeleteOrganizationAddress(address.Id) as OkNegotiatedContentResult<OrganizationAddressDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(address.Id, result.Content.Id);
        }

        private OrganizationAddress GetDemo()
        {
            OrganizationAddress gun = new OrganizationAddress
            {
                Id = 0,
                OrganizationId = 1,
                AddressId = 0,
                Address = new Models.Dictionary.Address { Id = 1 }
            };
            return gun;
        }

        private void GetFewDemo()
        {
            repo.Add(new OrganizationAddress { Id = 1, Deleted = true, OrganizationId = 1, AddressId = 0, Address = new Models.Dictionary.Address { Id = 1 } });
            repo.Add(new OrganizationAddress { Id = 2, Deleted = false, OrganizationId = 1, AddressId = 0, Address = new Models.Dictionary.Address { Id = 1 } });
            repo.Add(new OrganizationAddress { Id = 3, Deleted = false, OrganizationId = 2, AddressId = 0, Address = new Models.Dictionary.Address { Id = 1 } });
            repo.Add(new OrganizationAddress { Id = 4, Deleted = false, OrganizationId = 2, AddressId = 0, Address = new Models.Dictionary.Address { Id = 1 } });
        }
    }
}
