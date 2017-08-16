using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Organizations;
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
    public class TestOrganizationAddressesController
    {

        [TestMethod]
        public void GetPassportsByOrganizationId_ShouldReturn()
        {
            var passportsTestData = new List<OrganizationAddress>()
            {
                new OrganizationAddress { Id = 1, OrganizationId = 2 },
                new OrganizationAddress { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationAddress { Id = 3, OrganizationId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationAddresses).Returns(passports.Object);
            dbContext.Setup(d => d.Set<OrganizationAddress>()).Returns(passports.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new OrganizationAddressesController(factory.Object);
            var result = controller.GetOrganizationAddressByOrganizationId(2) as OkNegotiatedContentResult<IEnumerable<OrganizationAddressDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetPassportById_ShouldReturn()
        {
            var passportsTestData = new List<OrganizationAddress>()
            {
                new OrganizationAddress { Id = 1, OrganizationId = 2 },
                new OrganizationAddress { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationAddress { Id = 3, OrganizationId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            passports.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return passports.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationAddresses).Returns(passports.Object);
            dbContext.Setup(d => d.Set<OrganizationAddress>()).Returns(passports.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new OrganizationAddressesController(factory.Object);
            var result = controller.GetOrganizationAddress(1) as OkNegotiatedContentResult<OrganizationAddressDTO>;
            Assert.AreEqual(1, result.Content.Id);
            Assert.AreEqual(2, result.Content.OrganizationId);
        }

        [TestMethod]
        public void PutDocument_ShouldReturnOk()
        {
            var passportsTestData = new List<OrganizationAddress>()
            {
                new OrganizationAddress { Id = 1, OrganizationId = 2 },
                new OrganizationAddress { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationAddress { Id = 3, OrganizationId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            passports.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return passports.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationAddresses).Returns(passports.Object);
            dbContext.Setup(d => d.Set<OrganizationAddress>()).Returns(passports.Object);

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            OrganizationAddress passport = new OrganizationAddress { Id = 3, OrganizationId = 3 };
            var controller = new OrganizationAddressesController(factory.Object);
            var result = controller.PutOrganizationAddress(3, passport) as OkNegotiatedContentResult<OrganizationAddressDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
        }

        [TestMethod]
        public void PostPassport_ShoulAddPassport()
        {
            var passportsTestData = new List<OrganizationAddress>()
            {
                new OrganizationAddress { Id = 1, OrganizationId = 2 },
                new OrganizationAddress { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationAddress { Id = 3, OrganizationId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            passports.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return passports.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });
            passports.Setup(d => d.Add(It.IsAny<OrganizationAddress>())).Returns<OrganizationAddress>((contact) =>
            {
                passportsTestData.Add(contact);
                passports = MockHelper.MockDbSet(passportsTestData);
                return contact;
            });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationAddresses).Returns(passports.Object);
            dbContext.Setup(d => d.Set<OrganizationAddress>()).Returns(passports.Object);

            dbContext.Setup(d => d.ExecuteStoredProcedure<int>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<int> list = new List<int>();
                   if (query.Contains("NewTableId"))
                   {
                       int i = passports.Object.Max(d => d.Id) + 1;
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
            OrganizationAddress passport = new OrganizationAddress { Id = 0, OrganizationId = 3, Address = new Address { Apartment = "3", BuildingNumber = 45 } };
            var controller = new OrganizationAddressesController(factory.Object);
            var result = controller.PostOrganizationAddress(passport.ToDTO()) as CreatedAtRouteNegotiatedContentResult<OrganizationAddressDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Content.Id);
            Assert.AreEqual(3, result.Content.OrganizationId);
        }

        [TestMethod]
        public void DeletePassport_ShouldDeleteAndReturnOk()
        {
            var passportsTestData = new List<OrganizationAddress>()
            {
                new OrganizationAddress { Id = 1, OrganizationId = 2 },
                new OrganizationAddress { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationAddress { Id = 3, OrganizationId = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            passports.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return passports.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationAddresses).Returns(passports.Object);
            dbContext.Setup(d => d.Set<OrganizationAddress>()).Returns(passports.Object);


            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            OrganizationAddress passport = new OrganizationAddress { Id = 3, OrganizationId = 3 };
            var controller = new OrganizationAddressesController(factory.Object);
            var result = controller.DeleteOrganizationAddress(3) as OkNegotiatedContentResult<OrganizationAddressDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
            Assert.AreEqual(3, result.Content.OrganizationId);
        }
    }
}
