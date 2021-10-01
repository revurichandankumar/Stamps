using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneposStamps.Models;

namespace OneposStamps.Controllers
{
    public class ZoneController : BaseController
    {
        public ActionResult Index(string store = null)
        {
            var id = store;
            DataSet ds = db.GetMysqlDataSet("USP_GetZones", id);
            Zones ZoneData = new Zones();
            if (ds.Tables.Count > 0)
            {
                List<Zonelist> zl = new List<Zonelist>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    
                    Zonelist a = new Zonelist();
                    a.ZoneId= (row["Id"]).ToString();
                    a.Carrier = (row["Carrier"]).ToString();
                    a.Citycount = (row["City"]).ToString();
                    a.Shipmentfee = Convert.ToDecimal(row["ShipmentFee"]);
                    a.Statecount = (row["State"]).ToString();
                    a.Zipcount = (row["Zipcodes"]).ToString();
                    a.ZoneName = (row["ZoneName"]).ToString();
                    zl.Add(a);
                    
                }
                ZoneData.ZoneList = zl;
            }

            return View("Index", ZoneData);

        }

        public ActionResult AddZone()
        {
            return View();
        }

    }
}