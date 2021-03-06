﻿using GTIWebAPI.Filters;
using GTIWebAPI.Models.Organizations;
using GTIWebAPI.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GTIWebAPI.Models.Account;
using Microsoft.AspNet.Identity;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using GTIWebAPI.Exceptions;
using System.Data;
using System.Data.SqlClient;
using GTIWebAPI.Models.Repository;

namespace GTIWebAPI.Controllers
{
    /// <summary>
    /// Controller for organizations
    /// </summary>
    [RoutePrefix("api/Organizations")]
    public class OrganizationsController : ApiController
    {
        private IDbContextFactory factory;
        private IIdentityHelper identityHelper;

        public OrganizationsController()
        {
            factory = new DbContextFactory();
            identityHelper = new IdentityHelper();
        }

        public OrganizationsController(IDbContextFactory factory)
        {
            this.factory = factory;
            this.identityHelper = new IdentityHelper();
        }

        public OrganizationsController(IDbContextFactory factory, IIdentityHelper identityHelper)
        {
            this.factory = factory;
            this.identityHelper = identityHelper;
        }

        [GTIFilter]
        [HttpGet]
        [Route("SearchOrganization")]
        [ResponseType(typeof(IEnumerable<OrganizationSearchDTO>))]
        public IHttpActionResult SearchOrganization(int countryId, string registrationNumber)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                SqlParameter pCountryId = new SqlParameter
                {
                    ParameterName = "@CountryId",
                    IsNullable = true,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = countryId
                };
                SqlParameter pRegistrationNumber = new SqlParameter
                {
                    ParameterName = "@RegistrationNumber",
                    IsNullable = true,
                    Direction = ParameterDirection.Input,
                    DbType = DbType.String,
                    Size = 50,
                    Value = registrationNumber
                };
                IEnumerable<OrganizationSearchDTO> orgs = unitOfWork.SQLQuery<OrganizationSearchDTO>("exec SearchOrganization @CountryId, @RegistrationNumber", pCountryId, pRegistrationNumber);
                foreach (var item in orgs)
                {
                    SqlParameter parId = new SqlParameter
                    {
                        ParameterName = "@OrganizationId",
                        IsNullable = false,
                        Direction = ParameterDirection.Input,
                        DbType = DbType.Int32,
                        Value = item.Id
                    };
                    item.OrganizationGTILinks = unitOfWork.SQLQuery<OrganizationGTIShortDTO>("exec OrganizationGTILinkForSearchByOrganization @OrganizationId", parId).ToList();

                }
                return Ok(orgs);
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                DataTable dataTable = new DataTable();
                dataTable.Clear();
                dataTable.Columns.Add("Value");
                foreach (var id in OfficeIds)
                {
                    DataRow row = dataTable.NewRow();
                    row["Value"] = id;
                    dataTable.Rows.Add(row);
                }
                SqlParameter parameter = new SqlParameter
                {
                    ParameterName = "@OfficeIds",
                    TypeName = "ut_IntList",
                    Value = dataTable,
                    SqlDbType = SqlDbType.Structured
                };
                IEnumerable<OrganizationView> orgs = unitOfWork.SQLQuery<OrganizationView>("exec OrganizationByOfficeIds @OfficeIds", parameter);
                return Ok(orgs);
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
        [Route("GetAllLead")]
        [ResponseType(typeof(IEnumerable<OrganizationView>))]
        public IHttpActionResult GetOrganizationLeadByOfficeIds(string officeIds)
        {
            List<int> OfficeIds = QueryParser.Parse(officeIds, ',');
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                DataTable dataTable = new DataTable();
                dataTable.Clear();
                dataTable.Columns.Add("Value");
                foreach (var id in OfficeIds)
                {
                    DataRow row = dataTable.NewRow();
                    row["Value"] = id;
                    dataTable.Rows.Add(row);
                }
                SqlParameter parameter = new SqlParameter
                {
                    ParameterName = "@OfficeIds",
                    TypeName = "ut_IntList",
                    Value = dataTable,
                    SqlDbType = SqlDbType.Structured
                };
                IEnumerable<OrganizationView> orgs = unitOfWork.SQLQuery<OrganizationView>("exec OrganizationLeadByOfficeIds @OfficeIds", parameter);
                return Ok(orgs);
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
        [Route("GetByEmployeeId")]
        [ResponseType(typeof(IEnumerable<OrganizationView>))]
        public IHttpActionResult GetOrganizationsByEmployeeId(int employeeId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                SqlParameter parameter = new SqlParameter
                {
                    ParameterName = "@EmployeeId",
                    DbType = DbType.Int32,
                    Value = employeeId
                };
                IEnumerable<OrganizationView> orgs = unitOfWork.SQLQuery<OrganizationView>("exec OrganizationByOwner @EmployeeId", parameter);
                return Ok(orgs);
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);

                OrganizationDTO organization = unitOfWork.OrganizationsRepository.Get(d => d.Id == id).FirstOrDefault().MapToDTO();

                //Addresses with Address values 
                organization.OrganizationAddresses = unitOfWork.OrganizationAddressesRepository
                    .Get(d => d.Deleted != true && d.OrganizationId == id, includeProperties: "Address,Address.AddressLocality,Address.AddressPlace,Address.AddressRegion,Address.AddressVillage,Address.Country,OrganizationAddressType")
                    .Select(d => d.ToDTO());


                //ContactPersons with contacts 
                organization.OrganizationContactPersons = unitOfWork.OrganizationContactPersonsViewRepository
                    .Get(d => d.Deleted != true && d.OrganizationId == id)
                    .Select(d => d.ToDTO());
                if (organization.OrganizationContactPersons != null)
                {
                    foreach (var person in organization.OrganizationContactPersons)
                    {
                        person.OrganizationContactPersonContact = unitOfWork.OrganizationContactPersonContactsRepository
                        .Get(d => d.Deleted != true && d.OrganizationContactPersonId == person.Id, includeProperties: "ContactType").Select(d => d.ToDTO());
                    }
                }


                //GTI Links with OrganizationGTI and Office
                organization.OrganizationGTILinks = unitOfWork.OrganizationGTILinksRepository
                    .Get(d => d.Deleted != true && d.OrganizationId == id)
                    .Select(d => d.ToDTO());
                foreach (var link in organization.OrganizationGTILinks)
                {
                    if (link.GTIId != null)
                    {
                        link.OrganizationGTI = unitOfWork.OrganizationGTIsRepository.
                            Get(d => d.Id == link.GTIId).FirstOrDefault().ToDTO();
                        if (link.OrganizationGTI != null)
                        {
                            link.OrganizationGTI.Office =
                            unitOfWork.OfficesRepository.Get(d => d.Id == link.OrganizationGTI.OfficeId).FirstOrDefault().ToDTO();
                        }
                    }
                }


                //Properties
                List<OrganizationProperty> properties = unitOfWork.OrganizationPropertiesRepository
                    .Get(d => d.Deleted != true && d.OrganizationId == id, includeProperties: "OrganizationPropertyType,OrganizationPropertyType.OrganizationPropertyTypeAlias").ToList();


               IEnumerable<OrganizationPropertyType> types = properties.Select(d => d.OrganizationPropertyType).Distinct();

                List<OrganizationPropertyTreeView> propertiesDTO = new List<OrganizationPropertyTreeView>();

                foreach (var value in types)
                {
                    List<OrganizationProperty> propertiesByType =
                    properties
                    .Where(d => d.OrganizationPropertyTypeId == value.Id)
                    .ToList();
                    propertiesDTO.Add(new OrganizationPropertyTreeView
                    {
                        OrganizationPropertyType = value.ToDTO(),
                        PropertiesByType = propertiesByType.Select(d => d.ToTreeViewDTO())
                    });
                }
                organization.OrganizationProperties = propertiesDTO;


                //Tax addresses 
                organization.OrganizationTaxAddresses = unitOfWork.OrganizationTaxAddressesRepository
                    .Get(d => d.Deleted != true && d.OrganizationId == id, includeProperties: "Address,Address.AddressRegion,Address.AddressLocality,Address.AddressPlace,Address.AddressVillage,Address.Country")
                    .Select(d => d.ToDTO());

                //Language names 
                organization.OrganizationLanguageNames = unitOfWork.OrganizationLanguageNamesRepository
                    .Get(d => d.Deleted != true && d.OrganizationId == id, includeProperties: "Language").Select(d => d.ToDTO());
                return Ok(organization);
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                OrganizationEditDTO organization = unitOfWork.OrganizationsRepository
                    .Get(d => d.Id == id, includeProperties: "Country,OrganizationLegalForm")
                        .FirstOrDefault().MapToEdit();
                return Ok(organization);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }


        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        public IHttpActionResult PutOrganization(int id, OrganizationEditDTO organization)
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
                Organization org = organization.FromDTO();
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.OrganizationsRepository.Update(org);
                unitOfWork.Save();
                OrganizationEditDTO dto = unitOfWork.OrganizationsRepository
                    .Get(d => d.Id == id, includeProperties: "Country,OrganizationLegalForm")
                        .FirstOrDefault().MapToEdit();
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
        [HttpPut]
        [Route("ChangeOwner")]
        public IHttpActionResult ChangeOrganizationOwner(int organizationId, int ownerId)
        {
            if (organizationId == 0 && ownerId == 0)
            {
                return BadRequest("Empty organization or owner!");
            }
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.OrganizationOwnersRepository.Insert(
                    new OrganizationOwner()
                    {
                        EmployeeId = ownerId,
                        OrganizationId = organizationId,
                        DateBegin = DateTime.Now
                    }
                    );
                unitOfWork.Save();
                return Ok();
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

        //мы знаем страну??? или нет???
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(OrganizationEditDTO))]
        public IHttpActionResult PostOrganization(OrganizationEditDTO organization)
        {
            try
            {
                if (identityHelper.GetUserTableName(User) == "Employee")
                {
                    if (organization.CountryId == null || organization.CountryId == 0)
                    {
                        return BadRequest("Empty CountryId");
                    }

                    Organization org = organization.FromDTO();
                    //now this information is stored in OrganizationOwner (we do not need this anymore) 
                    //  organization.EmployeeId = identityHelper.GetUserTableId(User);
                    UnitOfWork unitOfWork = new UnitOfWork(factory);
                    org.Id = org.NewId(unitOfWork);
                    unitOfWork.OrganizationsRepository.Insert(org);

                    unitOfWork.OrganizationOwnersRepository.Insert(
                          new OrganizationOwner()
                          {
                              DateBegin = DateTime.Now,
                              EmployeeId = identityHelper.GetUserTableId(User),
                              OrganizationId = org.Id
                          });

                    //constant types 
                    List<OrganizationPropertyType> types = unitOfWork.OrganizationPropertyTypesRepository.Get(d => d.Constant == true && d.CountryId == org.CountryId).ToList();
                    foreach (var type in types)
                    {
                        OrganizationProperty property = new OrganizationProperty();
                        property.Id = property.NewId(unitOfWork);
                        property.OrganizationPropertyTypeId = type.Id;
                        property.OrganizationId = org.Id;
                        unitOfWork.OrganizationPropertiesRepository.Insert(property);
                    }
                    unitOfWork.Save();
                    OrganizationEditDTO dto = unitOfWork.OrganizationsRepository
                        .Get(d => d.Id == org.Id, includeProperties: "Country,OrganizationLegalForm")
                    .FirstOrDefault().MapToEdit();

                    return CreatedAtRoute("GetOrganizationEdit", new { id = dto.Id }, dto);
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                Organization organization = unitOfWork.OrganizationsRepository
                    .Get(d => d.Id == id, includeProperties: "Country,OrganizationLegalForm")
                    .FirstOrDefault();
                organization.Deleted = true;
                unitOfWork.OrganizationsRepository.Update(organization);
                return Ok(organization.MapToEdit());
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
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                OrganizationList list = unitOfWork.CreateOrganizationList();
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
