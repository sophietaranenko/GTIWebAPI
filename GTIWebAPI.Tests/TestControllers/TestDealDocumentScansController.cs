using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GTIWebAPI.Models.Accounting;
using GTIWebAPI.Models.Context;
using System.Collections.Generic;
using GTIWebAPI.Controllers;
using System.Web.Http.Results;
using System.Linq;
using System.Security.Principal;
using GTIWebAPI.Models.Service;

namespace GTIWebAPI.Tests.TestControllers
{
    [TestClass]
    public class TestDealDocumentScansController
    {
        [TestMethod]
        public void GetAllScans_ShouldReturnAll()
        {
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(d => d.ExecuteStoredProcedure<DocumentScanTypeDTO>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<DocumentScanTypeDTO> list = new List<DocumentScanTypeDTO>();
                   if (query.Contains("GetDocumentScanTypes"))
                   {
                       list.Add(new DocumentScanTypeDTO { Id = 1 });
                       list.Add(new DocumentScanTypeDTO { Id = 2 });
                       list.Add(new DocumentScanTypeDTO { Id = 3 });
                   }
                   else
                   {
                       list.Add(new DocumentScanTypeDTO { Id = 11 });
                   }
                   return list;
               });

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            var controller = new DealDocumentScansController(factory.Object);

            var result = controller.GetDocumentScanTypes() as OkNegotiatedContentResult<IEnumerable<DocumentScanTypeDTO>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Count());
        }

        [TestMethod]
        public void GetScansByDealId_ShouldReturnScans()
        {
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(d => d.ExecuteStoredProcedure<DocumentScanDTO>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<DocumentScanDTO> list = new List<DocumentScanDTO>();
                   if (query.Contains("GetDocumentScanByDeal"))
                   {
                       list.Add(new DocumentScanDTO { Id = Guid.NewGuid() });
                       list.Add(new DocumentScanDTO { Id = Guid.NewGuid() });
                       list.Add(new DocumentScanDTO { Id = Guid.NewGuid() });
                   }
                   else
                   {
                       list.Add(new DocumentScanDTO { Id = Guid.NewGuid() });
                   }
                   return list;
               });

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            var controller = new DealDocumentScansController(factory.Object);

            var result = controller.GetDocumentScansByDealId(Guid.NewGuid()) as OkNegotiatedContentResult<IEnumerable<DocumentScanDTO>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Count());
        }

        [TestMethod]
        public void PutDealDocumentScan_ShoulPut()
        {
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(d => d.ExecuteStoredProcedure<DocumentScanDTO>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<DocumentScanDTO> list = new List<DocumentScanDTO>();
                   if (query.Contains("GetDocumentScanById"))
                   {
                       list.Add(new DocumentScanDTO { Id = Guid.NewGuid(), ComputerName = "sometestemail" });
                       list.Add(new DocumentScanDTO { Id = Guid.NewGuid(), ComputerName = "sometestemail" });
                       list.Add(new DocumentScanDTO { Id = Guid.NewGuid(), ComputerName = "sometestemail" });
                   }
                   else
                   {
                       if (query.Contains("UpdateDocumentScanType"))
                       {
                           list.Add(new DocumentScanDTO { Id = Guid.NewGuid(), ComputerName = "sometestemail" });
                       }
                       else
                       {
                           list.Add(new DocumentScanDTO { Id = Guid.NewGuid(), ComputerName = "sometestemail" });
                       }
                   }
                   return list;
               });

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            var identityHelper = new Mock<IIdentityHelper>();
            identityHelper.Setup(d => d.GetUserEmail(It.IsAny<IPrincipal>())).Returns("sometestemail");

            var controller = new DealDocumentScansController(factory.Object, identityHelper.Object);
            var result = controller.PutDealDocumentScan(Guid.NewGuid(), 1) as OkNegotiatedContentResult<DocumentScanDTO>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PostDealDocumentScan_ShouldPost()
        {
            Guid newFileGuid = Guid.NewGuid();
            Guid dealGuid = Guid.NewGuid();

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(d => d.ExecuteStoredProcedure<Guid>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<Guid> list = new List<Guid>();
                   if (query.Contains("InsertDocumentScanByDeal"))
                   {
                       list.Add(newFileGuid);
                   }
                   return list;
               });

            dbContext.Setup(d => d.ExecuteStoredProcedure<DocumentScanTypeDTO>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<DocumentScanTypeDTO> list = new List<DocumentScanTypeDTO>();
                   if (query.Contains("GetDocumentScanTypes"))
                   {
                       list.Add(new DocumentScanTypeDTO { Id = 1 });
                       list.Add(new DocumentScanTypeDTO { Id = 2 });
                       list.Add(new DocumentScanTypeDTO { Id = 3 });
                   }
                   return list;
               });

            dbContext.Setup(d => d.ExecuteStoredProcedure<DocumentScanDTO>(It.IsAny<string>(), It.IsAny<object[]>()))
              .Returns<string, object[]>((query, parameters) =>
              {
                  List<DocumentScanDTO> list = new List<DocumentScanDTO>();
                  if (query.Contains("GetDocumentScanById"))
                  {
                      list.Add(new DocumentScanDTO { Id = newFileGuid, ComputerName = "sometestemail" });
                  }
                  return list;
              });

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            var identityHelper = new Mock<IIdentityHelper>();
            identityHelper.Setup(d => d.GetUserEmail(It.IsAny<IPrincipal>())).Returns("sometestemail");

            var request = new Mock<IRequest>();
            request.Setup(d => d.Collection()).Returns(new List<string> { "sss" });
            request.Setup(d => d.FileCount()).Returns(1);
            request.Setup(d => d.GetBytes(It.IsAny<string>())).Returns(Convert.FromBase64String("AAA55HH67HAAAAAABBB12345"));

            var controller = new DealDocumentScansController(factory.Object, identityHelper.Object, request.Object);
            var result = controller.UploadDealDocumentScan(1, dealGuid) as OkNegotiatedContentResult<DocumentScanDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual("sometestemail", result.Content.ComputerName);
            Assert.AreEqual(newFileGuid, result.Content.Id);
        }

        [TestMethod]
        public void DeleteDocumentScan_ShouldDelete()
        {
            Guid toDeleteFileGuid = Guid.NewGuid();
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(d => d.ExecuteStoredProcedure<DocumentScanDTO>(It.IsAny<string>(), It.IsAny<object[]>()))
              .Returns<string, object[]>((query, parameters) =>
              {
                  List<DocumentScanDTO> list = new List<DocumentScanDTO>();
                  if (query.Contains("GetDocumentScanById"))
                  {
                      list.Add(new DocumentScanDTO { Id = toDeleteFileGuid, ComputerName = "sometestemail" });
                  }
                  return list;
              });

            dbContext.Setup(d => d.ExecuteStoredProcedure<bool>(It.IsAny<string>(), It.IsAny<object[]>()))
              .Returns<string, object[]>((query, parameters) =>
              {
                  List<bool> list = new List<bool>();
                  if (query.Contains("DeleteDocumentScan"))
                  {
                      list.Add(true);
                  }
                  return list;
              });

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            var controller = new DealDocumentScansController(factory.Object);
            var result = controller.DeleteDealDocumentScan(toDeleteFileGuid) as OkNegotiatedContentResult<DocumentScanDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(toDeleteFileGuid, result.Content.Id);
        }
    }
}
