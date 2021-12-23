using Newtonsoft.Json;
using OneposStamps.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneposStamps.Controllers
{
    public class ZipCodeController : BaseController
    {
        // GET: ZipCode
        public ActionResult AddZipCodes(ZipcodeFilters zf)
        {
            GetZipCodeData ZipData = new GetZipCodeData();
            //GetZipCodeData ZipData2 = new GetZipCodeData();
            ZipData.StoreId = zf.StoreId;
            ZipData.ZoneId = zf.ZoneId;
            DataSet dsZoneList = db.GetMysqlDataSet("USP_GetZones", ZipData.StoreId);
            List<Zonelist> zl = new List<Zonelist>();
            if (dsZoneList.Tables.Count > 0)
            {

                foreach (DataRow row in dsZoneList.Tables[0].Rows)
                {

                    Zonelist a = new Zonelist();
                    a.ZoneId = (row["Id"]).ToString();
                    a.Carrier = (row["Carrier"]).ToString();
                    a.ZoneName = (row["ZoneName"]).ToString();

                    zl.Add(a);

                }

            }
            ZipData.ZoneName = zl.Where(x => x.ZoneId == ZipData.ZoneId).Select(y => y.ZoneName).FirstOrDefault();
            ZipData.CarrierName = zl.Where(x => x.ZoneId == ZipData.ZoneId).Select(y => y.Carrier).FirstOrDefault();

            if (string.IsNullOrEmpty(zf.StateName) && string.IsNullOrEmpty(zf.CityName) && string.IsNullOrEmpty(zf.ZipCode))
            {
                return View(ZipData);
            }

            string statename = string.Empty;
            string cityname = string.Empty;
            string zipcode = string.Empty;

            if (!string.IsNullOrEmpty(zf.StateName))
            {
                statename = zf.StateName;
            }
            if (!string.IsNullOrEmpty(zf.CityName))
            {
                cityname = zf.CityName;
            }
            if (!string.IsNullOrEmpty(zf.ZipCode))
            {
                zipcode = zf.ZipCode;
            }

            DataSet ds = db.GetZipcodeData("USP_GetZipcodesDefaultData", statename, cityname, zipcode, ZipData.StoreId, ZipData.ZoneId);
            
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
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
                    ZipData.GroupbyZipCodeList = Gz.GroupBy(u => u.Name).Select(grp => grp.ToList()).ToList();
                }
                else
                {
                    ZipData.GroupbyZipCodeList = new List<List<ZipCodes>>();
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    List<ZipCodes> Gz = new List<ZipCodes>();
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        ZipCodes a = new ZipCodes();
                        a.Name = (row["CityName"]).ToString();
                        a.Zipcode = (row["Zipcode"]).ToString();
                        Gz.Add(a);
                    }
                    ZipData.AddedZipCodeList = Gz;
                    ZipData.GroupbyAddedZipCodeList = Gz.GroupBy(u => u.Name).Select(grp => grp.ToList()).ToList();
                }
                else
                {
                    ZipData.GroupbyAddedZipCodeList= new List<List<ZipCodes>>();
                }
               
            }
            else
            {
                return new HttpStatusCodeResult(400, "No data found");
            }
            return PartialView("_AddZipCodes", ZipData);
        }

        public ActionResult SearchByFilerZipcodes(SearchedZipCodes sz)
        {
            GetZipCodeData ZipData = new GetZipCodeData();
            //GetZipCodeData ZipData2 = new GetZipCodeData();
            ZipData.StoreId = sz.zipcodeFilters.StoreId;
            ZipData.ZoneId = sz.zipcodeFilters.ZoneId;
            
            if (string.IsNullOrEmpty(sz.zipcodeFilters.StateName) && string.IsNullOrEmpty(sz.zipcodeFilters.CityName) && string.IsNullOrEmpty(sz.zipcodeFilters.ZipCode))
            {
                return View(ZipData);
            }

            string statename = string.Empty;
            string cityname = string.Empty;
            string zipcode = string.Empty;

            if (!string.IsNullOrEmpty(sz.zipcodeFilters.StateName))
            {
                statename = sz.zipcodeFilters.StateName;
            }
            if (!string.IsNullOrEmpty(sz.zipcodeFilters.CityName))
            {
                cityname = sz.zipcodeFilters.CityName;
            }
            if (!string.IsNullOrEmpty(sz.zipcodeFilters.ZipCode))
            {
                zipcode = sz.zipcodeFilters.ZipCode;
            }

            DataSet ds = db.GetZipcodeData("USP_GetZipcodesFilterData", statename, cityname, zipcode, ZipData.StoreId, ZipData.ZoneId);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    List<ZipCodes> Gz = new List<ZipCodes>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ZipCodes a = new ZipCodes();
                        a.Name = (row["primary_city"]).ToString();
                        a.Zipcode = (row["zip"]).ToString();
                        Gz.Add(a);
                    }
                    if (sz.ZipCodeList != null)
                    {
                        ZipData.ZipCodeList = Gz.Where(x => !sz.ZipCodeList.Select(a=> a.Zipcode).Contains(x.Zipcode)).ToList();
                    }
                    else
                    {
                        ZipData.ZipCodeList = Gz.ToList();
                    }

                    ZipData.GroupbyZipCodeList = ZipData.ZipCodeList.GroupBy(u => u.Name).Select(grp => grp.ToList()).ToList();
                }
                else
                {
                    ZipData.GroupbyZipCodeList = new List<List<ZipCodes>>();
                }

            }
            else
            {
                return new HttpStatusCodeResult(400, "No data found");
            }
            return PartialView("_SearchedZipCodes", ZipData);
        }

        public ActionResult InsertZipCodestoZone(GetZipCodeData zd)
        {
            List<ZipCodes> list = new List<ZipCodes>();
            if (zd.ZipCodeList != null && zd.ZipCodeList.Count > 0)
            {
                list = zd.ZipCodeList;
            }

            var jsons = JsonConvert.SerializeObject(list);
            DataSet ds = db.GetZipInsertData("USP_InsertZoneMasterData", jsons, zd.StoreId, zd.ZoneId);

            return Json(new { result = "Redirect", url = Url.Action("Index", "Zone") + "?store=" + zd.StoreId });
        }

    }

}