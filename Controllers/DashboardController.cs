using OneposStamps.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneposStamps.Controllers
{
    public class DashboardController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainId"></param>
        /// <param name="storeId"></param>
        /// <param name="reportid"></param>
        /// <returns></returns>
        public ActionResult Index( string storeId = null, int type = 0)
        {
            //TempData["Type"] = type;
            if ( !string.IsNullOrWhiteSpace(storeId))
            {

                var storeInfo = new Store();
                
                switch (type)
                {
                    case 1:
                        return RedirectToAction("Index", "Zone", new { store= storeId });
                    case 2:
                        return RedirectToAction("GetShipRates", "Order", new { StoreId = storeId } );

                }

            }
            return View();
        }
      
    }
}