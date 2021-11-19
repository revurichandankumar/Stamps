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
}