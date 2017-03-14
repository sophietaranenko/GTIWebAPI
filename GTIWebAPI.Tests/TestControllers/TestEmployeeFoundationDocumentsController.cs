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
    public class TestEmployeeFoundationDocumentsController
    {
        private IDbContextFactory factory;
        private IRepository<EmployeeFoundationDocument> repo;

        public TestEmployeeFoundationDocumentsController()
        {
            factory = new TestDbContextFactory();
            repo = new EmployeeFoundationDocumentsRepository(factory);
            GetFewDemo();
        }

        [TestMethod]
        public void GetAll_ShouldReturnNotDeleted()
        {
            var controller = new EmployeeFoundationDocumentsController(repo);
            var result = controller.GetEmployeeFoundationDocumentAll() as OkNegotiatedContentResult<IEnumerable<EmployeeFoundationDocumentDTO>>;
            Assert.AreEqual(3, result.Content.Count());
        }

        [TestMethod]
        public void GetByEmployeeId_ShouldReturnNotDeletedEmployeesPassport()
        {
            var controller = new EmployeeFoundationDocumentsController(repo);
            var result = controller.GetEmployeeFoundationDocumentByEmployee(1) as OkNegotiatedContentResult<IEnumerable<EmployeeFoundationDocumentDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetById_ShouldReturnObjectWithSameId()
        {
            var controller = new EmployeeFoundationDocumentsController(repo);
            var result = controller.GetEmployeeFoundationDocument(1) as OkNegotiatedContentResult<EmployeeFoundationDocumentDTO>;
            Assert.AreEqual(result.Content.Id, 1);
        }

        [TestMethod]
        public void Put_ShouldReturnOk()
        {
            var controller = new EmployeeFoundationDocumentsController(repo);
            EmployeeFoundationDocument foundationDocument = GetDemo();

            var addResult = controller.PostEmployeeFoundationDocument(foundationDocument) as CreatedAtRouteNegotiatedContentResult<EmployeeFoundationDocumentDTO>;
            EmployeeFoundationDocumentDTO dto = addResult.Content;

            var result = controller.PutEmployeeFoundationDocument(dto.Id, foundationDocument) as OkNegotiatedContentResult<EmployeeFoundationDocumentDTO>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Put_ShouldFail_WhenDifferentID()
        {
            var controller = new EmployeeFoundationDocumentsController(repo);
            EmployeeFoundationDocument foundationDocument = GetDemo();
            var badresult = controller.PutEmployeeFoundationDocument(999, foundationDocument);
            Assert.IsInstanceOfType(badresult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Post_ShouldReturnSame()
        {
            var controller = new EmployeeFoundationDocumentsController(repo);
            var item = GetDemo();
            var result = controller.PostEmployeeFoundationDocument(item) as CreatedAtRouteNegotiatedContentResult<EmployeeFoundationDocumentDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "GetEmployeeFoundationDocument");
            Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
            Assert.AreEqual(result.Content.FoundationDocumentId, item.FoundationDocumentId);
            Assert.AreEqual(result.Content.Number, item.Number);
        }

        [TestMethod]
        public void Delete_ShouldReturnOK()
        {
            EmployeeFoundationDocument foundationDocument = GetDemo();
            foundationDocument = repo.Add(foundationDocument);

            var controller = new EmployeeFoundationDocumentsController(repo);
            var result = controller.DeleteEmployeeFoundationDocument(foundationDocument.Id) as OkNegotiatedContentResult<EmployeeFoundationDocumentDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(foundationDocument.Id, result.Content.Id);
        }

        private EmployeeFoundationDocument GetDemo()
        {
            EmployeeFoundationDocument foundationDocument = new EmployeeFoundationDocument
            {
                Id = 0,
                Seria = "SS",
                EmployeeId = 1
            };
            return foundationDocument;
        }

        private void GetFewDemo()
        {
            repo.Add(new EmployeeFoundationDocument { Id = 1, Deleted = true, EmployeeId = 1 });
            repo.Add(new EmployeeFoundationDocument { Id = 2, Deleted = false, EmployeeId = 1 });
            repo.Add(new EmployeeFoundationDocument { Id = 3, Deleted = false, EmployeeId = 2 });
            repo.Add(new EmployeeFoundationDocument { Id = 4, Deleted = false, EmployeeId = 2 });
        }
    }
}
