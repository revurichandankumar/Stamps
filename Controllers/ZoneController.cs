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
                ZoneData.StoreId = id;
                ZoneData.ZoneList = zl;
            }

            return View("Index", ZoneData);

        }

        public ActionResult AddZone(string StoreId = null)
        {
            var id = StoreId;
            DataSet ds = db.GetCarrierdata("USP_GetCarrier");
            AddZones AddZonesData = new AddZones();
            List<CarrierData> cd = new List<CarrierData>();
            List<ServicetypeData> sd = new List<ServicetypeData>();
            List<PackageData> pd = new List<PackageData>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {

                CarrierData a = new CarrierData();
                a.Id = (row["CarrierId"]).ToString();
                a.Name = (row["CarrierName"]).ToString();

                cd.Add(a);

            }
            foreach (DataRow row in ds.Tables[1].Rows)
            {

                ServicetypeData a = new ServicetypeData();
                a.Id = (row["ServiceTypeId"]).ToString();
                a.Name = (row["ServiceTypeName"]).ToString();

                sd.Add(a);

            }
            foreach (DataRow row in ds.Tables[2].Rows)
            {

                PackageData a = new PackageData();
                a.Id = (row["PackageId"]).ToString();
                a.Name = (row["PackageaName"]).ToString();

                pd.Add(a);

            }
            AddZonesData.CarrierList = cd;
            AddZonesData.PackageList = pd;
            AddZonesData.ServiceList = sd;
            AddZonesData.Store_Id = StoreId;

            return View("AddZone", AddZonesData);
        }

        [HttpPost]
        public ActionResult InsetZones(InsertZones obj)
        {
            DataSet ds = db.InsertZone("USP_InsertZones", obj);

            return View();
        }
    }
}