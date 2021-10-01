using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models
{
    public class AddZones
    {
        public List<CarrierData> CarrierList { get; set; }
        public List<ServicetypeData> ServiceList { get; set; }
        public List<PackageData> PackageList { get; set; }
    }
    public class CarrierData
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class ServicetypeData
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class PackageData
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}