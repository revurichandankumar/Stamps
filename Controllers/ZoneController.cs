using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneposStamps.Models;

using Newtonsoft.Json;
using System.Data.SqlClient;

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
        public ActionResult InsertZones(InsertZones obj,int type=0)
        {
            if(type==1)
            {
                DataSet ds = db.InsertZone("USP_InsertZones", obj);
            }
            else
            {
                DataSet ds = db.InsertZone("USP_UpdateZones", obj);
            }
            
           

            return Json(new { result = "Redirect", url = Url.Action("Index", "Zone") + "?store=" + obj.Store_Id });
        }

        public ActionResult GetZonesData(string StoreId, string ZoneId)
        {



            DataSet ds = db.GetZonesData("USP_GetZoneDetails", StoreId, ZoneId);
            GetZones ZoneData = new GetZones();
            if (ds.Tables.Count > 0)
            {
                List<GetZoneDtsData> Gz = new List<GetZoneDtsData>();
                foreach (DataRow row in ds.Tables[1].Rows)
                {

                    GetZoneDtsData a = new GetZoneDtsData();
                    a.ZipCode = (row["Zip"]).ToString();
                    a.City = (row["City"]).ToString();
                    a.State = (row["State"]).ToString();
                    a.Id = (row["Id"]).ToString();
                    a.StoreId = (row["Store_Id"]).ToString();


                    Gz.Add(a);

                }
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ZoneData.ZoneName = (row["ZoneName"]).ToString();
                    ZoneData.Carriername = (row["CarrierName"]).ToString();
                    ZoneData.Shipmentfee = Convert.ToDecimal(row["ShipmentFee"]);
                }
                ZoneData.GetZoneList = Gz;

            }
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



            }
            return View();
        }
        public ActionResult GetStampsOrderScreen(string StoreId)
        {
            DataSet ds = db.GetMysqlDataSet("USP_GetDataBaseDetails", StoreId);
            DbData data = new DbData();
            if (ds.Tables.Count > 0)
            {
               
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    data.Address= (row["Address"]).ToString();
                    data.DatabaseName= (row["DatabaseName"]).ToString();
                    data.Password= (row["Password"]).ToString();
                    data.UserName= (row["UserName"]).ToString();
                }
            }
            DataSet ds1 = db.GetMysqlDataSet("USP_GetDataBaseDetails", StoreId);


            return View();
        }
        public ActionResult GetZonesdetails(string Store_Id, string ZoneId)
        {
            DataSet ds = db.GetZonesData("USP_GetZonesData", Store_Id, ZoneId);
            if (ds.Tables.Count > 0)
            {
                GetZoneSingleData ZoneData = new GetZoneSingleData();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                   
                    ZoneData.ZoneId = (row["ZoneId"]).ToString();
                    ZoneData.ZoneName = (row["ZoneName"]).ToString();
                    ZoneData.CarrierId= (row["CarrierId"]).ToString();
                    ZoneData.PackageId= (row["zip"]).ToString();
                    ZoneData.ServiceTypeId= (row["ServiceTypeId"]).ToString();
                    ZoneData.ShipmentFee= Convert.ToDecimal(row["ShipmentFee"]);
                    ZoneData.Store_Id= (row["zip"]).ToString();
                    ZoneData.Weight= Convert.ToDecimal(row["Weight"]);
                    ZoneData.Length= Convert.ToDecimal(row["Length"]);
                    ZoneData.Breadth= Convert.ToDecimal(row["Breadth"]);
                    ZoneData.Height= Convert.ToDecimal(row["Height"]);
                    
                }

            }
            return View();
        }
        public ActionResult DeleteZones(string Store_Id, string ZoneId)
        {
            DataSet ds = db.GetZonesData("USP_DeleteZone", Store_Id, ZoneId);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                  var status=(row["Status"]).ToString();
                }
            }
                return View();
        }
    }
   
}