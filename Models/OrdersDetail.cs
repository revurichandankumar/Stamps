using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models
{
    public class OrdersDetail
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string StoreId { get; set; }
        public List<GetordersData> GetorderList { get; set; }
    }
}