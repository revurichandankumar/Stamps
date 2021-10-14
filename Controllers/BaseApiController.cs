using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OneposStamps.Controllers
{
    public class BaseApiController : ApiController
    {
        public Helper.SqlDBConnection db = Helper.SqlDBConnection.GetInstance();
    }
}
