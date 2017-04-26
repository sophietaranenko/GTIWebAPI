using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
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
    public class TestEmployeeMilitaryCardsController
    {
        [TestMethod]
        public void GetAllMilitaryCards_ShouldReturnNotDeleted()
        {
            var militaryCardsTestData = new List<EmployeeMilitaryCard>()
            {
                new EmployeeMilitaryCard { Id = 1, EmployeeId = 2 },
                new EmployeeMilitaryCard { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeMilitaryCard { Id = 3, EmployeeId = 3 }
            };
            var militaryCards = MockHelper.MockDbSet(militaryCardsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeMilitaryCards).Returns(militaryCards.Object);
            dbContext.Setup(d => d.Set<EmployeeMilitaryCard>()).Returns(militaryCards.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeMilitaryCardsController(factory.Object);
            var result = controller.GetEmployeeMilitaryCardAll() as OkNegotiatedContentResult<IEnumerable<EmployeeMilitaryCardDTO>>;
            Assert.AreEqual(2, result.Content.Count());
        }

        [TestMethod]
        public void GetMilitaryCardsByEmployeeId_ShouldReturn()
        {
            var militaryCardsTestData = new List<EmployeeMilitaryCard>()
            {
                new EmployeeMilitaryCard { Id = 1, EmployeeId = 2 },
                new EmployeeMilitaryCard { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeMilitaryCard { Id = 3, EmployeeId = 3 }
            };
            var militaryCards = MockHelper.MockDbSet(militaryCardsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeMilitaryCards).Returns(militaryCards.Object);
            dbContext.Setup(d => d.Set<EmployeeMilitaryCard>()).Returns(militaryCards.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeMilitaryCardsController(factory.Object);
            var result = controller.GetEmployeeMilitaryCardByEmployee(2) as OkNegotiatedContentResult<IEnumerable<EmployeeMilitaryCardDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetMilitaryCardById_ShouldReturn()
        {
            var militaryCardsTestData = new List<EmployeeMilitaryCard>()
            {
                new EmployeeMilitaryCard { Id = 1, EmployeeId = 2 },
                new EmployeeMilitaryCard { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeMilitaryCard { Id = 3, EmployeeId = 3 }
            };
            var militaryCards = MockHelper.MockDbSet(militaryCardsTestData);
            militaryCards.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return militaryCards.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeMilitaryCards).Returns(militaryCards.Object);
            dbContext.Setup(d => d.Set<EmployeeMilitaryCard>()).Returns(militaryCards.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeMilitaryCardsController(factory.Object);
            var result = controller.GetEmployeeMilitaryCardView(1) as OkNegotiatedContentResult<EmployeeMilitaryCardDTO>;
            Assert.AreEqual(1, result.Content.Id);
            Assert.AreEqual(2, result.Content.EmployeeId);
        }

        [TestMethod]
        public void PutDocument_ShouldReturnOk()
        {
            var militaryCardsTestData = new List<EmployeeMilitaryCard>()
            {
                new EmployeeMilitaryCard { Id = 1, EmployeeId = 2 },
                new EmployeeMilitaryCard { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeMilitaryCard { Id = 3, EmployeeId = 3 }
            };
            var militaryCards = MockHelper.MockDbSet(militaryCardsTestData);
            militaryCards.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return militaryCards.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeMilitaryCards).Returns(militaryCards.Object);
            dbContext.Setup(d => d.Set<EmployeeMilitaryCard>()).Returns(militaryCards.Object);

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            EmployeeMilitaryCard passport = new EmployeeMilitaryCard { Id = 3, EmployeeId = 3 };
            var controller = new EmployeeMilitaryCardsController(factory.Object);
            var result = controller.PutEmployeeMilitaryCard(3, passport) as OkNegotiatedContentResult<EmployeeMilitaryCardDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
        }

        [TestMethod]
        public void PostMilitaryCard_ShoulAddMilitaryCard()
        {
            var militaryCardsTestData = new List<EmployeeMilitaryCard>()
            {
                new EmployeeMilitaryCard { Id = 1, EmployeeId = 2 },
                new EmployeeMilitaryCard { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeMilitaryCard { Id = 3, EmployeeId = 3 }
            };
            var militaryCards = MockHelper.MockDbSet(militaryCardsTestData);
            militaryCards.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return militaryCards.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });
            militaryCards.Setup(d => d.Add(It.IsAny<EmployeeMilitaryCard>())).Returns<EmployeeMilitaryCard>((contact) =>
            {
                militaryCardsTestData.Add(contact);
                militaryCards = MockHelper.MockDbSet(militaryCardsTestData);
                return contact;
            });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeMilitaryCards).Returns(militaryCards.Object);
            dbContext.Setup(d => d.Set<EmployeeMilitaryCard>()).Returns(militaryCards.Object);

            dbContext.Setup(d => d.ExecuteStoredProcedure<int>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<int> list = new List<int>();
                   if (query.Contains("NewTableId"))
                   {
                       int i = militaryCards.Object.Max(d => d.Id) + 1;
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

            EmployeeMilitaryCard passport = new EmployeeMilitaryCard { Id = 0, EmployeeId = 3 };
            var controller = new EmployeeMilitaryCardsController(factory.Object);
            var result = controller.PostEmployeeMilitaryCard(passport) as CreatedAtRouteNegotiatedContentResult<EmployeeMilitaryCardDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Content.Id);
            Assert.AreEqual(3, result.Content.EmployeeId);
        }

        [TestMethod]
        public void DeleteMilitaryCard_ShouldDeleteAndReturnOk()
        {
            var militaryCardsTestData = new List<EmployeeMilitaryCard>()
            {
                new EmployeeMilitaryCard { Id = 1, EmployeeId = 2 },
                new EmployeeMilitaryCard { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeMilitaryCard { Id = 3, EmployeeId = 3 }
            };
            var militaryCards = MockHelper.MockDbSet(militaryCardsTestData);
            militaryCards.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return militaryCards.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeMilitaryCards).Returns(militaryCards.Object);
            dbContext.Setup(d => d.Set<EmployeeMilitaryCard>()).Returns(militaryCards.Object);


            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            EmployeeMilitaryCard passport = new EmployeeMilitaryCard { Id = 3, EmployeeId = 3 };
            var controller = new EmployeeMilitaryCardsController(factory.Object);
            var result = controller.DeleteEmployeeMilitaryCard(3) as OkNegotiatedContentResult<EmployeeMilitaryCardDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
            Assert.AreEqual(3, result.Content.EmployeeId);
        }
    }
}
