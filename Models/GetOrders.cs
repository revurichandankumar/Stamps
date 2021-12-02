using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models
{
    public class GetOrders
    {
        public List<GetordersData> GetorderList { get; set; }
    }
    public class GetordersData
    {
        public string OrderId { get; set; }
        public string ZoneId { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal OrderTotal { get; set; }
        public string CustomerName { get; set; }
        public decimal Qty { get; set; }
        public string storeName { get; set; }
        public string StoreAddress { get; set; }
        public string StoreCity { get; set; }
        public string StoreState { get; set; }
        public string StoreCountry { get; set; }
        public string StoreZipcode { get; set; }
        public string StorePhoneNo { get; set; }
        public string name { get; set; }
        public string phoneNo { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zipcode { get; set; }
        public string email { get; set; }
        public string landMark { get; set; }
        public string street { get; set; }
    }

    public class OrderIdList
    {
        public List<OrderIds> OrderIds { get; set; }
        public string StoreId { get; set; }
        public string DeliverDate { get; set; }
    }

    public class OrderIds
    {
        public string OrderId { get; set; }
    }


}