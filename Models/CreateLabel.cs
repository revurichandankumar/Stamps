using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models
{
    public class CreateLabelRequest
    {
        public string AuthenticationId { get; set; }
        public string IntegratorTxID { get; set; }
        public string FromFullName { get; set; }
        public string Fromaddress { get; set; }
        public string FromCity { get; set; }
        public string FromState { get; set; }
        public string FromZIPCode { get; set; }
        public string FromCountry { get; set; }
        public string ToFullName { get; set; }
        public string Toaddress { get; set; }
        public string ToCity { get; set; }
        public string ToState { get; set; }
        public string ToZIPCode { get; set; }
        public string ToCountry { get; set; }
        public decimal WeightLb { get; set; }
        public decimal WeightOz { get; set; }
        public string PackageType { get; set; }
        public string shipdate { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string ServiceType { get; set; }
        public string Amount { get; set; }

    }
    public class CreateLabelResponse
    {
        public string Url { get; set; }
        public string DeliveryDate { get; set; }
        public string ServiceType { get; set; }
        public string TrackingNumber { get; set; }

    }
}

               
      
                
 
          
             
