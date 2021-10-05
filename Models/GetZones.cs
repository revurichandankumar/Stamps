﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models
{
    public class GetZones
    {
       public List<GetZoneDtsData> GetZoneList { get; set; }
        public string ZoneName { get; set; }
        public decimal Shipmentfee { get; set; }
        public string Carriername { get; set; }

    }
    public class GetZoneDtsData
    {
        public string Id { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string StoreId { get; set; }
    }
}