﻿using OneposStamps.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneposStamps.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Order

        public ActionResult OrderDetails(string StoreId, string DeliverDate = "")
        {
            OrdersDetail od = new OrdersDetail();
            od.StoreId = StoreId;
            if (string.IsNullOrEmpty(DeliverDate))
            {
                return View(od);
            }

            DbDetails dbdetails = db.GetDbDetails(StoreId);
            DataSet ds = db.GetOrders("USP_GetordersShip", StoreId, DeliverDate, dbdetails.Address, dbdetails.Password, dbdetails.DatabaseName, dbdetails.Username);


            List<GetordersData> getorderslist = new List<GetordersData>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                GetordersData value = new GetordersData();
                value.OrderId = (row["OrderId"]).ToString();
                value.OrderDate = (row["OrderDate"]).ToString();
                value.OrderTotal = (row["OrderTotal"]).ToString();
                value.CustomerName = (row["CustomerName"]).ToString();
                value.Qty = Convert.ToDecimal(row["Qty"]);
                getorderslist.Add(value);

            }

            od.GetorderList = getorderslist;

            return View(od);
        }
    }
}