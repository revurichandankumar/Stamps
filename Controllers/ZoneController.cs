using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneposStamps.Controllers
{
    public class ZoneController : Controller
    {
        // GET: Zone
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddZone()
        {
            return View();
        }
    }
}