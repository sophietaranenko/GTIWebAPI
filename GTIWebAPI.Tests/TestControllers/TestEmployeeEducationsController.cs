using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Tests.TestContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;
using GTIWebAPI.Controllers;
using System.Data.Entity;
using Moq;

namespace GTIWebAPI.Tests.TestControllers
{
    [TestClass]
    public class TestEmployeeEducationsController
    {
        [TestMethod]
        public void GetAllEducations_ShouldReturnNotDeletedEducations()
        {
            var educationsTestData = new List<EmployeeEducation>()
            {
                new EmployeeEducation { Id = 1, EmployeeId = 2 },
                new EmployeeEducation { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeEducation { Id = 3, EmployeeId = 3 }
            };
            var educations = MockHelper.MockDbSet(educationsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeEducations).Returns(educations.Object);
            dbContext.Setup(d => d.Set<EmployeeEducation>()).Returns(educations.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            IDbContextFactory fac = factory.Object;
            var controller = new EmployeeEducationsController(fac);
            var result = controller.GetEmployeeEducation() as OkNegotiatedContentResult<IEnumerable<EmployeeEducationDTO>>;
            Assert.AreEqual(2, result.Content.Count());
        }

        [TestMethod]
        public void GetEducationsByEmployeeId_ShouldReturnNotDeletedEducations()
        {
            var educationsTestData = new List<EmployeeEducation>()
            {
                new EmployeeEducation { Id = 1, EmployeeId = 2 },
                new EmployeeEducation { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeEducation { Id = 3, EmployeeId = 3 }
            };
            var educations = MockHelper.MockDbSet(educationsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeEducations).Returns(educations.Object);
            dbContext.Setup(d => d.Set<EmployeeEducation>()).Returns(educations.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            IDbContextFactory fac = factory.Object;
            var controller = new EmployeeEducationsController(fac);
            var result = controller.GetEmployeeEducationByEmployeeId(2) as OkNegotiatedContentResult<IEnumerable<EmployeeEducationDTO>>;
            Assert.AreEqual(1, result.Content.Count());
            Assert.AreEqual(1, result.Content.FirstOrDefault().Id);
        }

        [TestMethod]
        public void GetEducationById_ShouldReturnObjectWithSameId()
        {
            var educationsTestData = new List<EmployeeEducation>()
            {
                new EmployeeEducation { Id = 1, EmployeeId = 2 },
                new EmployeeEducation { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeEducation { Id = 3, EmployeeId = 3 }
            };
            var educations = MockHelper.MockDbSet(educationsTestData);
            educations.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return educations.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeEducations).Returns(educations.Object);
            dbContext.Setup(d => d.Set<EmployeeEducation>()).Returns(educations.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            IDbContextFactory fac = factory.Object;
            var controller = new EmployeeEducationsController(fac);
            var result = controller.GetEmployeeEducationView(1) as OkNegotiatedContentResult<EmployeeEducationDTO>;
            Assert.AreEqual(result.Content.Id, 1);
            Assert.AreEqual(result.Content.EmployeeId, 2);
        }

        [TestMethod]
        public void PutEducation_ShouldReturnOk()
        {
            var educationsTestData = new List<EmployeeEducation>()
            {
                new EmployeeEducation { Id = 1, EmployeeId = 2 },
                new EmployeeEducation { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeEducation { Id = 3, EmployeeId = 3 }
            };
            var educations = MockHelper.MockDbSet(educationsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeEducations).Returns(educations.Object);
            dbContext.Setup(d => d.Set<EmployeeEducation>()).Returns(educations.Object);

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            IDbContextFactory fac = factory.Object;
            var controller = new EmployeeEducationsController(fac);

            EmployeeEducation car = new EmployeeEducation { Id = 3, Deleted = null, EmployeeId = 3 };

            var result = controller.PutEmployeeEducation(3, car.ToDTO()) as OkNegotiatedContentResult<EmployeeEducationDTO>;
       
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
        }

        [TestMethod]
        public void PutEducation_ShouldFail_WhenDifferentID()
        {
            var educationsTestData = new List<EmployeeEducation>()
            {
                new EmployeeEducation { Id = 1, EmployeeId = 2 },
                new EmployeeEducation { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeEducation { Id = 3, EmployeeId = 3 }
            };
            var educations = MockHelper.MockDbSet(educationsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeEducations).Returns(educations.Object);
            dbContext.Setup(d => d.Set<EmployeeEducation>()).Returns(educations.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            IDbContextFactory fac = factory.Object;
            var controller = new EmployeeEducationsController(fac);

            EmployeeEducation car = new EmployeeEducation { Id = 1, Deleted = null, EmployeeId = 1 };
            var badresult = controller.PutEmployeeEducation(999, car.ToDTO());
            Assert.IsInstanceOfType(badresult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PostEducation_ShouldReturnSame()
        {
            var educationsTestData = new List<EmployeeEducation>()
            {
                new EmployeeEducation { Id = 1, EmployeeId = 2 },
                new EmployeeEducation { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeEducation { Id = 3, EmployeeId = 3 }
            };
            var educations = MockHelper.MockDbSet(educationsTestData);
            educations.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return educations.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });
            educations.Setup(d => d.Add(It.IsAny<EmployeeEducation>())).Returns<EmployeeEducation>((contact) =>
            {
                educationsTestData.Add(contact);
                educations = MockHelper.MockDbSet(educationsTestData);
                return contact;
            });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeEducations).Returns(educations.Object);
            dbContext.Setup(d => d.Set<EmployeeEducation>()).Returns(educations.Object);
            dbContext.Setup(d => d.ExecuteStoredProcedure<int>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<int> list = new List<int>();
                   if (query.Contains("NewTableId"))
                   {
                       int i = educations.Object.Max(d => d.Id) + 1;
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

            IDbContextFactory fac = factory.Object;
            var controller = new EmployeeEducationsController(fac);

            EmployeeEducation item = new EmployeeEducation { Id = 0, Deleted = null, EmployeeId = 3 };
            var result = controller.PostEmployeeEducation(item.ToDTO()) as CreatedAtRouteNegotiatedContentResult<EmployeeEducationDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "GetEmployeeEducation");
            Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
        }

        [TestMethod]
        public void DeleteEducation_ShouldReturnOK()
        {
            var educationsTestData = new List<EmployeeEducation>()
            {
                new EmployeeEducation { Id = 1, EmployeeId = 2 },
                new EmployeeEducation { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeEducation { Id = 3, EmployeeId = 3 }
            };
            object ids = 344;
            educationsTestData.Find(d => d.Id == Int32.Parse(ids.ToString()));

            var educations = MockHelper.MockDbSet(educationsTestData);
            educations.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return educations.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeEducations).Returns(educations.Object);
            dbContext.Setup(d => d.Set<EmployeeEducation>()).Returns(educations.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            IDbContextFactory fac = factory.Object;
            var controller = new EmployeeEducationsController(fac);

            var result = controller.DeleteEmployeeEducation(1) as OkNegotiatedContentResult<EmployeeEducationDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Content.Id);
        }

        
    }
}
