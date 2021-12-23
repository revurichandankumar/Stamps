using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models
{
    public class GetZonesResponse
    {
        public string ZoneId { get; set; }
        public decimal Shipmentfee { get; set; }
        public string Zonename { get; set; }
    }
    public class GetZonesAvailabiltyResponse
    {
        public string ZoneId { get; set; }
        public decimal Shipmentfee { get; set; }
        public string Zonename { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public bool Box { get; set; }
        public decimal Count { get; set; }
        public decimal Amount { get; set; }
        public List<DeliveryDate> Dates { get; set; }
    }

    public class DeliveryDate
    {
        public string Date { get; set; }
        public string Reason { get; set; }
        public string IsActive { get; set; }
    }
}