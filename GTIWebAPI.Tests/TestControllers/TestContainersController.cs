using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Accounting;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Repository.Accounting;
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
    public class TestContainersController
    {
        private IDbContextFactory factory;
        private IContainersRepository repo;
        private TestDbContext db;

        public TestContainersController()
        {
            db = new TestDbContext();
            factory = new TestDbContextFactory(db);
            repo = new ContainersRepository(factory);
            FillFewDemo();
        }

        [TestMethod]
        public void GetAllContainers_ShouldReturnBetween1900and2200()
        {
            var controller = new ContainersController(repo);
            var result = controller.GetContainers(1, null, null) as OkNegotiatedContentResult<List<DealContainerViewDTO>>;
            Assert.AreEqual(4, result.Content.Count);
        }

        [TestMethod]
        public void GetContainer_ShouldReturnContainerWithSameId()
        {
            var controller = new ContainersController(repo);
            var result = controller.GetContainer(db.Containers.Take(1).FirstOrDefault().Id) as OkNegotiatedContentResult<DealContainerViewDTO>;
            Assert.AreEqual(db.Containers.Take(1).FirstOrDefault().Id, result.Content.Id);
        }

        public void FillFewDemo()
        {
            db.Containers.Add(
                new DealContainerViewDTO
                {
                    Container = "MRKU1567899",
                    Seal = "some Seal",
                    Id = Guid.NewGuid(),
                    DealId = Guid.NewGuid()
                    });
            db.Containers.Add(
                new DealContainerViewDTO
                {
                    Container = "PRNU1567899",
                    Seal = "some Seal",
                    Id = Guid.NewGuid()
                });
            db.Containers.Add(
                new DealContainerViewDTO
                {
                    Container = "PONU1567899",
                    Seal = "some Seal",
                    Id = Guid.NewGuid()
                });
            db.Containers.Add(
                new DealContainerViewDTO
                {
                    Container = "MONU1567899",
                    Seal = "some Seal",
                    Id = Guid.NewGuid()
                });
        }
    }
}
