using GTIWebAPI.Exceptions;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Repository;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/OrganizationProperties")]
    public class OrganizationPropertiesController : ApiController
    {
        private IDbContextFactory factory;
         //    private static Logger logger = LogManager.GetCurrentClassLogger();

        public OrganizationPropertiesController()
        {
            factory = new DbContextFactory();
            
        }

        public OrganizationPropertiesController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetByOrganizationId")]
        [ResponseType(typeof(IEnumerable<OrganizationPropertyDTO>))]
        public IHttpActionResult GetOrganizationPropertyByOrganizationId(int organizationId)
        {
            try
            {
                UnitOfWork unitOFWork = new UnitOfWork(factory);

                IEnumerable<OrganizationPropertyDTO> properties = unitOFWork.OrganizationPropertiesRepository
                    .Get(d => d.Deleted != true && d.OrganizationId == organizationId, includeProperties: "OrganizationPropertyType,OrganizationPropertyType.OrganizationPropertyTypeAlias")
                    .Select(d => d.ToDTO());
                //string name = this.ControllerContext.RouteData.Values["action"].ToString();
             //   logger.Log(LogLevel.Info, "sss", User.Identity.Name, DateTime.Now, organizationId);
                return Ok(properties);
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
        [Route("Get", Name = "GetOrganizationProperty")]
        [ResponseType(typeof(OrganizationPropertyDTO))]
        public IHttpActionResult GetOrganizationProperty(int id)
        {
            try
            {

                UnitOfWork unitOFWork = new UnitOfWork(factory);
                OrganizationPropertyDTO property = unitOFWork.OrganizationPropertiesRepository
                    .Get(d => d.Id == id, includeProperties: "OrganizationPropertyType,OrganizationPropertyType.OrganizationPropertyTypeAlias")
                    .FirstOrDefault()
                    .ToDTO();
              //  logger.Log(LogLevel.Info, "how to pass obkects", null, id, property);

                //logger.Log<int>(LogLevel.Debug, id);
                return Ok(property);
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
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(OrganizationPropertyDTO))]
        public IHttpActionResult PutOrganizationProperty(int id, OrganizationProperty organizationProperty)
        {
            if (organizationProperty == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != organizationProperty.Id)
            {
                return BadRequest();
            }
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                int? propertyCountryId = unitOfWork.OrganizationPropertyTypesRepository
                .Get(d => d.Id == organizationProperty.OrganizationPropertyTypeId)
                    .Select(d => d.CountryId)
                    .FirstOrDefault();
                int? organizationCountryId = unitOfWork.OrganizationsRepository
                .Get(d => d.Id == organizationProperty.OrganizationId)
                    .Select(d => d.CountryId)
                    .FirstOrDefault();
                if (propertyCountryId != organizationCountryId)
                {
                    return BadRequest("Country that property belogs to, doesn't match the Organization registration country");
                }
                unitOfWork.OrganizationPropertiesRepository.Update(organizationProperty);
                unitOfWork.Save();
               // logger.Log(LogLevel.Info, "how to pass objects");
                OrganizationPropertyDTO property = unitOfWork.OrganizationPropertiesRepository
                    .Get(d => d.Id == id)
                    .FirstOrDefault()
                    .ToDTO();
                property.OrganizationPropertyType = null;
                return Ok(property);
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
        [ResponseType(typeof(OrganizationPropertyDTO))]
        public IHttpActionResult PostOrganizationProperty(OrganizationProperty organizationProperty)
        {
            if (organizationProperty == null)
            {
                return BadRequest(ModelState);
            }
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                int? propertyCountryId = unitOfWork.OrganizationPropertyTypesRepository
                .Get(d => d.Id == organizationProperty.OrganizationPropertyTypeId)
                    .Select(d => d.CountryId)
                    .FirstOrDefault();
                int? organizationCountryId = unitOfWork.OrganizationsRepository
                .Get(d => d.Id == organizationProperty.OrganizationId)
                    .Select(d => d.CountryId)
                    .FirstOrDefault();

                if (propertyCountryId != organizationCountryId)
                {
                    return BadRequest("Country that property belogs to, doesn't match the Organization registration country");
                }

                organizationProperty.Id = organizationProperty.NewId(unitOfWork);
                unitOfWork.OrganizationPropertiesRepository.Insert(organizationProperty);
                unitOfWork.Save();
              //  logger.Log(LogLevel.Info, "how to pass objects");
                OrganizationPropertyDTO property = unitOfWork.OrganizationPropertiesRepository
                         .Get(d => d.Id == organizationProperty.Id)
                         .FirstOrDefault()
                         .ToDTO();
                property.OrganizationPropertyType = null;
                return CreatedAtRoute("GetOrganizationProperty", new { id = property.Id }, property);
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
        [Route("PostConstant")]
        [ResponseType(typeof(OrganizationPropertyDTO))]
        public IHttpActionResult PostArrayOrganizationProperty(IEnumerable<OrganizationPropertyConstant> properties)
        {
            if (properties == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            List<OrganizationPropertyDTO> propertiesToReturn = new List<OrganizationPropertyDTO>();

            UnitOfWork unitOfWork = new UnitOfWork(factory);

            try
            {
                foreach (var item in properties)
                {
                    OrganizationProperty organizationProperty = new OrganizationProperty()
                    {
                        DateBegin = null,
                        DateEnd = null,
                        Deleted = null,
                        OrganizationId = item.OrganizationId,
                        OrganizationPropertyTypeId = item.OrganizationPropertyTypeId,
                        Value = item.Value
                    };

                    int? propertyCountryId = unitOfWork.OrganizationPropertyTypesRepository
                    .Get(d => d.Id == organizationProperty.OrganizationPropertyTypeId)
                        .Select(d => d.CountryId)
                        .FirstOrDefault();
                    int? organizationCountryId = unitOfWork.OrganizationsRepository
                    .Get(d => d.Id == organizationProperty.OrganizationId)
                        .Select(d => d.CountryId)
                        .FirstOrDefault();

                    if (propertyCountryId != organizationCountryId)
                    {
                        return BadRequest("Country that property belogs to, doesn't match the Organization registration country");
                    }

                    organizationProperty.Id = organizationProperty.NewId(unitOfWork);
                    unitOfWork.OrganizationPropertiesRepository.Insert(organizationProperty);
                    unitOfWork.Save();

                    OrganizationPropertyDTO property = unitOfWork.OrganizationPropertiesRepository
                         .Get(d => d.Id == organizationProperty.Id)
                         .FirstOrDefault()
                         .ToDTO();
                    property.OrganizationPropertyType = null;
                    propertiesToReturn.Add(property);
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
            return Ok(propertiesToReturn);
        }

        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(OrganizationPropertyDTO))]
        public IHttpActionResult DeleteOrganizationProperty(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);

                OrganizationProperty organizationProperty = unitOfWork.OrganizationPropertiesRepository
                    .Get(d => d.Id == id).FirstOrDefault();
                organizationProperty.Deleted = true;
                unitOfWork.OrganizationPropertiesRepository.Update(organizationProperty);
                unitOfWork.Save();
                return Ok(organizationProperty.ToDTO());
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }


    }
}
