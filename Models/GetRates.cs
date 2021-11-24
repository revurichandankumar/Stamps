using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models
{
    public class GetRates
    {
        //public string AuthenticationId { get; set; }
        //public GetrateRoot getrate { get; set; }
        public RateOptions rate_options { get; set; }
        public Shipment shipment { get; set; }
    }
    public class GetRatesResponse
    {
        public double Amount { get; set; }
        public string Deliverydate { get; set; }
    }
    public class RateOptions
    {
        public List<string> carrier_ids { get; set; }
        public List<string> package_types { get; set; }
        public List<string> service_codes { get; set; }
    }
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
    public class ShipFrom
    {
        public string company_name { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public string city_locality { get; set; }
        public string state_province { get; set; }
        public string postal_code { get; set; }
        public string country_code { get; set; }
        public string address_residential_indicator { get; set; }
    }
    public class Weight
    {
        public double value { get; set; }
        public string unit { get; set; }
    }
    public class Dimensions
    {
        public string unit { get; set; }
        public int length { get; set; }
        public double width { get; set; }
        public int height { get; set; }
    }
    public class Package
    {
        public Weight weight { get; set; }
        public Dimensions dimensions { get; set; }
    }
    public class Shipment
    {
        public string validate_address { get; set; }
        public ShipTo ship_to { get; set; }
        public ShipFrom ship_from { get; set; }
        public List<Package> packages { get; set; }
    }
}