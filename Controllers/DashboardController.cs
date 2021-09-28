using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneposStamps.Controllers
{
    public class DashboardController : BaseController
    {
        // GET: Dashboard
        public ActionResult Index(string storeId = null, int type = 0)
        {
            switch (type)
            {
                case 1:
                    return RedirectToAction("Zones", "Report");
            }
            return View();
        }
    }
}