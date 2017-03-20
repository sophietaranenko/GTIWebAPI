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
    public class TestDealsController
    {
        private IDbContextFactory factory;
        private IDealsRepository repo;
        private TestDbContext db;

        public TestDealsController()
        {
            db = new TestDbContext();
            factory = new TestDbContextFactory(db);
            repo = new DealsRepository(factory);
            FillFewDemo();
        }

        [TestMethod]
        public void GetDeals_ShouldReturnBetween1900and2200years()
        {
            var controller = new DealsController(repo);
            var result = controller.GetDeals(1, null, null) as OkNegotiatedContentResult<List<DealViewDTO>>;
            Assert.AreEqual(1, result.Content.Count);
        }

        [TestMethod]
        public void GetDeal_ShouldReturnDealWithContainersAndInvoices()
        {
            var controller = new DealsController(repo);
            var result = controller.GetDealView("3FA98452-ACD4-4BF5-9463-71637789493B") as OkNegotiatedContentResult<DealFullViewDTO>;
            Assert.AreEqual(Guid.Parse("3FA98452-ACD4-4BF5-9463-71637789493B"), result.Content.Id);
            Assert.AreEqual(2, result.Content.Containers.Count());
            Assert.AreEqual(1, result.Content.Invoices.Count());
        }

        public void FillFewDemo()
        {
            db.Deals.Add(
                new DealFullViewDTO()
                {
                    Id = Guid.Parse("3FA98452-ACD4-4BF5-9463-71637789493B"),
                    Number = 1111
                });
            db.Deals.Add(
                new DealFullViewDTO()
                {
                    Id = Guid.Parse("CDF0F057-C63E-45AC-8D68-A6171F9D3CED"),
                    Number = 222
                });




            db.Containers.Add(new DealContainerViewDTO()
            {
                Id = Guid.NewGuid(),
                DealId = Guid.Parse("3FA98452-ACD4-4BF5-9463-71637789493B"),
                Container = "MRKU7776665"
            });
            db.Containers.Add(new DealContainerViewDTO()
            {
                Id = Guid.NewGuid(),
                DealId = Guid.Parse("3FA98452-ACD4-4BF5-9463-71637789493B"),
                Container = "MRKU7776665"
            });
            db.Containers.Add(new DealContainerViewDTO()
            {
                Id = Guid.NewGuid(),
                DealId = Guid.Parse("CDF0F057-C63E-45AC-8D68-A6171F9D3CED"),
                Container = "MRKU7776665"
            });



            db.DealInvoices.Add(new DealInvoiceViewDTO()
            {
                DealId = Guid.Parse("CDF0F057-C63E-45AC-8D68-A6171F9D3CED"),
                Id = 1
            });

            db.DealInvoices.Add(new DealInvoiceViewDTO()
            {
                DealId = Guid.Parse("3FA98452-ACD4-4BF5-9463-71637789493B"),
                Id = 2
            });


            db.DealsView.Add(new DealViewDTO()
            {
                Id = Guid.NewGuid(),
                CreateDate = new DateTime(2000, 1, 1)
            });
            db.DealsView.Add(new DealViewDTO()
            {
                Id = Guid.NewGuid(),
                CreateDate = new DateTime(2201, 1, 1)
            });
        }
    }
}
