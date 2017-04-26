using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using GTIWebAPI.Tests.TestContext;
using GTIWebAPI.Models.Accounting;
using Moq;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Controllers;
using System.Web.Http.Results;
using System.Linq;

namespace GTIWebAPI.Tests.TestControllers
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetAllDeals_ShouldReturnDeals()
        {
            var dealsTestData = new List<DealViewDTO>()
            {
                new DealViewDTO { Id = Guid.NewGuid()  },
                new DealViewDTO { Id = Guid.NewGuid() },
                new DealViewDTO { Id = Guid.NewGuid() }
            };
            var cars = MockHelper.MockDbSet(dealsTestData);

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(d => d.ExecuteStoredProcedure<DealViewDTO>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<DealViewDTO> list = new List<DealViewDTO>();
                   if (query.Contains("DealsFilter"))
                   {
                       list.Add(new DealViewDTO { Id = Guid.NewGuid() });
                       list.Add(new DealViewDTO { Id = Guid.NewGuid() });
                       list.Add(new DealViewDTO { Id = Guid.NewGuid() });
                   }
                   else
                   {
                       list.Add(new DealViewDTO { Id = Guid.NewGuid() });
                   }
                   return list;
               });

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new DealsController(factory.Object);

            var result = controller.GetDeals(1, DateTime.Now, DateTime.Now) as OkNegotiatedContentResult<IEnumerable<DealViewDTO>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Count());
        }

        [TestMethod]
        public void GetOneDeal_ShouldReturnDeal()
        {
            Guid dealId = Guid.NewGuid();
            var dbContext = new Mock<IAppDbContext>();

            dbContext.Setup(d => d.ExecuteStoredProcedure<DealFullViewDTO>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<DealFullViewDTO> list = new List<DealFullViewDTO>();
                   if (query.Contains("DealInfo"))
                   {
                       list.Add(new DealFullViewDTO { Id = dealId });
                   }
                   else
                   {
                       list.Add(new DealFullViewDTO { Id = Guid.NewGuid() });
                   }
                   return list;
               });

            dbContext.Setup(d => d.ExecuteStoredProcedure<DealContainerViewDTO>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<DealContainerViewDTO> list = new List<DealContainerViewDTO>();
                   if (query.Contains("DealContainersList"))
                   {
                       list.Add(new DealContainerViewDTO { Id = Guid.NewGuid(), DealId = dealId });
                   }
                   else
                   {
                       list.Add(new DealContainerViewDTO { Id = Guid.NewGuid() });
                   }
                   return list;
               });

            dbContext.Setup(d => d.ExecuteStoredProcedure<DealInvoiceViewDTO>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<DealInvoiceViewDTO> list = new List<DealInvoiceViewDTO>();
                   if (query.Contains("DealInvoicesList"))
                   {
                       list.Add(new DealInvoiceViewDTO { Id = 1, DealId = dealId });
                   }
                   else
                   {
                       list.Add(new DealInvoiceViewDTO { Id = 22});
                   }
                   return list;
               });

            dbContext.Setup(d => d.ExecuteStoredProcedure<DocumentScanDTO>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<DocumentScanDTO> list = new List<DocumentScanDTO>();
                   if (query.Contains("GetDocumentScanByDeal"))
                   {
                       list.Add(new DocumentScanDTO { Id = Guid.NewGuid() });
                   }
                   else
                   {
                       list.Add(new DocumentScanDTO { Id = Guid.NewGuid() });
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
                   }
                   else
                   {
                       list.Add(new DocumentScanTypeDTO { Id = 2 });
                   }
                   return list;
               });

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new DealsController(factory.Object);

            var result = controller.GetDealView(dealId.ToString()) as OkNegotiatedContentResult<DealFullViewDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(dealId, result.Content.Id);
        }
    }
}
