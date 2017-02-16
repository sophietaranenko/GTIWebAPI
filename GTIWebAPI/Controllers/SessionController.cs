using GTIWebAPI.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GTIWebAPI.Controllers
{
    [RoutePrefix("api/Session")]
    public class SessionController : ApiController
    {
        [HttpGet]
        [Route("Get")]
        public int GetCurrentUser()
        {
            DbSession db = new DbSession();
            int res = db.GetCurrentUser();
            return res;
        }
    }
}
