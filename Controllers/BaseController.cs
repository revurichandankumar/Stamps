using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneposStamps.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
       
             public Helper.SqlDBConnection db = Helper.SqlDBConnection.GetInstance();
        
    }
}