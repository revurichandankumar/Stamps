using OneposStamps.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
        public ActionResult Index( string storeId = null, int type = 0)
        {
            //TempData["Type"] = type;
            if ( !string.IsNullOrWhiteSpace(storeId))
            {
                //db.Dispose();

                


                var storeInfo = new Store();
                
                switch (type)
                {
                    case 1:
                        return RedirectToAction("Index", "Zone", new { store= storeId });
                        //return RedirectToAction("GetZipDefaultData", "Zone");

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
    }
}