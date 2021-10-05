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
        public ActionResult AddZipCodes()
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
}