using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Accounting;
using GTIWebAPI.Models.Context;
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
    public class TestInvoicesController
    {
        private IDbContextFactory factory;
        private IInvoicesRepository repo;
        private TestDbContext db;

        public TestInvoicesController()
        {
            db = new TestDbContext();
            factory = new TestDbContextFactory(db);
            repo = new InvoicesRepository(factory);
            FillFewDemo();
        }

        [TestMethod]
        public void GetInvoices_ShouldReturnBetween1900and2200yearsAndWithSameOrganization()
        {
            var controller = new InvoicesController(repo);
            var result = controller.GetInvoiceAll(1, null, null) as OkNegotiatedContentResult<List<DealInvoiceViewDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetInvoice_ShouldReturnDealWithContainersAndLines()
        {
            var controller = new InvoicesController(repo);
            var result = controller.GetInvoice(22) as OkNegotiatedContentResult<InvoiceFullViewDTO>;
            Assert.AreEqual(22, result.Content.Id);
            Assert.AreEqual(2, result.Content.Containers.Count());
            Assert.AreEqual(1, result.Content.Lines.Count());
        }

        public void FillFewDemo()
        {
            db.DealInvoices.Add(new DealInvoiceViewDTO()
            {
                Id = 1,
                InvoiceDate = new DateTime(2201, 1, 1),
                ClientId = 1
            });
            db.DealInvoices.Add(new DealInvoiceViewDTO()
            {
                Id = 2,
                InvoiceDate = new DateTime(2005, 1, 3),
                ClientId = 1
            });
            db.DealInvoices.Add(new DealInvoiceViewDTO()
            {
                Id = 3,
                InvoiceDate = new DateTime(2005, 1, 3),
                ClientId = 2
            });


            db.InvoiceFull.Add(new InvoiceFullViewDTO()
            {
                Id = 22
            });
            db.InvoiceContainers.Add(new InvoiceContainerViewDTO()
            {
                InvoiceId = 22,
                BL = ""
            });
            db.InvoiceContainers.Add(new InvoiceContainerViewDTO()
            {
                InvoiceId = 22,
                BL = ""
            });
            db.InvoiceContainers.Add(new InvoiceContainerViewDTO()
            {
                InvoiceId = 33,
                BL = ""
            });
            db.InvoiceLines.Add(new InvoiceLineViewDTO()
            {
                InvoiceId = 33
            });
            db.InvoiceLines.Add(new InvoiceLineViewDTO()
            {
                InvoiceId = 22
            });


        }
    }
}
