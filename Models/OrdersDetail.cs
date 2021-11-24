using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models
{
    public class OrdersDetail
    {
        public string DeliverDate { get; set; }
        public string StoreId { get; set; }
        public List<GetordersData> OrderList { get; set; }
        public List<Zonelist> ZoneList { get; set; }
    }
}