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
    public class TestEmployeeFoundationDocumentsController
    {

        [TestMethod]
        public void GetAllDocuments_ShouldReturnNotDeleted()
        {
            var docsTestData = new List<EmployeeFoundationDocument>()
            {
                new EmployeeFoundationDocument { Id = 1, EmployeeId = 2 },
                new EmployeeFoundationDocument { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeFoundationDocument { Id = 3, EmployeeId = 3 }
            };
            var docs = MockHelper.MockDbSet(docsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeFoundationDocuments).Returns(docs.Object);
            dbContext.Setup(d => d.Set<EmployeeFoundationDocument>()).Returns(docs.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeFoundationDocumentsController(factory.Object);
            var result = controller.GetEmployeeFoundationDocumentAll() as OkNegotiatedContentResult<IEnumerable<EmployeeFoundationDocumentDTO>>;
            Assert.AreEqual(2, result.Content.Count());
        }

        [TestMethod]
        public void GetDocumentsByEmployeeId_ShouldReturn()
        {
            var docsTestData = new List<EmployeeFoundationDocument>()
            {
                new EmployeeFoundationDocument { Id = 1, EmployeeId = 2 },
                new EmployeeFoundationDocument { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeFoundationDocument { Id = 3, EmployeeId = 3 }
            };
            var docs = MockHelper.MockDbSet(docsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeFoundationDocuments).Returns(docs.Object);
            dbContext.Setup(d => d.Set<EmployeeFoundationDocument>()).Returns(docs.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeFoundationDocumentsController(factory.Object);
            var result = controller.GetEmployeeFoundationDocumentByEmployee(2) as OkNegotiatedContentResult<IEnumerable<EmployeeFoundationDocumentDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetDocumentById_ShouldReturn()
        {
            var docsTestData = new List<EmployeeFoundationDocument>()
            {
                new EmployeeFoundationDocument { Id = 1, EmployeeId = 2 },
                new EmployeeFoundationDocument { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeFoundationDocument { Id = 3, EmployeeId = 3 }
            };
            var docs = MockHelper.MockDbSet(docsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeFoundationDocuments).Returns(docs.Object);
            dbContext.Setup(d => d.Set<EmployeeFoundationDocument>()).Returns(docs.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeFoundationDocumentsController(factory.Object);
            var result = controller.GetEmployeeFoundationDocument(1) as OkNegotiatedContentResult<EmployeeFoundationDocumentDTO>;
            Assert.AreEqual(1, result.Content.Id);
            Assert.AreEqual(2, result.Content.EmployeeId);
        }

        [TestMethod]
        public void PutDocument_ShouldReturnOk()
        {
            var docsTestData = new List<EmployeeFoundationDocument>()
            {
                new EmployeeFoundationDocument { Id = 1, EmployeeId = 2 },
                new EmployeeFoundationDocument { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeFoundationDocument { Id = 3, EmployeeId = 3 }
            };
            var docs = MockHelper.MockDbSet(docsTestData);
            docs.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return docs.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeFoundationDocuments).Returns(docs.Object);
            dbContext.Setup(d => d.Set<EmployeeFoundationDocument>()).Returns(docs.Object);

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            EmployeeFoundationDocument license = new EmployeeFoundationDocument { Id = 3, EmployeeId = 3, IssuedBy = "Wow" };
            var controller = new EmployeeFoundationDocumentsController(factory.Object);
            var result = controller.PutEmployeeFoundationDocument(3, license) as OkNegotiatedContentResult<EmployeeFoundationDocumentDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
        }

        [TestMethod]
        public void PostLicense_ShoulAddLicense()
        {
            var docsTestData = new List<EmployeeFoundationDocument>()
            {
                new EmployeeFoundationDocument { Id = 1, EmployeeId = 2 },
                new EmployeeFoundationDocument { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeFoundationDocument { Id = 3, EmployeeId = 3 }
            };
            var docs = MockHelper.MockDbSet(docsTestData);
            docs.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return docs.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });
            docs.Setup(d => d.Add(It.IsAny<EmployeeFoundationDocument>())).Returns<EmployeeFoundationDocument>((contact) =>
            {
                docsTestData.Add(contact);
                docs = MockHelper.MockDbSet(docsTestData);
                return contact;
            });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeFoundationDocuments).Returns(docs.Object);
            dbContext.Setup(d => d.Set<EmployeeFoundationDocument>()).Returns(docs.Object);

            dbContext.Setup(d => d.ExecuteStoredProcedure<int>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<int> list = new List<int>();
                   if (query.Contains("NewTableId"))
                   {
                       int i = docs.Object.Max(d => d.Id) + 1;
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

            EmployeeFoundationDocument license = new EmployeeFoundationDocument { Id = 0, EmployeeId = 3,  IssuedBy = "Wow" };
            var controller = new EmployeeFoundationDocumentsController(factory.Object);
            var result = controller.PostEmployeeFoundationDocument(license) as CreatedAtRouteNegotiatedContentResult<EmployeeFoundationDocumentDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Content.Id);
            Assert.AreEqual(3, result.Content.EmployeeId);
            Assert.AreEqual("Wow", result.Content.IssuedBy);
        }

        [TestMethod]
        public void DeleteDocument_ShouldDeleteAndReturnOk()
        {
            var docsTestData = new List<EmployeeFoundationDocument>()
            {
                new EmployeeFoundationDocument { Id = 1, EmployeeId = 2 },
                new EmployeeFoundationDocument { Id = 2, Deleted = true, EmployeeId = 2 },
                new EmployeeFoundationDocument { Id = 3, EmployeeId = 3 }
            };
            var docs = MockHelper.MockDbSet(docsTestData);
            docs.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return docs.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeFoundationDocuments).Returns(docs.Object);
            dbContext.Setup(d => d.Set<EmployeeFoundationDocument>()).Returns(docs.Object);


            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            EmployeeFoundationDocument license = new EmployeeFoundationDocument { Id = 3, EmployeeId = 3, IssuedBy = "Wow" };
            var controller = new EmployeeFoundationDocumentsController(factory.Object);
            var result = controller.DeleteEmployeeFoundationDocument(3) as OkNegotiatedContentResult<EmployeeFoundationDocumentDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
            Assert.AreEqual(3, result.Content.EmployeeId);
        }
    }
}
