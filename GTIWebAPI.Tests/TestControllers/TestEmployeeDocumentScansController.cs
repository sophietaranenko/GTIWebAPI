using GTIWebAPI.Controllers;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Models.Service;
using GTIWebAPI.Tests.TestContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using System.Web;

namespace GTIWebAPI.Tests.TestControllers
{
    [TestClass]
    public class TestEmployeeDocumentScansController
    {
        //i don't know how to test httprequest files without any mocking framework 
        //to do later!

        [TestMethod]
        public void PostScan_ShouldReturnNewScan_OrFewScans_WithDifferentId()
        {
            var docsTestData = new List<EmployeeDocumentScan>()
            {
                new EmployeeDocumentScan { Id = 1, ScanName = "\\PostedFiles\\10000028.jpeg", TableId = 1, ScanTableName = "P" },
                new EmployeeDocumentScan { Id = 2, ScanName = "\\PostedFiles\\10000029.jpeg", TableId = 1, ScanTableName = "P"  },
                new EmployeeDocumentScan { Id = 3, ScanName = "\\PostedFiles\\10000030.jpeg", TableId = 1, ScanTableName = "P", Deleted = true }
            };
            var docs = MockHelper.MockDbSet(docsTestData);
            docs.Setup(d => d.Add(It.IsAny<EmployeeDocumentScan>())).Returns<EmployeeDocumentScan>((doc) =>
            {
                docsTestData.Add(doc);
                docs = MockHelper.MockDbSet(docsTestData);
                return doc;
            });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeDocumentScans).Returns(docs.Object);
            dbContext.Setup(d => d.Set<EmployeeDocumentScan>()).Returns(docs.Object);
            dbContext.Setup(d => d.ExecuteStoredProcedure<int>(It.IsAny<string>(), It.IsAny<object[]>()))
                .Returns<string, object[]>((query, parameters) =>
                {
                    List<int> list = new List<int>();
                    if (query.Contains("NewTableId"))
                    {
                        int i = docsTestData.Max(d => d.Id) + 1;
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
            var request = new Mock<IRequest>();
            request.Setup(d => d.Collection()).Returns(new List<string> { "1.jpg", "2.jpg", "3.jpg" });
            request.Setup(d => d.AppPath()).Returns("C:/TestDirectory");
            request.Setup(d => d.GetPath()).Returns("C:/TestDirectory/PosteFiles");
            request.Setup(d => d.SaveFile(It.IsAny<string>())).Returns<string>((d) => { return "C:/TestDirectory/PostedFiles/some_new_tested_posted_file.ext"; });
            var controller = new EmployeeDocumentScansController(factory.Object, request.Object);
            var result = controller.UploadEmployeeDocumentScan("DocumentTableName", 1) as CreatedAtRouteNegotiatedContentResult<List<EmployeeDocumentScan>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Count);
            Assert.AreEqual(3, result.Content.Select(d => d.Id).Distinct().Count());
            Assert.AreEqual("DocumentTableName", result.Content.FirstOrDefault().ScanTableName);
            Assert.AreEqual(1, result.Content.Select(d => d.TableId).Distinct().Count());
            Assert.AreEqual(1, result.Content.FirstOrDefault().TableId);
        }

        [TestMethod]
        public void GetScansByDocument_ShouldReturnScansByDocument()
        {
            var docsTestData = new List<EmployeeDocumentScan>()
            {
                new EmployeeDocumentScan { Id = 1, ScanName = "\\PostedFiles\\10000028.jpeg", TableId = 1, ScanTableName = "P" },
                new EmployeeDocumentScan { Id = 2, ScanName = "\\PostedFiles\\10000029.jpeg", TableId = 1, ScanTableName = "P"  },
                new EmployeeDocumentScan { Id = 3, ScanName = "\\PostedFiles\\10000030.jpeg", TableId = 1, ScanTableName = "P", Deleted = true },
                new EmployeeDocumentScan { Id = 2, ScanName = "\\PostedFiles\\10000029.jpeg", TableId = 2, ScanTableName = "P"  },
                new EmployeeDocumentScan { Id = 2, ScanName = "\\PostedFiles\\10000029.jpeg", TableId = 2, ScanTableName = "DD"  },
                new EmployeeDocumentScan { Id = 2, ScanName = "\\PostedFiles\\10000029.jpeg", TableId = 1, ScanTableName = "DD"  },
            };
            var dosc = MockHelper.MockDbSet(docsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeDocumentScans).Returns(dosc.Object);
            dbContext.Setup(d => d.Set<EmployeeDocumentScan>()).Returns(dosc.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeDocumentScansController(factory.Object);
            var result = controller.GetEmployeeDocumentScanByDocumentId("P", 1) as OkNegotiatedContentResult<IEnumerable<EmployeeDocumentScan>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Content.Count());
            Assert.AreEqual(1, result.Content.Select(d => d.ScanTableName).Distinct().Count());
            Assert.AreEqual("P", result.Content.FirstOrDefault().ScanTableName);
            Assert.AreEqual(1, result.Content.Select(d => d.TableId).Distinct().Count());
            Assert.AreEqual(1, result.Content.FirstOrDefault().TableId);
        }

        [TestMethod]
        public void GetScansByEmployeeId_ShoulCallStoredProcedure()
        {
            var docsTestData = new List<EmployeeDocumentScan>()
            {
                new EmployeeDocumentScan { Id = 1, ScanName = "\\PostedFiles\\10000028.jpeg", TableId = 1, ScanTableName = "P" },
                new EmployeeDocumentScan { Id = 2, ScanName = "\\PostedFiles\\10000029.jpeg", TableId = 1, ScanTableName = "P"  },
                new EmployeeDocumentScan { Id = 3, ScanName = "\\PostedFiles\\10000030.jpeg", TableId = 1, ScanTableName = "P", Deleted = true },
                new EmployeeDocumentScan { Id = 2, ScanName = "\\PostedFiles\\10000029.jpeg", TableId = 2, ScanTableName = "P"  },
                new EmployeeDocumentScan { Id = 2, ScanName = "\\PostedFiles\\10000029.jpeg", TableId = 2, ScanTableName = "DD"  },
                new EmployeeDocumentScan { Id = 2, ScanName = "\\PostedFiles\\10000029.jpeg", TableId = 1, ScanTableName = "DD"  },
            };
            var dosc = MockHelper.MockDbSet(docsTestData);
            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeDocumentScans).Returns(dosc.Object);
            dbContext.Setup(d => d.Set<EmployeeDocumentScan>()).Returns(dosc.Object);
            dbContext.Setup(d => d.ExecuteStoredProcedure<EmployeeDocumentScan>(It.IsAny<string>(), It.IsAny<object[]>()))
                .Returns<string, object[]>((query, parameters) =>
                {
                    List<EmployeeDocumentScan> list = new List<EmployeeDocumentScan>();
                    if (query.Contains("EmployeeDocumentScanValid"))
                    {
                        list = new List<EmployeeDocumentScan>()
                        {
                            new EmployeeDocumentScan { Id = 1, ScanName = "\\PostedFiles\\10000028.jpeg", TableId = 1, ScanTableName = "P" },
                            new EmployeeDocumentScan { Id = 2, ScanName = "\\PostedFiles\\10000029.jpeg", TableId = 1, ScanTableName = "P"  },
                            new EmployeeDocumentScan { Id = 3, ScanName = "\\PostedFiles\\10000030.jpeg", TableId = 1, ScanTableName = "P", Deleted = true },
                            new EmployeeDocumentScan { Id = 2, ScanName = "\\PostedFiles\\10000029.jpeg", TableId = 2, ScanTableName = "P"  },
                            new EmployeeDocumentScan { Id = 2, ScanName = "\\PostedFiles\\10000029.jpeg", TableId = 2, ScanTableName = "DD"  },
                            new EmployeeDocumentScan { Id = 2, ScanName = "\\PostedFiles\\10000029.jpeg", TableId = 1, ScanTableName = "DD"  },
                        };
                    }
                    else
                    {
                        list.Add(new EmployeeDocumentScan() { Id = 33, ScanTableName = "Smth", TableId = 1, ScanName = "sss" });
                    }
                    return list;
                });

            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeDocumentScansController(factory.Object);
            var result = controller.GetEmployeeDocumentScanByEmployeeId(11) as OkNegotiatedContentResult<IEnumerable<EmployeeDocumentScan>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.Content.Count());
        }

