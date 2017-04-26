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
    public class TestContainerController 
    {
        [TestMethod]
        public void GetAllContainers_ShouldReturnContainers()
        {
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(d => d.ExecuteStoredProcedure<DealContainerViewDTO>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<DealContainerViewDTO> list = new List<DealContainerViewDTO>();
                   if (query.Contains("ContainersList"))
                   {
                       list.Add(new DealContainerViewDTO { Id = Guid.NewGuid() });
                       list.Add(new DealContainerViewDTO { Id = Guid.NewGuid() });
                       list.Add(new DealContainerViewDTO { Id = Guid.NewGuid() });
                   }
                   else
                   {
                       list.Add(new DealContainerViewDTO { Id = Guid.NewGuid() });
                   }
                   return list;
               });

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            var controller = new ContainersController(factory.Object);

            var result = controller.GetContainers(1, DateTime.Now, DateTime.Now) 
                as OkNegotiatedContentResult<IEnumerable<DealContainerViewDTO>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Count());
        }

        [TestMethod]
        public void GetOneContainer_ShouldReturnContainer()
        {
            Guid cntrId = Guid.NewGuid();
            var dbContext = new Mock<IAppDbContext>();

            dbContext.Setup(d => d.ExecuteStoredProcedure<DealContainerViewDTO>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<DealContainerViewDTO> list = new List<DealContainerViewDTO>();
                   if (query.Contains("ContainerFullView"))
                   {
                       list.Add(new DealContainerViewDTO { Id = cntrId });
                   }
                   else
                   {
                       list.Add(new DealContainerViewDTO { Id = Guid.NewGuid() });
                   }
                   return list;
               });

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new ContainersController(factory.Object);

            var result = controller.GetContainer(cntrId) as OkNegotiatedContentResult<DealContainerViewDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(cntrId, result.Content.Id);
        }
    }
}
