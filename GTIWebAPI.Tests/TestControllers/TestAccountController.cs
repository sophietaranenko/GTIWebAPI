using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Account;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Security;
using GTIWebAPI.Models.Service;
using GTIWebAPI.Novell;
using GTIWebAPI.Tests.TestContext;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;



namespace GTIWebAPI.Tests.TestControllers
{
    [TestClass]
    public class TestAccountController
    {
        [TestMethod]
        public async Task GetUserInfo_ShouldReturnUserInfo()
        {
            var identityHelper = new Mock<IIdentityHelper>();
            identityHelper.Setup(d => d.FindUserById(It.IsAny<string>()))
                .Returns<string>((userId) =>
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        Id = userId,
                        Email = "SomeTestEmail@Test.Test",
                        TableId = 1,
                        TableName = "Employee",
                        UserName = "testuser"
                    };
                    return user;
                });
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(d => d.ExecuteStoredProcedure<bool>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<bool> list = new List<bool>();
                   if (query.Contains("IsEmployeeInformationFilled"))
                   {
                       list.Add(true);
                   }
                   return list;
               });

            var userImagesTestData = new List<UserImage>();
            var userImages = MockHelper.MockDbSet<UserImage>(userImagesTestData);
            dbContext.Setup(d => d.UserImages).Returns(userImages.Object);
            dbContext.Setup(d => d.Set<UserImage>()).Returns(userImages.Object);

            var dbFactory = new Mock<IDbContextFactory>();
            dbFactory.Setup(d => d.CreateDbContext()).Returns(dbContext.Object);

            var appDbContext = new Mock<IApplicationDbContext>();
            appDbContext.Setup(d => d.GetFullUserName(It.IsAny<string>())).Returns("Full Test User Name");

            var controller = new AccountController(dbFactory.Object, appDbContext.Object, identityHelper.Object);
            var result = controller.GetUserInfo() as UserInfoViewModel;
            Assert.IsNotNull(result);
            Assert.AreEqual("Full Test User Name", result.FullUserName);
            Assert.AreEqual("SomeTestEmail@Test.Test", result.Email);
            Assert.AreEqual("testuser", result.UserName);
            Assert.AreEqual(1, result.TableId);
        }

        [TestMethod]
        public void UploadPhoto_ShouldUpload()
        {
            var identityHelper = new Mock<IIdentityHelper>();
            identityHelper.Setup(d => d.FindUserById(It.IsAny<string>()))
                .Returns<string>((userId) =>
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        Id = userId,
                        Email = "SomeTestEmail@Test.Test",
                        TableId = 1,
                        TableName = "Employee",
                        UserName = "testuser"
                    };
                    return user;
                });
            identityHelper.Setup(d => d.GetUserId(It.IsAny<IPrincipal>())).Returns(Guid.NewGuid().ToString());

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(d => d.ExecuteStoredProcedure<bool>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<bool> list = new List<bool>();
                   if (query.Contains("IsEmployeeInformationFilled"))
                   {
                       list.Add(true);
                   }
                   return list;
               });

            var userImagesTestData = new List<UserImage>();
            var userImages = MockHelper.MockDbSet<UserImage>(userImagesTestData);
            dbContext.Setup(d => d.UserImages).Returns(userImages.Object);
            dbContext.Setup(d => d.Set<UserImage>()).Returns(userImages.Object);

            var dbFactory = new Mock<IDbContextFactory>();
            dbFactory.Setup(d => d.CreateDbContext()).Returns(dbContext.Object);


            var request = new Mock<IRequest>();
            request.Setup(d => d.FileCount()).Returns(1);
            request.Setup(d => d.Collection()).Returns(new List<string> { "one.jpg" });

            var controller = new AccountController(dbFactory.Object, identityHelper.Object, request.Object);

            var result = controller.UploadNewProfilePicture() as OkNegotiatedContentResult<UserImage>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content.UploadDate);
        }

        [TestMethod]
        public void SetAsProfilePicture_ShouldWork()
        {
            Guid pictureId = Guid.NewGuid();

            var identityHelper = new Mock<IIdentityHelper>();
            identityHelper.Setup(d => d.FindUserById(It.IsAny<string>()))
                .Returns<string>((userId) =>
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        Id = userId,
                        Email = "SomeTestEmail@Test.Test",
                        TableId = 1,
                        TableName = "Employee",
                        UserName = "testuser"
                    };
                    return user;
                });
            identityHelper.Setup(d => d.GetUserId(It.IsAny<IPrincipal>())).Returns(Guid.NewGuid().ToString());


            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(d => d.ExecuteStoredProcedure<bool>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<bool> list = new List<bool>();
                   if (query.Contains("IsEmployeeInformationFilled"))
                   {
                       list.Add(true);
                   }
                   return list;
               });

            var userImagesTestData = new List<UserImage>
            {
                new UserImage { Id = pictureId, IsProfilePicture = false },
                new UserImage { Id = Guid.NewGuid(), IsProfilePicture = false },
                new UserImage { Id = Guid.NewGuid(), IsProfilePicture = true }
            };
            var userImages = MockHelper.MockDbSet<UserImage>(userImagesTestData);
            dbContext.Setup(d => d.UserImages).Returns(userImages.Object);
            dbContext.Setup(d => d.Set<UserImage>()).Returns(userImages.Object);

            var dbFactory = new Mock<IDbContextFactory>();
            dbFactory.Setup(d => d.CreateDbContext()).Returns(dbContext.Object);


            var request = new Mock<IRequest>();
            request.Setup(d => d.FileCount()).Returns(1);
            request.Setup(d => d.Collection()).Returns(new List<string> { "one.jpg" });

            var controller = new AccountController(dbFactory.Object, identityHelper.Object, request.Object);

            var result = controller.SetAsProfilePicture(pictureId) as OkNegotiatedContentResult<UserImage>;
            Assert.IsNotNull(result);
            Assert.AreEqual(pictureId, result.Content.Id);
        }

        [TestMethod]
        public void GetUserRigths_ShouldReturnRights()
        {
            string UserId = Guid.NewGuid().ToString();

            var identityHelper = new Mock<IIdentityHelper>();
            identityHelper.Setup(d => d.FindUserById(It.IsAny<string>()))
                .Returns<string>((userId) =>
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        Id = userId,
                        Email = "SomeTestEmail@Test.Test",
                        TableId = 1,
                        TableName = "Employee",
                        UserName = "testuser",
                        UserRights = new List<UserRight>
                        {
                            new UserRight
                            {
                                ActionId = 1,
                                Id = Guid.NewGuid(),
                                AspNetUserId = UserId,
                                OfficeId = 1,
                                OfficeSecurity = new OfficeSecurity
                                    {
                                        Id = 1
                                },
                                Action = new Models.Security.Action
                                {
                                    ControllerId = 1,
                                    Id = 1,
                                    Controller = new Controller
                                    {
                                        ControllerBox = new ControllerBox
                                        {
                                            Id = 1,
                                            Name = "1"
                                        },
                                        BoxId = 1,
                                        Id = 1,
                                        Name = "1"
                                    },
                                    Name = "1"
                                }
                            },
                            new UserRight
                            {
                                ActionId = 2,
                                Id = Guid.NewGuid(),
                                AspNetUserId = UserId,
                                OfficeId = 3,
                                OfficeSecurity = new OfficeSecurity
                                    {
                                        Id = 3
                                },
                                Action = new Models.Security.Action
                                {
                                    ControllerId = 1,
                                    Id = 2,
                                    Controller = new Controller
                                    {
                                        ControllerBox = new ControllerBox
                                        {
                                            Id = 1,
                                            Name = "1"
                                        },
                                        BoxId = 1,
                                        Id = 1,
                                        Name = "1"
                                    },
                                    Name = "2"
                                }
                            },
                            new UserRight
                            {
                                ActionId = 3,
                                Id = Guid.NewGuid(),
                                AspNetUserId = UserId,
                                OfficeId = 2,
                                OfficeSecurity = new OfficeSecurity
                                    {
                                        Id = 2
                                },
                                Action = new Models.Security.Action
                                {
                                    ControllerId = 1,
                                    Id = 3,
                                    Controller = new Controller
                                    {
                                        ControllerBox = new ControllerBox
                                        {
                                            Id = 1,
                                            Name = "1"
                                        },
                                        BoxId = 1,
                                        Id = 1,
                                        Name = "1"
                                    },
                                    Name = "3"
                                }
                            }
                        }
                    };
                    return user;
                });
            identityHelper.Setup(d => d.GetUserId(It.IsAny<IPrincipal>())).Returns(UserId);

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(d => d.ExecuteStoredProcedure<bool>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<bool> list = new List<bool>();
                   if (query.Contains("IsEmployeeInformationFilled"))
                   {
                       list.Add(true);
                   }
                   return list;
               });

            var dbFactory = new Mock<IDbContextFactory>();
            dbFactory.Setup(d => d.CreateDbContext()).Returns(dbContext.Object);


            var request = new Mock<IRequest>();
            request.Setup(d => d.FileCount()).Returns(1);
            request.Setup(d => d.Collection()).Returns(new List<string> { "one.jpg" });

            var controller = new AccountController(dbFactory.Object, identityHelper.Object, request.Object);
            var result = controller.GetUserRights() as IEnumerable<UserRightDTO>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task SimpleRegisted_ShouldRegisterAndReturnOK()
        {
            //это было самым трудным 

            int organizationContactPersonId = 33;

            var identityHelper = new Mock<IIdentityHelper>();
            identityHelper.Setup(d => d.FindUserById(It.IsAny<string>()))
                .Returns<string>((userId) =>
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        Id = userId,
                        Email = "SomeTestEmail@Test.Test",
                        TableId = 1,
                        TableName = "Employee",
                        UserName = "testuser"
                    };
                    return user;
                });
            identityHelper.Setup(d => d.GetUserId(It.IsAny<IPrincipal>())).Returns(Guid.NewGuid().ToString());

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(d => d.ExecuteStoredProcedure<bool>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<bool> list = new List<bool>();
                   if (query.Contains("IsEmployeeInformationFilled"))
                   {
                       list.Add(true);
                   }
                   return list;
               });

            var personsData = new List<OrganizationContactPersonView>
            {
                new OrganizationContactPersonView { Id = 33, FirstName = "Тест", SecondName = "Тестович", LastName = "Тестовый", Email = "sometest@test.test" },
                new OrganizationContactPersonView { Id = 99, Email = "ddd", FirstName = "ss", SecondName = "ss", LastName = "ssss"}
            };
            var persons = MockHelper.MockDbSet<OrganizationContactPersonView>(personsData);
            dbContext.Setup(d => d.OrganizationContactPersonViews).Returns(persons.Object);
            dbContext.Setup(d => d.Set<OrganizationContactPersonView>()).Returns(persons.Object);

            var dbFactory = new Mock<IDbContextFactory>();
            dbFactory.Setup(d => d.CreateDbContext()).Returns(dbContext.Object);

            var request = new Mock<IRequest>();
            request.Setup(d => d.FileCount()).Returns(1);
            request.Setup(d => d.Collection()).Returns(new List<string> { "one.jpg" });
            request.Setup(d => d.AppPath()).Returns("C://Projects//GTIWebAPI//GTIWebAPI//");

            var novell = new Mock<INovellManager>();
            novell.Setup(d => d.GenerateLogin(It.IsAny<string>())).Returns<string>((login) => { return login; });
            novell.Setup(d => d.CreateOrganization(It.IsAny<INovellOrganizationContactPerson>())).Returns(true);

            var context = new Mock<IApplicationDbContext>();
            context.Setup(d => d.CreateOrganization(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            context.Setup(d => d.GrantRightsToOrganization(It.IsAny<string>())).Returns(true);

            var mockUS = new Mock<IUserStore<ApplicationUser>>();

            var mockUM = new Mock<ApplicationUserManager>(mockUS.Object);

                mockUM.Setup(d => d.AddToRoleAsync(It.IsAny<string>(), It.IsAny<string>())).Returns<string, string>
                ((a, b) =>
                {
                    return Task.FromResult<IdentityResult>(IdentityResult.Success);
                });
                mockUM.Setup(d => d.CreateAsync(It.IsAny<ApplicationUser>())).Returns<ApplicationUser>
                ((user) =>
                {
                    return Task.FromResult<IdentityResult>(IdentityResult.Success);
                });
                mockUM.Setup(d => d.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).Returns<ApplicationUser, string>
                ((user, password) =>
                {
                    return Task.FromResult<IdentityResult>(IdentityResult.Success);
                });
                mockUM.Setup(d => d.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(default(object)));
                
            var controller = new AccountController(dbFactory.Object, context.Object, request.Object, identityHelper.Object, novell.Object, mockUM.Object);
            var result = await controller.SimpleRegisterOrganizationContactPerson(33) as OkResult;
            Assert.IsNotNull(result);
            
        }
    }
}
