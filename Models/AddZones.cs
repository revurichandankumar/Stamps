using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models
{
    public class AddZones
    {
        public string ZoneId { get; set; }
        public List<CarrierData> CarrierList { get; set; }
        public List<ServicetypeData> ServiceList { get; set; }
        public List<PackageData> PackageList { get; set; }
        public string Store_Id { get; set; }
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
        public string CarrierId { get; set; }
        public string Service_Code { get; set; }
        public bool INHouse { get; set; }
    }
    public class PackageData
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class InsertZones
    {
        public string ZoneId { get; set; }
        public string ZoneName { get; set; }
        public string CarrierId { get; set; }
        public decimal ShipMentFee { get; set; }
        public decimal Weight { get; set; }
        public string ServiceTypeId { get; set; }
        public string PackingId { get; set; }
        public decimal Length { get; set; }
        public decimal Breadth { get; set; }
        public decimal Height { get; set; }
        public string Store_Id { get; set; }
    }
}