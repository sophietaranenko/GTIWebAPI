using GTIWebAPI.Filters;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using GTIWebAPI.Models.Dictionary;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web.Http.Description;
using GTIWebAPI.Models.Account;
using Microsoft.AspNet.Identity;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using GTIWebAPI.Models.Repository.Organization;
using GTIWebAPI.Exceptions;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Controller for organizations
    /// </summary>
    [RoutePrefix("api/Organizations")]
    public class OrganizationsController : ApiController
    {
        private IOrganizationsRepository repo;

        public OrganizationsController()
        {
            repo = new OrganizationsRepository();
        }

        public OrganizationsController(IOrganizationsRepository repo)
        {
            this.repo = repo;
        }

        [GTIFilter]
        [HttpGet]
        [Route("SearchOrganization")]
        [ResponseType(typeof(IEnumerable<OrganizationSearchDTO>))]
        public IHttpActionResult SearchOrganization(int countryId, string registrationNumber)
        {
            try
            {
                List<OrganizationSearchDTO> list = repo.Search(countryId, registrationNumber);
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

        [GTIOfficeFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<OrganizationView>))]
        public IHttpActionResult GetOrganizationByOfficeIds(string officeIds)
        {
            List<int> OfficeIds = QueryParser.Parse(officeIds, ',');
            try
            {
                List<OrganizationView> organizationList = repo.GetAll(OfficeIds);
                return Ok(organizationList);
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
        /// Get one organization by organization Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [GTIFilter]
        [HttpGet]
        [Route("GetView", Name = "GetOrganizationView")]
        [ResponseType(typeof(OrganizationDTO))]
        public IHttpActionResult GetOrganizationView(int id)
        {
            try
            {
                Organization organization = repo.GetView(id);
                IEnumerable<int> popertyTypeIds = organization
                .OrganizationProperties
                .Select(d => d.OrganizationPropertyTypeId)
                .Distinct();

                //особый вид OrganizationProperties 
                List<OrganizationPropertyTreeView> propertiesDTO = new List<OrganizationPropertyTreeView>();
                foreach (int value in popertyTypeIds)
                {
                    List<OrganizationProperty> propertiesByType =
                    organization
                    .OrganizationProperties
                    .Where(d => d.OrganizationPropertyTypeId == value)
                    .ToList();

                    propertiesDTO.Add(new OrganizationPropertyTreeView
                    {
                        OrganizationPropertyTypeId = value,
                        PropertiesById = propertiesByType.Select(d => d.ToDTO())
                    });
                }
                OrganizationDTO dto = organization.MapToDTO();
                dto.OrganizationProperties = propertiesDTO;
                return Ok(dto);
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
        [Route("GetEdit", Name = "GetOrganizationEdit")]
        [ResponseType(typeof(OrganizationEditDTO))]
        public IHttpActionResult GetOrganizationEdit(int id)
        {        
            try
            {
                OrganizationEditDTO dto = repo.GetEdit(id).MapToEdit();
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }


        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        public IHttpActionResult PutOrganization(int id, Organization organization)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != organization.Id)
            {
                return BadRequest();
            }

            try
            {
                OrganizationEditDTO dto = repo.Edit(organization).MapToEdit();
                return Ok(dto);
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
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(OrganizationEditDTO))]
        public IHttpActionResult PostOrganization(Organization organization)
        {
            try
            {
                string userId = ActionContext.RequestContext.Principal.Identity.GetUserId();
                ApplicationUser user = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);

                if (user != null && user.TableName == "Employee")
                {
                    organization.EmployeeId = user.TableId;
                    try
                    {
                        OrganizationEditDTO dto = repo.Add(organization).MapToEdit();
                        return CreatedAtRoute("GetOrganizationEdit", new { id = dto.Id }, dto);
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
            return BadRequest();
        }

        /// <summary>
        /// Delete organization
        /// </summary>
        /// <param name="id">organization id</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult DeleteOrganization(int id)
        {
            try
            {
                OrganizationEditDTO dto = repo.DeleteOrganization(id).MapToEdit();
                return Ok(dto);
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
        [Route("GetOrganizationList")]
        [ResponseType(typeof(OrganizationList))]
        public IHttpActionResult GetOrganizationTypes()
        {
            try
            {
                OrganizationList list = repo.GetTypes();
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
    }
}
