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

            DataSet ds = db.GetZipcodeData("USP_GetZipcodesDefaultData", statename, cityname, zipcode);
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
                    return new HttpStatusCodeResult(400, "No data found");
                }
                //ZipData.StoreId = zf.StoreId;
                //ZipData.ZoneId = zf.ZoneId;

                var groupedCustomerList = Gz.GroupBy(u => u.Name).Select(grp => grp.ToList()).ToList();
                //foreach(ZipData.ZipCodeList a in groupedCustomerList)
                //{

                //}


                //var json = JsonConvert.SerializeObject(groupedCustomerList);


            return View();
        }

    }

}