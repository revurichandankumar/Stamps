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
                db.Dispose();
               // DataTable dbconnection = GetDatabaseConnectionValues(domainId);
              
                    //db = Helper.SqlDBConnection.GetInstance(serverName: Convert.ToString(dbconnection.Rows[0]["Address"]), dbName: Convert.ToString(dbconnection.Rows[0]["DatabaseName"]), userName: Convert.ToString(dbconnection.Rows[0]["UserName"]), password: Convert.ToString(dbconnection.Rows[0]["Password"]));
                


                var storeInfo = new Store();
                //DataTable dt = db.GetDataTable(string.Format("SELECT Name,Id,Address1,Address2,VerticalDomainID FROM [dbo].[Stores] where Id='{0}'", storeId));
                //if (dt.Rows.Count > 0)
                //{
                //    storeInfo.Id = new Guid(dt.Rows[0]["Id"].ToString());
                //    storeInfo.Name = (string)dt.Rows[0]["Name"];
                //    storeInfo.Address1 = dt.Rows[0]["Address1"] == DBNull.Value ? "" : (string)dt.Rows[0]["Address1"];
                //    storeInfo.Address2 = dt.Rows[0]["Address2"] == DBNull.Value ? "" : (string)dt.Rows[0]["Address2"];
                //    storeInfo.VerticalDomainID = dt.Rows[0]["VerticalDomainID"] == DBNull.Value ? "" : (string)dt.Rows[0]["VerticalDomainID"];
                //}
                //Session["StoreInfo"] = storeInfo;
                switch (type)
                {
                    case 1:
                        return RedirectToAction("Index", "Zone");
                    
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