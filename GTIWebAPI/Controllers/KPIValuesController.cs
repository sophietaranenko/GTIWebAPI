using GTIWebAPI.Exceptions;
using GTIWebAPI.Filters;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Reports.ProductivityReport;
using GTIWebAPI.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/KPIValues")]
    public class KPIValuesController : ApiController
    {
        private IDbContextFactory factory;

        public KPIValuesController()
        {
            factory = new DbContextFactory();
        }

        public KPIValuesController(IDbContextFactory factory)
        {
            this.factory = factory;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IEnumerable<KPIValueDTO>))]
        public IHttpActionResult GetKPIValuesAll()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<KPIValueDTO> dtos =
                    unitOfWork.KPIValuesRepository.Get(d => d.Deleted != true, includeProperties: "KPIParameter,KPIPeriod").Select(d => d.ToDTO());
                return Ok(dtos);
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
        [Route("GetByParameterId")]
        [ResponseType(typeof(IEnumerable<KPIValueDTO>))]
        public IHttpActionResult GetKPIValueByParameter(int parameterId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<KPIValue> cs = unitOfWork.KPIValuesRepository.
                    Get(d => d.Deleted != true && d.KPIParameterId == parameterId, includeProperties: "KPIParameter,KPIPeriod");
                IEnumerable<KPIValueDTO> dtos = cs.Select(d => d.ToDTO());
                return Ok(dtos);
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
        /// Get one car for view by car id
        /// </summary>
        /// <param name="id">KPIValue id</param>
        /// <returns>KPIValueEditDTO object</returns>
        [GTIFilter]
        [HttpGet]
        [Route("Get", Name = "GetKPIValue")]
        [ResponseType(typeof(KPIValueDTO))]
        public IHttpActionResult GetKPIValue(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                KPIValueDTO dto = unitOfWork.KPIValuesRepository.Get(d => d.Id == id, includeProperties: "KPIParameter,KPIPeriod").FirstOrDefault().ToDTO();
                if (dto == null)
                {
                    return NotFound();
                }
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

        /// <summary>
        /// Update employee car
        /// </summary>
        /// <param name="id">Car id</param>
        /// <param name="employeeCar">KPIValue object</param>
        /// <returns>204 - No content</returns>
        [GTIFilter]
        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutKPIValue(int id, KPIValue KPIvalue)
        {
            if (KPIvalue == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != KPIvalue.Id)
            {
                return BadRequest();
            }
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.KPIValuesRepository.Update(KPIvalue);
                unitOfWork.Save();

                //cos there are no included object-properies we need to load, then just ToDTO call  
                KPIValueDTO dto = dto = unitOfWork.KPIValuesRepository.Get(d => d.Id == id, includeProperties: "KPIParameter,KPIPeriod").FirstOrDefault().ToDTO();
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

        /// <summary>
        /// Insert new employee car
        /// </summary>
        /// <param name="KPIvalue">KPIValue object</param>
        /// <returns></returns>
        [GTIFilter]
        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(KPIValueDTO))]
        public IHttpActionResult PostKPIValue(KPIValue KPIvalue)
        {
            if (KPIvalue == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                unitOfWork.KPIValuesRepository.Insert(KPIvalue);
                
                unitOfWork.Save();
                int newId = KPIvalue.Id;
                //cos there are no included object-properies we need to load, then just ToDTO call  
                KPIValueDTO dto = dto = unitOfWork.KPIValuesRepository.Get(d => d.Id == newId, includeProperties: "KPIParameter,KPIPeriod").FirstOrDefault().ToDTO();
                return CreatedAtRoute("GetKPIValue", new { id = dto.Id }, dto);
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
        /// Delete car
        /// </summary>
        /// <param name="id">Car Id</param>
        /// <returns>200</returns>
        [GTIFilter]
        [HttpDelete]
        [Route("Delete")]
        [ResponseType(typeof(KPIValue))]
        public IHttpActionResult DeleteKPIValue(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);

                KPIValue KPIvalue = unitOfWork.KPIValuesRepository.GetByID(id);
                KPIvalue.Deleted = true;
                unitOfWork.KPIValuesRepository.Update(KPIvalue);
                unitOfWork.Save();
                KPIValueDTO dto = dto = unitOfWork.KPIValuesRepository.Get(d => d.Id == id, includeProperties: "KPIParameter,KPIPeriod").FirstOrDefault().ToDTO();
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
        [Route("GetList")]
        public IHttpActionResult GetList()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(factory);
                IEnumerable<KPIParameterDTO> parameters = unitOfWork.KPIParametersRepository.Get().Select(d => d.ToDTO());
                IEnumerable<KPIPeriodDTO> periods = unitOfWork.KPIPeriodsRepository.Get().Select(d => d.ToDTO());

                return Ok(new { parameters, periods});
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
