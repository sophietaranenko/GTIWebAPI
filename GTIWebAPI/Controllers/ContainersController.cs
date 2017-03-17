using GTIWebAPI.Filters;
using GTIWebAPI.Models.Accounting;
using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Repository.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GTIWebAPI.Controllers
{
    //Containers as part of deal 

    [RoutePrefix("api/Containers")]
    public class ContainersController : ApiController
    {
        IContainersRepository repo;

        public ContainersController()
        {
            repo = new ContainersRepository();
        }

        public ContainersController(IContainersRepository repo)
        {
            this.repo = repo;
        }

        [GTIFilter]
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(List<DealContainerViewDTO>))]
        public IHttpActionResult GetContainers(int organizationId, DateTime? dateBegin, DateTime? dateEnd)
        {
            if (organizationId == 0)
            {
                return BadRequest("Empty organization");
            }
            if (dateBegin == null)
            {
                dateBegin = new DateTime(1900, 1, 1);
            }
            if (dateEnd == null)
            {
                dateEnd = new DateTime(2200, 1, 1);
            }
            DateTime modDateBegin = dateBegin.GetValueOrDefault();
            DateTime modeDateEnd = dateEnd.GetValueOrDefault();
            try
            {
                List<DealContainerViewDTO> list = repo.GetAll(organizationId, modDateBegin, modeDateEnd);
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [GTIFilter]
        [HttpGet]
        [ResponseType(typeof(DealContainerViewDTO))]
        [Route("Get")]
        public IHttpActionResult GetContainer(Guid id)
        {
            try
            {
                DealContainerViewDTO container = repo.Get(id);
                return Ok(container);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
