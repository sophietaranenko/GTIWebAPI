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
    public class TestInvoicesController 
    {
        [TestMethod]
        public void GetAllContainers_ShouldReturnContainers()
        {
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(d => d.ExecuteStoredProcedure<DealInvoiceViewDTO>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<DealInvoiceViewDTO> list = new List<DealInvoiceViewDTO>();
                   if (query.Contains("InvoicesList"))
                   {
                       list.Add(new DealInvoiceViewDTO { Id = 1 } );
                       list.Add(new DealInvoiceViewDTO { Id = 2 } );
                       list.Add(new DealInvoiceViewDTO { Id = 3 } );
                   }
                   return list;
               });

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            var controller = new InvoicesController(factory.Object);

            var result = controller.GetInvoiceAll(1, DateTime.Now, DateTime.Now) 
                as OkNegotiatedContentResult<IEnumerable<DealInvoiceViewDTO>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Count());
        }

        [TestMethod]
        public void GetOneContainer_ShouldReturnContainer()
        {
            int invoiceId = 44;
            var dbContext = new Mock<IAppDbContext>();

            dbContext.Setup(d => d.ExecuteStoredProcedure<InvoiceFullViewDTO>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<InvoiceFullViewDTO> list = new List<InvoiceFullViewDTO>();
                   if (query.Contains("InvoiceInfo"))
                   {
                       list.Add(new InvoiceFullViewDTO { Id = invoiceId });
                   }
                   else
                   {
                       list.Add(new InvoiceFullViewDTO { Id = 1 });
                   }
                   return list;
               });

            dbContext.Setup(d => d.ExecuteStoredProcedure<InvoiceContainerViewDTO>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<InvoiceContainerViewDTO> list = new List<InvoiceContainerViewDTO>();
                   if (query.Contains("InvoiceContainerList"))
                   {
                       list.Add(new InvoiceContainerViewDTO { Id = Guid.NewGuid()});
                   }
                   else
                   {
                       list.Add(new InvoiceContainerViewDTO { Id = Guid.NewGuid() });
                   }
                   return list;
               });

            dbContext.Setup(d => d.ExecuteStoredProcedure<InvoiceLineViewDTO>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<InvoiceLineViewDTO> list = new List<InvoiceLineViewDTO>();
                   if (query.Contains("InvoiceLineList"))
                   {
                       list.Add(new InvoiceLineViewDTO { Id = Guid.NewGuid() });
                   }
                   else
                   {
                       list.Add(new InvoiceLineViewDTO { Id = Guid.NewGuid() });
                   }
                   return list;
               });


            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new InvoicesController(factory.Object);

            var result = controller.GetInvoice(invoiceId) as OkNegotiatedContentResult<InvoiceFullViewDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(invoiceId, result.Content.Id);
        }
    }
}
