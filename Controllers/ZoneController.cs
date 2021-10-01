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
        public ActionResult Zone(string storeid)
        {
            DataSet ds = db.GetMysqlDataSet("USP_GetZones", storeid);
            List<Zones> zoneData = new List<Zones>();
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Zones a = new Zones();
                    a.Carrier = (row["Carrier"]).ToString();
                    a.Citycount = (row["City"]).ToString();
                    a.Shipmentfee = Convert.ToDecimal(row["ShipmentFee"]);
                    a.Statecount = (row["State"]).ToString();
                    a.Zipcount = (row["Zipcodes"]).ToString();
                    a.ZoneName = (row["ZoneName"]).ToString();
                    zoneData.Add(a);
                }
            }

            return PartialView("_AddZone", zoneData);

        }

    }
}