using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Tests.TestContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace GTIWebAPI.Tests
{
    [TestClass]
    public class TestEmployeePassportsController
    {
        private IDbContextFactory factory;
        private IRepository<EmployeePassport> repo;

        public TestEmployeePassportsController()
        {
            factory = new TestDbContextFactory();
            repo = new EmployeePassportsRepository(factory);
            GetFewDemo();
        }

        [TestMethod]
        public void GetAll_ShouldReturnNotDeleted()
        {
            var controller = new EmployeePassportsController(repo);
            var result = controller.GetEmployeePassportAll() as OkNegotiatedContentResult<List<EmployeePassportDTO>>;
            Assert.AreEqual(3, result.Content.Count());
        }

        [TestMethod]
        public void GetByEmployeeId_ShouldReturnNotDeletedEmployeesPassport()
        {
            var controller = new EmployeePassportsController(repo);
            var result = controller.GetEmployeePassportByEmployee(1) as OkNegotiatedContentResult<List<EmployeePassportDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }


        [TestMethod]
        public void GetByEmployeeId_ShouldReturnZeroCount()
        {
            var controller = new GTIWebAPI.Controllers.EmployeePassportsController(repo);
            var badResult = controller.GetEmployeePassportByEmployee(999) as OkNegotiatedContentResult<List<EmployeePassportDTO>>;
            Assert.AreEqual(0, badResult.Content.Count());
        }

        [TestMethod]
        public void GetPassportById_ShouldReturnObjectWithSameId()
        {
            var controller = new EmployeePassportsController(repo);
            var result = controller.GetEmployeePassport(1) as OkNegotiatedContentResult<EmployeePassportDTO>;
            Assert.AreEqual(result.Content.Id, 1);
        }

        [TestMethod]
        public void GetByPassportId_ShouldReturnBadRequestWhenIdIsNotFound()
        {
            var controller = new EmployeePassportsController(repo);
            var badResult = controller.GetEmployeePassport(999);
            Assert.IsInstanceOfType(badResult, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void PutPassport_ShouldReturnOk()
        {
            var controller = new EmployeePassportsController(repo);
            EmployeePassport passport = GetDemo();

            var resultAdd = controller.PostEmployeePassport(passport) as CreatedAtRouteNegotiatedContentResult<EmployeePassportDTO>;
            EmployeePassportDTO dto = resultAdd.Content;

            var result = controller.PutEmployeePassport(dto.Id, passport) as OkNegotiatedContentResult<EmployeePassportDTO>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PutPassport_ShouldFail_WhenDifferentID()
        {
            var controller = new EmployeePassportsController(repo);
            EmployeePassport passport = GetDemo();
            var badresult = controller.PutEmployeePassport(999, passport);
            Assert.IsInstanceOfType(badresult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PostPassport_ShouldReturnSamePassport()
        {
            var controller = new EmployeePassportsController(repo);
            var item = GetDemo();
            var result = controller.PostEmployeePassport(item) as CreatedAtRouteNegotiatedContentResult<EmployeePassportDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "GetEmployeePassport");
            Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
            Assert.AreEqual(result.Content.FirstName, item.FirstName);
        }

        [TestMethod]
        public void DeletePassport_ShouldReturnOK()
        {
            EmployeePassport passport = GetDemo();
            passport = repo.Add(passport);

            var controller = new EmployeePassportsController(repo);
            var result = controller.DeleteEmployeePassport(passport.Id) as OkNegotiatedContentResult<EmployeePassportDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(passport.Id, result.Content.Id);
        }

        private EmployeePassport GetDemo()
        {
            EmployeePassport passport = new EmployeePassport
            {
                Id = 0,
                Seria = "SS",
                FirstName = "Иван",
                EmployeeId = 1,
                AddressId = 22,
                Address = new Address
                {
                    Id = 22
                }
            };
            return passport;
        }

        private void GetFewDemo()
        {
            repo.Add(new EmployeePassport
            {
                Id = 1,
                Deleted = true,
                EmployeeId = 1,
                AddressId = 1,
                Address = new Address
                {
                    Id = 1
                }
            });

            repo.Add(new EmployeePassport
            {
                Id = 2,
                Deleted = false,
                EmployeeId = 1,
                AddressId = 2,
                Address = new Address
                {
                    Id = 2
                }
            });

            repo.Add(new EmployeePassport
            {
                Id = 3,
                Deleted = false,
                EmployeeId = 2,
                AddressId = 3,
                Address = new Address
                {
                    Id = 3
                }
            });

            repo.Add(new EmployeePassport
            {
                Id = 4,
                Deleted = false,
                EmployeeId = 2,
                AddressId = 4,
                Address = new Address
                {
                    Id = 4
                }
            });
        }
    }


}
