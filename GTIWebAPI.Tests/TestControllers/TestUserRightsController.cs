using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using GTIWebAPI.Models.Accounting;
using Moq;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Controllers;
using System.Web.Http.Results;
using System.Linq;
using GTIWebAPI.Models.Security;


namespace GTIWebAPI.Tests.TestControllers
{

    [TestClass]
    public class TestUserRigthsController 
    {

        [TestMethod]
        public void GetAllUserRigthsByUserId_ShouldReturnContainers()
        {
            string userId = Guid.NewGuid().ToString();

            var dbContext = new Mock<IAppDbContext>();

            dbContext.Setup(d => d.ExecuteStoredProcedure<int>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<int> list = new List<int>();
                   if (query.Contains("GetAspNetUserRightsByOfficeId"))
                   {
                       list.Add(1);
                       list.Add(4);
                       list.Add(57);
                   }
                   return list;
               });

            dbContext.Setup(d => d.ExecuteStoredProcedure<UserRightOfficeDTO>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) => 
               {
                   List<UserRightOfficeDTO> list = new List<UserRightOfficeDTO>();
               if (query.Contains("GetAspNetUSerRights"))
               {
                       list.Add(new UserRightOfficeDTO
                       {
                           OfficeId = 1,
                           ActionId = 1,
                           BoxId = 1,
                           ControllerId = 1,
                           ActionLongName = "1",
                           ActionName = "1",
                           BoxLongName = "1",
                           BoxName = "1",
                           ControllerLongName = "1",
                           ControllerName = "1",
                           OfficeShortName = "1",
                           UserId = userId,
                           Value = true
                       });



                   }
                   return list;
               });


            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            var controller = new UserRightsController(factory.Object);

            var result = controller.GetUserRightsByUser(userId) 
                as OkNegotiatedContentResult<List<UserRightTreeView>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Count());
        }

        [TestMethod]
        public void GetOneContainer_ShouldReturnContainer()
        {
            string userId = Guid.NewGuid().ToString();

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(d => d.ExecuteStoredProcedure<UserRightOfficeDTO>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<UserRightOfficeDTO> list = new List<UserRightOfficeDTO>();
                   if (query.Contains("GetAspNetUSerRights"))
                   {
                       list.Add(new UserRightOfficeDTO
                       {
                           OfficeId = 1,
                           ActionId = 1,
                           BoxId = 1,
                           ControllerId = 1,
                           ActionLongName = "1",
                           ActionName = "1",
                           BoxLongName = "1",
                           BoxName = "1",
                           ControllerLongName = "1",
                           ControllerName = "1",
                           OfficeShortName = "1",
                           UserId = userId,
                           Value = true
                       });
                   }
                   return list;
               });

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            var controller = new UserRightsController(factory.Object);
            var result = controller.GetUserRightsByUserAndOffice(userId, 1)
                as OkNegotiatedContentResult<UserRightTreeView>;
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Content.OfficeId);
        }

        [TestMethod]
        public void PutRights_ShouldPut()
        {
            string userId = Guid.NewGuid().ToString();

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(d => d.ExecuteStoredProcedure<UserRightOfficeDTO>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<UserRightOfficeDTO> list = new List<UserRightOfficeDTO>();
                   if (query.Contains("UpdateAspNetUserRights"))
                   {
                       list.Add(new UserRightOfficeDTO
                       {
                           OfficeId = 1,
                           ActionId = 1,
                           BoxId = 1,
                           ControllerId = 1,
                           ActionLongName = "1",
                           ActionName = "1",
                           BoxLongName = "1",
                           BoxName = "1",
                           ControllerLongName = "1",
                           ControllerName = "1",
                           OfficeShortName = "1",
                           UserId = userId,
                           Value = true
                       });
                   }
                   return list;
               });
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new UserRightsController(factory.Object);
            var result = controller.PutUserRights(userId, 1, new UserRightTreeView()) as OkNegotiatedContentResult<UserRightTreeView>;
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Content.OfficeId);
        }
    }
}
