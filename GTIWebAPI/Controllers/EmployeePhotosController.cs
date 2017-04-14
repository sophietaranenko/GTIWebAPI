using GTIWebAPI.Exceptions;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Repository;
using System;
using System.Collections.Generic;
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
    /// Controller for storing and showing Employee photos
    /// </summary>
    [RoutePrefix("api/EmployeePhotos")]
    public class EmployeePhotosController : ApiController
    {

        private IEmployeePhotosRepository repo;

        public EmployeePhotosController()
        {
            repo = new EmployeePhotosRepository();
        }

        public EmployeePhotosController(IEmployeePhotosRepository repo)
        {
            this.repo = repo;
        }

        [GTIFilter]
        [HttpPost]
        [Route("UploadFile")]
        public IHttpActionResult UploadEmployeePhoto(int employeeId)
        {
            if (HttpContext.Current == null)
            {
                return BadRequest("No context found");
            }
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                try
                {
                    List<EmployeePhoto> photos = new List<EmployeePhoto>();
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        EmployeePhoto photo = new EmployeePhoto
                        {
                            EmployeeId = employeeId,
                            PhotoName = repo.SaveFile(postedFile)
                        };
                        photo = repo.Add(photo);
                        photos.Add(photo);
                    }
                    return CreatedAtRoute("GetByEmployeeId", new { employeeId = employeeId }, photos);
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
            else
            {
                return BadRequest("No photos found");
            }
        }

        ///// <summary>
        ///// Читает байтовый массив и превращает его в файл (для первоначального переноса при начале работы с новой базой данных)
        ///// </summary>
        ///// <returns></returns>
        //[GTIFilter]
        //[HttpPut]
        //[Route("PutDbFilesToFilesystem")]
        //public IHttpActionResult PutDbFilesToFilesystem()
        //{
        //    try
        //    {
        //        List<EmployeePhoto> photos = repo.PutDbFilesToFilesystem();
        //        return Ok(photos);
        //    }
        //    catch (NotFoundException nfe)
        //    {
        //        return NotFound();
        //    }
        //    catch (ConflictException ce)
        //    {
        //        return Conflict();
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId", Name = "GetByEmployeeId")]
        [ResponseType(typeof(List<EmployeePhoto>))]
        public IHttpActionResult GetEmployeePhotoByEmployeeId(int employeeId)
        {
            try
            {
                List<EmployeePhoto> list = repo.GetByEmployeeId(employeeId);
                return Ok(list);
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


        [GTIFilter]
        [HttpGet]
        [Route("Get")]
        public IHttpActionResult GetEmployeePhoto(int id)
        {
            try
            {
                EmployeePhoto photo = repo.Get(id);
                return Ok(photo);
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
        /// Delete photo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult DeleteEmployeePhoto(int id)
        {
            try
            {
                EmployeePhoto photo = repo.Delete(id);
                return Ok(photo);
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
