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
}