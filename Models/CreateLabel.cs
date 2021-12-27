using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models.CreateLabelRequest
{
    public class CreateLabelRequest
    {
        //public string AuthenticationId { get; set; }
        //public string IntegratorTxID { get; set; }
        //public string FromFullName { get; set; }
        //public string Fromaddress { get; set; }
        //public string FromCity { get; set; }
        //public string FromState { get; set; }
        //public string FromZIPCode { get; set; }
        //public string FromCountry { get; set; }
        //public string ToFullName { get; set; }
        //public string Toaddress { get; set; }
        //public string ToCity { get; set; }
        //public string ToState { get; set; }
        //public string ToZIPCode { get; set; }
        //public string ToCountry { get; set; }
        //public decimal WeightLb { get; set; }
        //public decimal WeightOz { get; set; }
        //public string PackageType { get; set; }
        //public string shipdate { get; set; }
        //public string Length { get; set; }
        //public string Width { get; set; }
        //public string Height { get; set; }
        //public string ServiceType { get; set; }
        //public string Amount { get; set; }
        public string ZoneName { get; set; }
        public string OrderId { get; set; }        
        public string ZoneId { get; set; }        
        public string LabelCode { get; set; }        
        public string logoBase64String { get; set; }        
        public string barcodeBase64String { get; set; }        
        public string TodayDate { get; set; }        
        public string ShipmentDate { get; set; }        
        public Shipment shipment { get; set; }
        public List<OrderItemDetail> orderItemDetail { get; set; }
        public List<List<OrderItemDetail>> OrderItemDetailsList { get; set; }
    }
    public class Shipment
    {
        public string service_code { get; set; }
        public ShipTo ship_to { get; set; }
        public ShipFrom ship_from { get; set; }
        public List<Package> packages { get; set; }
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
    public class CreateLabelResponse
    {
        public string Url { get; set; }
        public string DeliveryDate { get; set; }
        public string ShipDate { get; set; }
        public string TrackingNumber { get; set; }
        public string ServiceType { get; set; }
        public string ZoneName { get; set; }
        public List<ShippingDetailsByOrder> shippingDetailsByOrdersList { get; set; } 
    }

    public class ShippingDetailsByOrder
    {
        public string ShipDate { get; set; }
        public string TrackingNumber { get; set; }
        public string ServiceType { get; set; }
    }

    public  class CreateMultipleLabelRequest
    {
        public List<CreateLabelRequest> createLabelRequest { get; set; }
        public string logoBase64String { get; set; }
        public string ShipmentDate { get; set; }
    }
}