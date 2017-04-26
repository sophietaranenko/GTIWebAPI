using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
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
    public class TestOrganizationPropertiesController
    {
        [TestMethod]
        public void GetPropertysByOrganizationId_ShouldReturn()
        {
            var propertiesTestData = new List<OrganizationProperty>()
            {
                new OrganizationProperty { Id = 1, OrganizationId = 2 },
                new OrganizationProperty { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationProperty { Id = 3, OrganizationId = 3 }
            };
            var properties = MockHelper.MockDbSet(propertiesTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationProperties).Returns(properties.Object);
            dbContext.Setup(d => d.Set<OrganizationProperty>()).Returns(properties.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new OrganizationPropertiesController(factory.Object);
            var result = controller.GetOrganizationPropertyByOrganizationId(2) as OkNegotiatedContentResult<IEnumerable<OrganizationPropertyDTO>>;
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetPropertyById_ShouldReturn()
        {
            var propertiesTestData = new List<OrganizationProperty>()
            {
                new OrganizationProperty { Id = 1, OrganizationId = 2 },
                new OrganizationProperty { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationProperty { Id = 3, OrganizationId = 3 }
            };
            var properties = MockHelper.MockDbSet(propertiesTestData);
            properties.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return properties.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationProperties).Returns(properties.Object);
            dbContext.Setup(d => d.Set<OrganizationProperty>()).Returns(properties.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new OrganizationPropertiesController(factory.Object);
            var result = controller.GetOrganizationProperty(1) as OkNegotiatedContentResult<OrganizationPropertyDTO>;
            Assert.AreEqual(1, result.Content.Id);
            Assert.AreEqual(2, result.Content.OrganizationId);
        }

        [TestMethod]
        public void PutDocument_ShouldReturnOk()
        {
            var propertiesTestData = new List<OrganizationProperty>()
            {
                new OrganizationProperty { Id = 1, OrganizationId = 2 },
                new OrganizationProperty { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationProperty { Id = 3, OrganizationId = 3 }
            };
            var properties = MockHelper.MockDbSet(propertiesTestData);
            properties.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return properties.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });
            properties.Setup(d => d.Add(It.IsAny<OrganizationProperty>())).Returns<OrganizationProperty>((contact) =>
            {
                propertiesTestData.Add(contact);
                properties = MockHelper.MockDbSet(propertiesTestData);
                return contact;
            });


            var typesTestData = new List<OrganizationPropertyType>()
            {
                new OrganizationPropertyType { Id = 1, CountryId = 1 },
                new OrganizationPropertyType { Id = 2, CountryId = 1 },
                new OrganizationPropertyType { Id = 3, CountryId = 2 }
            };
            var types = MockHelper.MockDbSet(typesTestData);

            var orgsTestData = new List<Organization>()
            {
                new Organization { CountryId = 1 }
            };
            var orgs = MockHelper.MockDbSet(orgsTestData);



            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationProperties).Returns(properties.Object);
            dbContext.Setup(d => d.Set<OrganizationProperty>()).Returns(properties.Object);

            dbContext.Setup(m => m.Organizations).Returns(orgs.Object);
            dbContext.Setup(d => d.Set<Organization>()).Returns(orgs.Object);

            dbContext.Setup(m => m.OrganizationPropertyTypes).Returns(types.Object);
            dbContext.Setup(d => d.Set<OrganizationPropertyType>()).Returns(types.Object);

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            OrganizationProperty passport = new OrganizationProperty { Id = 3, OrganizationId = 3 };
            var controller = new OrganizationPropertiesController(factory.Object);
            var result = controller.PutOrganizationProperty(3, passport) as OkNegotiatedContentResult<OrganizationPropertyDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
        }

        [TestMethod]
        public void PostProperty_ShoulAddProperty()
        {
            var propertiesTestData = new List<OrganizationProperty>()
            {
                new OrganizationProperty { Id = 1, OrganizationId = 2 },
                new OrganizationProperty { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationProperty { Id = 3, OrganizationId = 3 }
            };
            var properties = MockHelper.MockDbSet(propertiesTestData);
            properties.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return properties.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });
            properties.Setup(d => d.Add(It.IsAny<OrganizationProperty>())).Returns<OrganizationProperty>((contact) =>
            {
                propertiesTestData.Add(contact);
                properties = MockHelper.MockDbSet(propertiesTestData);
                return contact;
            });


            var typesTestData = new List<OrganizationPropertyType>()
            {
                new OrganizationPropertyType { Id = 1, CountryId = 1 },
                new OrganizationPropertyType { Id = 2, CountryId = 1 },
                new OrganizationPropertyType { Id = 3, CountryId = 2 }
            };
            var types = MockHelper.MockDbSet(typesTestData);

            var orgsTestData = new List<Organization>()
            {
                new Organization { CountryId = 1 }
            };
            var orgs = MockHelper.MockDbSet(orgsTestData);



            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationProperties).Returns(properties.Object);
            dbContext.Setup(d => d.Set<OrganizationProperty>()).Returns(properties.Object);

            dbContext.Setup(m => m.Organizations).Returns(orgs.Object);
            dbContext.Setup(d => d.Set<Organization>()).Returns(orgs.Object);

            dbContext.Setup(m => m.OrganizationPropertyTypes).Returns(types.Object);
            dbContext.Setup(d => d.Set<OrganizationPropertyType>()).Returns(types.Object);


            dbContext.Setup(d => d.ExecuteStoredProcedure<int>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<int> list = new List<int>();
                   if (query.Contains("NewTableId"))
                   {
                       int i = properties.Object.Max(d => d.Id) + 1;
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
            OrganizationProperty passport = new OrganizationProperty { Id = 0, OrganizationId = 3 };
            var controller = new OrganizationPropertiesController(factory.Object);
            var result = controller.PostOrganizationProperty(passport) as CreatedAtRouteNegotiatedContentResult<OrganizationPropertyDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Content.Id);
            Assert.AreEqual(3, result.Content.OrganizationId);
        }

        [TestMethod]
        public void DeleteProperty_ShouldDeleteAndReturnOk()
        {
            var propertiesTestData = new List<OrganizationProperty>()
            {
                new OrganizationProperty { Id = 1, OrganizationId = 2 },
                new OrganizationProperty { Id = 2, Deleted = true, OrganizationId = 2 },
                new OrganizationProperty { Id = 3, OrganizationId = 3 }
            };
            var properties = MockHelper.MockDbSet(propertiesTestData);
            properties.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return properties.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.OrganizationProperties).Returns(properties.Object);
            dbContext.Setup(d => d.Set<OrganizationProperty>()).Returns(properties.Object);


            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            OrganizationProperty passport = new OrganizationProperty { Id = 3, OrganizationId = 3 };
            var controller = new OrganizationPropertiesController(factory.Object);
            var result = controller.DeleteOrganizationProperty(3) as OkNegotiatedContentResult<OrganizationPropertyDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
            Assert.AreEqual(3, result.Content.OrganizationId);
        }
    }
}