        [TestMethod]
        public void GetEmployeeDocumentScan_ShouldReturnObjectWithSameId()
        {
            var docsTestData = new List<EmployeeDocumentScan>()
            {
                new EmployeeDocumentScan { Id = 1, ScanName = "\\PostedFiles\\10000028.jpeg", TableId = 1, ScanTableName = "P" },
                new EmployeeDocumentScan { Id = 2, ScanName = "\\PostedFiles\\10000029.jpeg", TableId = 1, ScanTableName = "P" },
                new EmployeeDocumentScan { Id = 3, ScanName = "\\PostedFiles\\10000030.jpeg", TableId = 1, ScanTableName = "P", Deleted = true },
                new EmployeeDocumentScan { Id = 2, ScanName = "\\PostedFiles\\10000029.jpeg", TableId = 2, ScanTableName = "P" },
                new EmployeeDocumentScan { Id = 2, ScanName = "\\PostedFiles\\10000029.jpeg", TableId = 2, ScanTableName = "DD" },
                new EmployeeDocumentScan { Id = 2, ScanName = "\\PostedFiles\\10000029.jpeg", TableId = 1, ScanTableName = "DD" },
            };
            var dosc = MockHelper.MockDbSet(docsTestData);
            dosc.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return dosc.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeDocumentScans).Returns(dosc.Object);
            dbContext.Setup(d => d.Set<EmployeeDocumentScan>()).Returns(dosc.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeDocumentScansController(factory.Object);
            var result = controller.GetEmployeeDocumentScan(1) as OkNegotiatedContentResult<EmployeeDocumentScan>;
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Content.Id);
            Assert.AreEqual("\\PostedFiles\\10000028.jpeg", result.Content.ScanName);
            Assert.AreEqual(1, result.Content.TableId);
            Assert.AreEqual("P", result.Content.ScanTableName);
        }

        [TestMethod]
        public void DeleteEmployeeDocumentScan_ShouldNotDeleteButMarkAsDeleted()
        {
            var docsTestData = new List<EmployeeDocumentScan>()
            {
                new EmployeeDocumentScan { Id = 1, ScanName = "\\PostedFiles\\10000028.jpeg", TableId = 1, ScanTableName = "P" },
                new EmployeeDocumentScan { Id = 2, ScanName = "\\PostedFiles\\10000029.jpeg", TableId = 1, ScanTableName = "P" },
                new EmployeeDocumentScan { Id = 3, ScanName = "\\PostedFiles\\10000030.jpeg", TableId = 1, ScanTableName = "P", Deleted = true },
                new EmployeeDocumentScan { Id = 2, ScanName = "\\PostedFiles\\10000029.jpeg", TableId = 2, ScanTableName = "P" },
                new EmployeeDocumentScan { Id = 2, ScanName = "\\PostedFiles\\10000029.jpeg", TableId = 2, ScanTableName = "DD" },
                new EmployeeDocumentScan { Id = 2, ScanName = "\\PostedFiles\\10000029.jpeg", TableId = 1, ScanTableName = "DD" },
            };
            var dosc = MockHelper.MockDbSet(docsTestData);
            dosc.Setup(d => d.Find(It.IsAny<object>())).Returns<object[]>((keyValues) => { return dosc.Object.SingleOrDefault(product => product.Id == (int)keyValues.Single()); });

            var dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(m => m.EmployeeDocumentScans).Returns(dosc.Object);
            dbContext.Setup(d => d.Set<EmployeeDocumentScan>()).Returns(dosc.Object);
            var factory = new Mock<IDbContextFactory>();
            factory.Setup(m => m.CreateDbContext()).Returns(dbContext.Object);
            var controller = new EmployeeDocumentScansController(factory.Object);
            var result = controller.DeleteEmployeeDocumentScan(1) as OkNegotiatedContentResult<EmployeeDocumentScan>;
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Content.Id);
            Assert.AreEqual(true, result.Content.Deleted);
        }

    }
}

