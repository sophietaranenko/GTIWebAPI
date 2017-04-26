using GTIWebAPI.Exceptions;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Repository;
using GTIWebAPI.Models.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Description;

namespace GTIWebAPI.Controllers
{

    /// <summary>
    /// Controllers for storing and showing images of documents
    /// </summary>
    [RoutePrefix("api/EmployeeDocumentScans")]
    public class EmployeeDocumentScansController : ApiController
    {
        private IDbContextFactory factory;
        private IRequest request;

        //Это особенный контроллер, тут есть использование HttpPostedFiles и других эелементов библиотеки System.Web
        //Эти классы не могут работать в тестах (потому что нельзя делать перекрестные ссылки на проекты) 
        //Чтобы сделать это более тестируемым, HttpContext.Current.Request был обернут в IRequest
        //класс Request реализует IRequest
        //внутри Request скрыты невидимые в тестах классы
         
        public EmployeeDocumentScansController()
        {
            factory = new DbContextFactory();
            request = new Request();
        }

        public EmployeeDocumentScansController(IDbContextFactory factory)
        {
            this.factory = factory;
            request = new Request();
        }

        public EmployeeDocumentScansController(IDbContextFactory factory, IRequest request)
        {
            this.factory = factory;
            this.request = request;
        }

        /// <summary>
        /// Method fo file upload
        /// </summary>
        /// <param name="tableName">Image of which document we're uploading</param>
        /// <param name="tableId">Id of document which image we're uploading</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("UploadFile")]
        [ResponseType(typeof(List<EmployeeDocumentScan>))]
        public IHttpActionResult UploadEmployeeDocumentScan(string tableName, int tableId)
        {
            try
            {
                List<EmployeeDocumentScan> scans = new List<EmployeeDocumentScan>();
                foreach (string file in request.Collection())
                {
                    string filePath = request.SaveFile(file);
                    EmployeeDocumentScan scan = new EmployeeDocumentScan()
                    {
                        TableId = tableId,
                        ScanTableName = tableName,
                        ScanName = filePath
                    };
                    UnitOfWork unitOfWork = new UnitOfWork(factory);
                    scan.Id = scan.NewId(unitOfWork);
                    unitOfWork.EmployeeDocumentScansRepository.Insert(scan);
                    unitOfWork.Save();
                    scans.Add(scan);
                }
                return CreatedAtRoute("GetScanListByDocumentId", new { tableName = tableName, tableId = tableId }, scans);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        /// <summary>
        /// Get all scans by document Id 
        /// </summary>
        /// <param name="tableName">Image of which document we're uploading</param>
        /// <param name="tableId">Id of document which image we're uploading</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByDocumentId", Name = "GetScanListByDocumentId")]
        [ResponseType(typeof(IEnumerable<EmployeeDocumentScan>))]
        public IHttpActionResult GetEmployeeDocumentScanByDocumentId(string tableName, int tableId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<EmployeeDocumentScan> scans = unitOfWork.EmployeeDocumentScansRepository.Get(d => d.Deleted != true && d.TableId == tableId && d.ScanTableName == tableName);
                return Ok(scans);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Get all scans by employee
        /// </summary>
        /// <param name="tableName">Image of which document we're uploading</param>
        /// <param name="tableId">Id of document which image we're uploading</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(IEnumerable<EmployeeDocumentScan>))]
        public IHttpActionResult GetEmployeeDocumentScanByEmployeeId(int employeeId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                SqlParameter parameter = new SqlParameter
                {
                    ParameterName = "@EmployeeId",
                    IsNullable = true,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = employeeId
                };
                IEnumerable<EmployeeDocumentScan> scans = unitOfWork.SQLQuery<EmployeeDocumentScan>("exec EmployeeDocumentScanValid @EmployeeId", parameter);
                return Ok(scans);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Get scan by its id
        /// </summary>
        /// <param name="id">scan id</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get")]
        [ResponseType(typeof(EmployeeDocumentScan))]
        public IHttpActionResult GetEmployeeDocumentScan(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeeDocumentScan scan = unitOfWork.EmployeeDocumentScansRepository.GetByID(id);
                return Ok(scan);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Delete scan 
        /// </summary>
        /// <param name="id">scan id</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(EmployeeDocumentScan))]
        public IHttpActionResult DeleteEmployeeDocumentScan(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                EmployeeDocumentScan scan = unitOfWork.EmployeeDocumentScansRepository.GetByID(id);
                scan.Deleted = true;
                unitOfWork.EmployeeDocumentScansRepository.Update(scan);
                unitOfWork.Save();
                return Ok(scan);
            }
            catch (NotFoundException nfe)
            {
                return NotFound();
            }
            catch (ConflictException ce)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Dispose controller
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }

}
