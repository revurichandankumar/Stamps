using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models
{
    public class GetRates
    {
        public string AuthenticationId { get; set; }
        public string fromZipcode { get; set; }
        public string toZipcode { get; set; }
        public decimal WeightLb { get; set; }
        public decimal WeightOz { get; set; }
        public string PackageType { get; set; }
        public string shipdate { get; set; }
        public string servicetype { get; set; }
    }
    public class GetRatesResponse
    {
        public string Amount { get; set; }
    }
}