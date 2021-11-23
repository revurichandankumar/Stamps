using OneposStamps.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
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
        public ActionResult Index( string storeId = null, int type = 0,string storeName="")
        {

            Session["StoreId"] = storeId;
            if ( !string.IsNullOrWhiteSpace(storeId))
            {
                //db.Dispose();


                switch (type)
                {
                    case 1:
                        return RedirectToAction("Index", "Zone", new { store= storeId });
                        break;
                    case 2:
                        return RedirectToAction("OrderDetails", "Order", new { StoreId = storeId } );
                        break;
                    case 3:
                        return RedirectToAction("InhouseLabel", "Order", new { StoreId = storeId,StoreName= storeName });
                        break;
                }

            }
            return View();
        }
        //private DataTable GetDatabaseConnectionValues(string domainId)
        //{
        //    var ds = new DataSet();
        //    var sqlText = @"SELECT [Id],[Address],[DatabaseName],[DatabaseType],[UserName],[Password],[IsMainDB],[VerticalDomainTypeId]FROM[app].[DatabaseConnections]" +
        //                    "where [VerticalDomainTypeId] = (select[VerticalDomainTypeId] from[app].[MarchantVerticalDomains] where[DomainUniqueKey] = @domainId)" +
        //                    "and   [IsMainDB] = 1";
        //    var connectionString = ConfigurationManager.ConnectionStrings["onePOS"].ConnectionString;
        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        var command =
        //            new SqlCommand(sqlText, connection)
        //            {
        //                CommandType = CommandType.Text
        //            };
        //        command.Parameters.Add("@domainId", SqlDbType.NVarChar).Value = domainId;
        //        connection.Open();

        //        var adapter = new SqlDataAdapter(command);
        //        adapter.Fill(ds);
        //    }
        //    return ds.Tables[0];
        //}

        public ActionResult LoadImageForStore()
        {
            string path = null;
            string base64String = null;
            if (Session["StoreId"] != null)
            {
                string storeid = Session["StoreId"].ToString();


                if (storeid == "d73add35-876a-4c82-82f9-9591baf2c20d")
                {
                    path = Server.MapPath("~/Content/assets/images/logo.png");
                }
                else if (storeid == "f575a340-44a8-4f68-b5fc-efba3350a264")
                {
                    path = Server.MapPath("~/Content/StoreLogo/png/Mylapore Logo – 2.png");
                }
                else if (storeid == "2cfb7b87-3e7d-486f-b14a-356730689fbd")
                {
                    path = Server.MapPath("~/Content/StoreLogo/png/Logo.png");
                }
                try
                {
                    using (Image image = Image.FromFile(path))
                    {
                        //Bitmap b = new Bitmap(image);

                        //Image i = resizeImage(b, new Size(134, 30));

                        using (MemoryStream m = new MemoryStream())
                        {
                            image.Save(m, image.RawFormat);
                            byte[] imageBytes = m.ToArray();

                            // Convert byte[] to Base64 String
                            base64String = Convert.ToBase64String(imageBytes);
                            //return base64String;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
            }

            return Json(base64String);

        }

        private static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
        {
            //Get the image current width  
            int sourceWidth = imgToResize.Width;
            //Get the image current height  
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //Calulate  width with new desired size  
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //Calculate height with new desired size  
            nPercentH = ((float)size.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //New Width  
            int destWidth = (int)(sourceWidth * nPercent);
            //New Height  
            int destHeight = (int)(sourceHeight * nPercent);
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // Draw image with new width and height  
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }
    }
}