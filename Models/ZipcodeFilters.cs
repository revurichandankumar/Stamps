using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models
{
    public class ZipcodeFilters
    {
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string ZipCode { get; set; }
        public string StoreId { get; set; }
        public string ZoneId { get; set; }
    }
}