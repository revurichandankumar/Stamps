using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models
{
    public class OrderIdList
    {
        public string StoreId { get; set; }
        public string DeliverDate { get; set; }
        public List<string> OrderIds { get; set; }
    }

    public class OrderIds
    {
        public string OrderId { get; set; }
    }
}