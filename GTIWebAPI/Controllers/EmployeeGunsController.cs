﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Filters;
using AutoMapper;
using GTIWebAPI.Models.Repository;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Document confirming employee's permission to store and carry weapon 
    /// </summary>
    [RoutePrefix("api/EmployeeGuns")]
    public class EmployeeGunsController : ApiController
    {
        private IRepository<EmployeeGun> repo;

        public EmployeeGunsController()
        {
            repo = new EmployeeGunsRepository();
        }

        public EmployeeGunsController(IRepository<EmployeeGun> repo)
        {
            this.repo = repo;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<EmployeeGunDTO>))]
        public IHttpActionResult GetEmployeeGunAll()
        {
            IEnumerable<EmployeeGunDTO> dtos = new List<EmployeeGunDTO>();

            try
            {
                List<EmployeeGunDTO> guns = 
                    repo.GetAll()
                    .Select(g => g.ToDTO())
                    .ToList();
                return Ok(repo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(IEnumerable<EmployeeGunDTO>))]
        public IHttpActionResult GetEmployeeGunByEmployee(int employeeId)
        {
            try
            {
                List<EmployeeGunDTO> guns = 
                    repo.GetByEmployeeId(employeeId)
                    .Select(d => d.ToDTO())
                    .ToList();
                return Ok(guns);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetEmployeeGun")]
        [ResponseType(typeof(EmployeeGunDTO))]
        public IHttpActionResult GetEmployeeGun(int id)
        {
            try
            {
                EmployeeGunDTO gun = repo.Get(id).ToDTO();
                return Ok(gun);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }


        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeGun(int id, EmployeeGun employeeGun)
        {
            if (employeeGun == null || !ModelState.IsValid || id != employeeGun.Id)
            {
                return BadRequest(ModelState);
            }
            try
            {
                EmployeeGunDTO gun = repo.Edit(employeeGun).ToDTO();
                return Ok(gun);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(EmployeeGunDTO))]
        public IHttpActionResult PostEmployeeGun(EmployeeGun employeeGun)
        {
            if (employeeGun == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                EmployeeGunDTO gun = repo.Add(employeeGun).ToDTO();
                return CreatedAtRoute("GetEmployeeGun", new { id = gun.Id }, gun);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(EmployeeGun))]
        public IHttpActionResult DeleteEmployeeGun(int id)
        {
            try
            {
                EmployeeGunDTO gun = repo.Delete(id).ToDTO();
                return Ok(gun);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}