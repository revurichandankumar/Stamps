using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models
{
    public class Zones
    {
       public List<Zonelist> ZoneList { get; set; }
        public string StoreId { get; set; }
    }
    public class Zonelist
    {
        public string ZoneId { get; set; }
        public string ZoneName { get; set; }
        public string Statecount { get; set; }
        public string Citycount { get; set; }
        public string Zipcount { get; set; }
        public decimal Shipmentfee { get; set; }
        public string Carrier { get; set; }
        
    }

}