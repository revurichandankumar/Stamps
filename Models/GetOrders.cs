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
        public DateTime? OrderDate { get; set; }
        public decimal OrderTotal { get; set; }
        public string CustomerName { get; set; }
        public decimal Qty { get; set; }
    }
}