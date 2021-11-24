using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models
{
    public class AddressVerifyRequest
    {
        public string AuthenticationId { get; set; }
        public string name { get; set; }
        public string address1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }

    }
    public class AddressVerifyResponse
    {
        public string AddressMatched { get; set; }
    }

    public class ShipengineAddressRequest
    {
        public string name { get; set; }
        public string address_line1 { get; set; }
        public string city_locality { get; set; }
        public string state_province { get; set; }
        public string postal_code { get; set; }
        public string country_code { get; set; }
    }

    //public class ShipengineAddressResponse
    //{
    //    public List<Root> Shippingdetails { get; set; }
    //}
    public class OriginalAddress
    {
        public string name { get; set; }
        public object phone { get; set; }
        public object company_name { get; set; }
        public string address_line1 { get; set; }
        public object address_line2 { get; set; }
        public object address_line3 { get; set; }
        public string city_locality { get; set; }
        public string state_province { get; set; }
        public string postal_code { get; set; }
        public string country_code { get; set; }
        public string address_residential_indicator { get; set; }
    }
    public class MatchedAddress
    {
        public string name { get; set; }
        public object phone { get; set; }
        public object company_name { get; set; }
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public object address_line3 { get; set; }
        public string city_locality { get; set; }
        public string state_province { get; set; }
        public string postal_code { get; set; }
        public string country_code { get; set; }
        public string address_residential_indicator { get; set; }
    }
    public class ShipengineAddressResponse
    {
        public string status { get; set; }
        public OriginalAddress original_address { get; set; }
        public MatchedAddress matched_address { get; set; }
        public List<object> messages { get; set; }
    }
}