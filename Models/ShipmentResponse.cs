using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models.ShipmentResponse
{
    public class ShipTo
    {
        public string name { get; set; }
        public string phone { get; set; }
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

    public class ShipFrom
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string company_name { get; set; }
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public object address_line3 { get; set; }
        public string city_locality { get; set; }
        public string state_province { get; set; }
        public string postal_code { get; set; }
        public string country_code { get; set; }
        public string address_residential_indicator { get; set; }
    }

    public class ReturnTo
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string company_name { get; set; }
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public object address_line3 { get; set; }
        public string city_locality { get; set; }
        public string state_province { get; set; }
        public string postal_code { get; set; }
        public string country_code { get; set; }
        public string address_residential_indicator { get; set; }
    }

    public class Customs
    {
        public string contents { get; set; }
        public List<object> customs_items { get; set; }
        public string non_delivery { get; set; }
    }

    public class AdvancedOptions
    {
        public object bill_to_account { get; set; }
        public object bill_to_country_code { get; set; }
        public object bill_to_party { get; set; }
        public object bill_to_postal_code { get; set; }
        public bool contains_alcohol { get; set; }
        public bool delivered_duty_paid { get; set; }
        public bool non_machinable { get; set; }
        public bool saturday_delivery { get; set; }
        public bool dry_ice { get; set; }
        public object dry_ice_weight { get; set; }
        public object fedex_freight { get; set; }
        public object freight_class { get; set; }
        public object custom_field1 { get; set; }
        public object custom_field2 { get; set; }
        public object custom_field3 { get; set; }
        public object collect_on_delivery { get; set; }
        public object return_pickup_attempts { get; set; }
    }

    public class Weight
    {
        public double value { get; set; }
        public string unit { get; set; }
    }

    public class Dimensions
    {
        public string unit { get; set; }
        public double length { get; set; }
        public double width { get; set; }
        public double height { get; set; }
    }

    public class InsuredValue
    {
        public string currency { get; set; }
        public double amount { get; set; }
    }

    public class LabelMessages
    {
        public object reference1 { get; set; }
        public object reference2 { get; set; }
        public object reference3 { get; set; }
    }

    public class Package
    {
        public string package_code { get; set; }
        public Weight weight { get; set; }
        public Dimensions dimensions { get; set; }
        public InsuredValue insured_value { get; set; }
        public LabelMessages label_messages { get; set; }
        public object external_package_id { get; set; }
    }

    public class TotalWeight
    {
        public double value { get; set; }
        public string unit { get; set; }
    }

    public class Shipment
    {
        public List<object> errors { get; set; }
        public object address_validation { get; set; }
        public string shipment_id { get; set; }
        public string carrier_id { get; set; }
        public string service_code { get; set; }
        public string external_shipment_id { get; set; }
        public DateTime ship_date { get; set; }
        public DateTime created_at { get; set; }
        public DateTime modified_at { get; set; }
        public string shipment_status { get; set; }
        public ShipTo ship_to { get; set; }
        public ShipFrom ship_from { get; set; }
        public string warehouse_id { get; set; }
        public ReturnTo return_to { get; set; }
        public string confirmation { get; set; }
        public Customs customs { get; set; }
        public object external_order_id { get; set; }
        public object order_source_code { get; set; }
        public AdvancedOptions advanced_options { get; set; }
        public string insurance_provider { get; set; }
        public List<object> tags { get; set; }
        public List<Package> packages { get; set; }
        public TotalWeight total_weight { get; set; }
        public List<object> items { get; set; }
    }

    public class Root
    {
        public bool has_errors { get; set; }
        public List<Shipment> shipments { get; set; }
    }

    public class ShipmentId
    {
        public List<string> ShipmentIds { get; set; }
    }
}