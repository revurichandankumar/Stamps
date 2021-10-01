using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models
{
    public class GetZones
    {
       public List<GetZoneData> GetZoneList { get; set; }
    }
    public class GetZoneData
    {
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}