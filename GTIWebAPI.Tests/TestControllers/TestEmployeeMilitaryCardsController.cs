using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
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
    public class TestEmployeeMilitaryCardsController
    {
        private IDbContextFactory factory;
        private IRepository<EmployeeMilitaryCard> repo;

        public TestEmployeeMilitaryCardsController()
        {
            factory = new TestDbContextFactory();
            repo = new EmployeeMilitaryCardsRepository(factory);
            GetFewDemo();
        }

        [TestMethod]
        public void GetAllCards_ShouldReturnNotDeleted()
        {
            var controller = new EmployeeMilitaryCardsController(repo);
            var result = controller.GetEmployeeMilitaryCardAll() as OkNegotiatedContentResult<List<EmployeeMilitaryCardDTO>>;
            Assert.AreEqual(3, result.Content.Count());
        }

        [TestMethod]
        public void GetCardsByEmployeeId_ShouldReturnNotDeletedCards()
        {
            var controller = new EmployeeMilitaryCardsController(repo);
            var result = controller.GetEmployeeMilitaryCardByEmployee(1) as OkNegotiatedContentResult<List<EmployeeMilitaryCardDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetCardById_ShouldReturnObjectWithSameId()
        {
            var controller = new EmployeeMilitaryCardsController(repo);
            var result = controller.GetEmployeeMilitaryCardView(1) as OkNegotiatedContentResult<EmployeeMilitaryCardDTO>;
            Assert.AreEqual(result.Content.Id, 1);
        }

        [TestMethod]
        public void PutCard_ShouldReturnOk()
        {
            var controller = new EmployeeMilitaryCardsController(repo);
            EmployeeMilitaryCard militaryCard = GetDemo();
            var result = controller.PutEmployeeMilitaryCard(militaryCard.Id, militaryCard) as OkNegotiatedContentResult<EmployeeMilitaryCardDTO>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PutCard_ShouldFail_WhenDifferentID()
        {
            var controller = new EmployeeMilitaryCardsController(repo);
            EmployeeMilitaryCard militaryCard = GetDemo();
            var badresult = controller.PutEmployeeMilitaryCard(999, militaryCard);
            Assert.IsInstanceOfType(badresult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PostCard_ShouldReturnSame()
        {
            var controller = new EmployeeMilitaryCardsController(repo);
            var item = GetDemo();
            var result = controller.PostEmployeeMilitaryCard(item) as CreatedAtRouteNegotiatedContentResult<EmployeeMilitaryCardDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "GetEmployeeMilitaryCard");
            Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
        }

        [TestMethod]
        public void DeleteCard_ShouldReturnOK()
        {
            EmployeeMilitaryCard militaryCard = GetDemo();
            militaryCard = repo.Add(militaryCard);

            var controller = new EmployeeMilitaryCardsController(repo);
            var result = controller.DeleteEmployeeMilitaryCard(militaryCard.Id) as OkNegotiatedContentResult<EmployeeMilitaryCardDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(militaryCard.Id, result.Content.Id);
        }

        private EmployeeMilitaryCard GetDemo()
        {
            EmployeeMilitaryCard militaryCard = new EmployeeMilitaryCard
            {
                Id = 0,
                Seria = "SS",
                EmployeeId = 1
            };
            return militaryCard;
        }

        private void GetFewDemo()
        {
            repo.Add(new EmployeeMilitaryCard { Id = 1, Deleted = true, EmployeeId = 1 });
            repo.Add(new EmployeeMilitaryCard { Id = 2, Deleted = false, EmployeeId = 1 });
            repo.Add(new EmployeeMilitaryCard { Id = 3, Deleted = false, EmployeeId = 2 });
            repo.Add(new EmployeeMilitaryCard { Id = 4, Deleted = false, EmployeeId = 2 });
        }
    }
}
