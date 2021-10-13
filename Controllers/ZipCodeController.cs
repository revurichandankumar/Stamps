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