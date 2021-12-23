using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models.ShipmentRequest
{
    public class ShipTo
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string address_line1 { get; set; }
        public string city_locality { get; set; }
        public string state_province { get; set; }
        public string postal_code { get; set; }
        public string country_code { get; set; }
        public string address_residential_indicator { get; set; }
    }

    public class AdvancedOptions
    {
    }

    public class Weight
    {
        public double value { get; set; }
        public string unit { get; set; }
    }

    public class Package
    {
        public Weight weight { get; set; }
    }

    public class Shipment
    {
        public string warehouse_id { get; set; }
        public string validate_address { get; set; }
        public string service_code { get; set; }
        public string external_shipment_id { get; set; }
        public ShipTo ship_to { get; set; }
        public string confirmation { get; set; }
        public AdvancedOptions advanced_options { get; set; }
        public string insurance_provider { get; set; }
        public List<object> tags { get; set; }
        public List<Package> packages { get; set; }
    }

    public class Root
    {
        public List<Shipment> shipments { get; set; }
    }

    public class ShipmentDetails
    {
        public string OrderId { get; set; }
        public string ShipmentId { get; set; }
    }

}