using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneposStamps.Models;

using Newtonsoft.Json;

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
                    a.ZoneId = (row["Id"]).ToString();
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
        public ActionResult InsertZones(InsertZones obj)
        {
            DataSet ds = db.InsertZone("USP_InsertZones", obj);

            return Json(new { result = "Redirect", url = Url.Action("Index", "Zone") + "?store=" + obj.Store_Id });
        }


        public ActionResult EditZone(string Store_Id, string ZoneId)
        {
            var id = Store_Id;
            DataSet ds = db.GetCarrierdata("USP_GetCarrier");
            GetZoneSingleData UpdateZonesData = new GetZoneSingleData();
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

            UpdateZonesData.CarrierList = cd;
            UpdateZonesData.PackageList = pd;
            UpdateZonesData.ServiceList = sd;

            DataSet ds2 = db.GetZonesData("USP_GetZonesData", Store_Id, ZoneId);
            if (ds2.Tables.Count > 0)
            {
                foreach (DataRow row in ds2.Tables[0].Rows)
                {
                    UpdateZonesData.ZoneId = (row["ZoneId"]).ToString();
                    UpdateZonesData.ZoneName = (row["ZoneName"]).ToString();
                    UpdateZonesData.CarrierId = (row["CarrierId"]).ToString();
                    UpdateZonesData.PackageId = (row["PackageId"]).ToString();
                    UpdateZonesData.ServiceTypeId = (row["ServiceTypeId"]).ToString();
                    UpdateZonesData.ShipmentFee = Convert.ToDecimal(row["ShipmentFee"]);
                    UpdateZonesData.Store_Id = (row["Store_Id"]).ToString();
                    UpdateZonesData.Weight = Convert.ToDecimal(row["Weight"]);
                    UpdateZonesData.Length = Convert.ToDecimal(row["Length"]);
                    UpdateZonesData.Breadth = Convert.ToDecimal(row["Breadth"]);
                    UpdateZonesData.Height = Convert.ToDecimal(row["Height"]);
                }
            }
            return View("EditZone", UpdateZonesData);
        }

        public ActionResult UpdateZone(InsertZones obj)
        {
            DataSet ds = db.UpdateZone("USP_UpdateZones", obj);

            return Json(new { result = "Redirect", url = Url.Action("Index", "Zone") + "?store=" + obj.Store_Id });
        }

        [HttpPost]
        public ActionResult DeleteZone(string Store_Id, string ZoneId)
        {            
            DataSet ds = db.GetZonesData("USP_DeleteZone", Store_Id, ZoneId);
            string status = null;
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    status = (row["Status"]).ToString();
                }
            }

            //return status;
            return Json(new { result = "Redirect", url = Url.Action("Index", "Zone") + "?store=" + Store_Id });
        }

        public ActionResult GetZonesData(string StoreId,string ZoneId)
        {
            DataSet ds = db.GetZonesData("USP_GetZoneDetails", StoreId, ZoneId); //StoreId, ZoneId
            GetZones ZoneData = new GetZones();
            if (ds.Tables.Count > 0)
            {
                List<GetZoneData> Gz = new List<GetZoneData>();
                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    GetZoneData a = new GetZoneData();
                    a.ZipCode = (row["Zip"]).ToString();
                    a.City = (row["City"]).ToString();
                    a.State = (row["State"]).ToString();
                    a.Id = (row["Id"]).ToString();
                    a.StoreId = (row["Store_Id"]).ToString(); Gz.Add(a);
                }
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ZoneData.ZoneName = (row["ZoneName"]).ToString();
                    ZoneData.Carriername = (row["CarrierName"]).ToString();
                    ZoneData.Shipmentfee = Convert.ToDecimal(row["ShipmentFee"]);
                    ZoneData.StoreId = StoreId;
                }
                ZoneData.GetZoneList = Gz;
            }
            return View("ZoneDetails", ZoneData);
        }

        public ActionResult ZoneDetails(GetZones gz)
        {

            return View();
        }

        public ActionResult GetZipDefaultData()
        {
            DataSet ds = db.GetCarrierdata("USP_GetZipcodesDefaultData");
            GetZipCodeData ZipData = new GetZipCodeData();
            if (ds.Tables.Count > 0)
            {
                List<ZipCodes> Gz = new List<ZipCodes>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ZipCodes a = new ZipCodes();
                    a.Name = (row["primary_city"]).ToString();
                    a.Zipcode = (row["zip"]).ToString();
                    Gz.Add(a);
                }
                ZipData.ZipCodeList = Gz;

                var groupedCustomerList = Gz.GroupBy(u => u.Name).Select(grp => grp.ToList()).ToList();
                //foreach(ZipData.ZipCodeList a in groupedCustomerList)
                //{

                //}
                

                //var json = JsonConvert.SerializeObject(groupedCustomerList);


            }
            return View();
        }
    }
    public class zipvalue
    {
        public List<cityname> citylist { get; set; }
    }
    public class cityname
    {
        public string Cityname { get; set; }
        public List<Ziplist> ziplists { get; set; }
    }
    public class Ziplist
    {
        public string Zipcode { get; set; }
    }
}