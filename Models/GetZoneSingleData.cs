using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models
{
    public class GetZoneSingleData
    {
        public string Store_Id { get; set; }
        public bool Isdeleted { get; set; }
        public string  ZoneName { get; set; }
        public string ZoneId { get; set; }
        public string CarrierId { get; set; }
        public decimal ShipmentFee { get; set; }     
        public string PackageId { get; set; }
        public decimal Weight { get; set; }
        public string ServiceTypeId { get; set; }
        public decimal Length { get; set; }
        public decimal Breadth { get; set; }
        public decimal Height { get; set; }
    }
}