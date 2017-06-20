using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Employees;
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
    public class TestEmployeesController
    {
        //[TestMethod]
        //public void GetAllEmployees_ShouldReturnNotDeleted()
        //{
        //    var passportsTestData = new List<Employee>()
        //    {
        //        new Employee { Id = 1},
        //        new Employee { Id = 2},
        //        new Employee { Id = 3}
        //    };
        //    var passports = MockHelper.MockDbSet(passportsTestData);
        //    var contactsTestData = new List<EmployeeContact>()
        //    {
        //        new EmployeeContact { Id = 1, ContactTypeId = 1}
        //    };
        //    var contacts = MockHelper.MockDbSet(contactsTestData);

        //    var dbContext = new Mock<IAppDbContext>();

        //    dbContext.Setup(d => d.EmployeeContacts).Returns(contacts.Object);
        //    dbContext.Setup(d => d.Set<EmployeeContact>()).Returns(contacts.Object);

        //    dbContext.Setup(m => m.Employees).Returns(passports.Object);
        //    dbContext.Setup(d => d.Set<Employee>()).Returns(passports.Object);

        //    dbContext.Setup(d => d.ExecuteStoredProcedure<EmployeeView>(It.IsAny<string>(), It.IsAny<object[]>()))
        //       .Returns<string, object[]>((query, parameters) =>
        //       {
        //           List<EmployeeView> list = new List<EmployeeView>();
        //           if (query.Contains("EmployeeByOfficeIds"))
        //           {
        //               list.Add( new EmployeeView { Id = 1, Email = "ss"});
        //               list.Add(new EmployeeView { Id = 2, Email = "ss" });
        //               list.Add(new EmployeeView { Id = 3, Email = "ss" });
        //               list.Add(new EmployeeView { Id = 4, Email = "ss" });
        //               list.Add(new EmployeeView { Id = 5, Email = "ss" });
        //           }
        //           else
        //           {
        //               list.Add(new EmployeeView { Id = 1, Email = "ss" });
        //           }
        //           return list;
        //       });

        //    var factory = new Mock<IDbContextFactory>();
        //    factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
        //    var controller = new EmployeesController(factory.Object);
        //    var result = controller.GetEmployeeAll("1,2") as OkNegotiatedContentResult<Task<IEnumerable<EmployeeViewDTO>>>;
        //    Assert.AreEqual(5, result.Content.Count());
        //}

        [TestMethod]
        public void GetEmployeeView_ShouldReturn()
        {
            var employeesTestData = new List<Employee>()
            {
                new Employee { Id = 1},
                new Employee { Id = 2, Deleted = true },
                new Employee { Id = 3}
            };
            var employees = MockHelper.MockDbSet(employeesTestData);

            //жопа

            var contactsTestData = new List<EmployeeContact>()
            {
                new EmployeeContact() { Id = 1}
            };
            var contacts = MockHelper.MockDbSet(contactsTestData);

            var carsTestData = new List<EmployeeCar>()
            {
                new EmployeeCar() { Id = 1 }
            };
            var cars = MockHelper.MockDbSet(carsTestData);

            var licensesTestData = new List<EmployeeDrivingLicense>()
            {
                new EmployeeDrivingLicense() { Id = 1 }
            };
            var licenses = MockHelper.MockDbSet(licensesTestData);


            var officesTestData = new List<EmployeeOffice>()
            {
                new EmployeeOffice { Id = 1 }
            };
            var offices = MockHelper.MockDbSet(officesTestData);

            var passportsTestData = new List<EmployeePassport>()
            {
                new EmployeePassport { Id = 1 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);


            var cardsTestData = new List<EmployeeMilitaryCard>()
            {
                new EmployeeMilitaryCard {Id = 1 }
            };
            var cards = MockHelper.MockDbSet(cardsTestData);


            var languagesTestData = new List<EmployeeLanguage>()
            {
                new EmployeeLanguage { Id = 1 }
            };
            var language = MockHelper.MockDbSet(languagesTestData);

            var intPassportsTestData = new List<EmployeeInternationalPassport>()
            {
                new EmployeeInternationalPassport {Id = 1 }
            };
            var intPassports = MockHelper.MockDbSet(intPassportsTestData);

            var fDocsTestData = new List<EmployeeFoundationDocument>()
            {
                new EmployeeFoundationDocument { Id = 1 }
            };
            var fDocs = MockHelper.MockDbSet(fDocsTestData);

            var eduTestData = new List<EmployeeEducation>()
            {
                new EmployeeEducation { Id = 1}
            };
            var edu = MockHelper.MockDbSet(eduTestData);

            var gunTestData = new List<EmployeeGun>()
            {
                new EmployeeGun { Id = 1}
            };
            var gun = MockHelper.MockDbSet(gunTestData);

            var dbContext = new Mock<IAppDbContext>();


            dbContext.Setup(m => m.Employees).Returns(employees.Object);
            dbContext.Setup(d => d.Set<Employee>()).Returns(employees.Object);

            dbContext.Setup(m => m.EmployeeContacts).Returns(contacts.Object);
            dbContext.Setup(d => d.Set<EmployeeContact>()).Returns(contacts.Object);

            dbContext.Setup(m => m.EmployeeCars).Returns(cars.Object);
            dbContext.Setup(d => d.Set<EmployeeCar>()).Returns(cars.Object);

            dbContext.Setup(m => m.EmployeeDrivingLicenses).Returns(licenses.Object);
            dbContext.Setup(d => d.Set<EmployeeDrivingLicense>()).Returns(licenses.Object);

            dbContext.Setup(m => m.Employees).Returns(employees.Object);
            dbContext.Setup(d => d.Set<Employee>()).Returns(employees.Object);

            dbContext.Setup(m => m.EmployeeOffices).Returns(offices.Object);
            dbContext.Setup(d => d.Set<EmployeeOffice>()).Returns(offices.Object);

            dbContext.Setup(m => m.EmployeePassports).Returns(passports.Object);
            dbContext.Setup(d => d.Set<EmployeePassport>()).Returns(passports.Object);

            dbContext.Setup(m => m.EmployeeMilitaryCards).Returns(cards.Object);
            dbContext.Setup(d => d.Set<EmployeeMilitaryCard>()).Returns(cards.Object);

            dbContext.Setup(m => m.EmployeeLanguages).Returns(language.Object);
            dbContext.Setup(d => d.Set<EmployeeLanguage>()).Returns(language.Object);

            dbContext.Setup(m => m.EmployeeInternationalPassports).Returns(intPassports.Object);
            dbContext.Setup(d => d.Set<EmployeeInternationalPassport>()).Returns(intPassports.Object);

            dbContext.Setup(m => m.EmployeeFoundationDocuments).Returns(fDocs.Object);
            dbContext.Setup(d => d.Set<EmployeeFoundationDocument>()).Returns(fDocs.Object);

            dbContext.Setup(m => m.EmployeeEducations).Returns(edu.Object);
            dbContext.Setup(d => d.Set<EmployeeEducation>()).Returns(edu.Object);

            dbContext.Setup(m => m.EmployeeGuns).Returns(gun.Object);
            dbContext.Setup(d => d.Set<EmployeeGun>()).Returns(gun.Object);

            dbContext.Setup(d => d.ExecuteStoredProcedure<string>(It.IsAny<string>(), It.IsAny<object[]>()))
               .Returns<string, object[]>((query, parameters) =>
               {
                   List<string> stringList = new List<string>();
                   if (query.Contains("GetProfilePicturePathByEmployeeId") || query.Contains("GetFullAspNetUserNameByEmployeeId"))
                   {
                       stringList.Add("ss");
                   }
                   else
                   {
                       stringList.Add("sss");
                   }
                   return stringList;
               });


            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeesController(factory.Object);
            var result = controller.GetEmployeeView(2) as OkNegotiatedContentResult<EmployeeDTO>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetEmployeeEdit_ShouldReturn()
        {
            var passportsTestData = new List<Employee>()
            {
                new Employee { Id = 1},
                new Employee { Id = 2, Deleted = true},
                new Employee { Id = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            passports.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return passports.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.Employees).Returns(passports.Object);
            dbContext.Setup(d => d.Set<Employee>()).Returns(passports.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeesController(factory.Object);
            var result = controller.GetEmployeeEdit(1) as OkNegotiatedContentResult<EmployeeEditDTO>;
            Assert.AreEqual(1, result.Content.Id);
        }

        [TestMethod]
        public void PutEmployee_ShouldReturnOk()
        {
            var passportsTestData = new List<Employee>()
            {
                new Employee { Id = 1},
                new Employee { Id = 2, Deleted = true},
                new Employee { Id = 3}
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            passports.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return passports.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.Employees).Returns(passports.Object);
            dbContext.Setup(d => d.Set<Employee>()).Returns(passports.Object);

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            Employee passport = new Employee { Id = 3};
            var controller = new EmployeesController(factory.Object);
            var result = controller.PutEmployee(3, passport) as OkNegotiatedContentResult<EmployeeEditDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
        }

        [TestMethod]
        public void PostEmployee_ShoulAddEmployee()
        {
            var passportsTestData = new List<Employee>()
            {
                new Employee { Id = 1},
                new Employee { Id = 2, Deleted = true },
                new Employee { Id = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            passports.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return passports.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });
            passports.Setup(d => d.Add(It.IsAny<Employee>())).Returns<Employee>((contact) =>
            {
                passportsTestData.Add(contact);
                passports = MockHelper.MockDbSet(passportsTestData);
                return contact;
            });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.Employees).Returns(passports.Object);
            dbContext.Setup(d => d.Set<Employee>()).Returns(passports.Object);

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
            Employee passport = new Employee { Id = 0, Address = new Address { Apartment = "3", BuildingNumber = 45 } };
            var controller = new EmployeesController(factory.Object);
            var result = controller.PostEmployee(passport) as CreatedAtRouteNegotiatedContentResult<EmployeeEditDTO>;
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Content.Id);
        }

        [TestMethod]
        public void DeleteEmployee_ShouldDeleteAndReturnOk()
        {
            var passportsTestData = new List<Employee>()
            {
                new Employee { Id = 1 },
                new Employee { Id = 2 },
                new Employee { Id = 3 }
            };
            var passports = MockHelper.MockDbSet(passportsTestData);
            passports.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return passports.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.Employees).Returns(passports.Object);
            dbContext.Setup(d => d.Set<Employee>()).Returns(passports.Object);


            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);

            var controller = new EmployeesController(factory.Object);
            var result = controller.DeleteEmployee(3) as OkNegotiatedContentResult<EmployeeEditDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
        }
    }
}
