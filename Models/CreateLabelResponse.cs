using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models.CreateLabelResponse
{
    public class ShipmentCost
    {
        public string currency { get; set; }
        public double amount { get; set; }
    }

    public class InsuranceCost
    {
        public string currency { get; set; }
        public double amount { get; set; }
    }

    public class LabelDownload
    {
        public string pdf { get; set; }
        public string png { get; set; }
        public string zpl { get; set; }
        public string href { get; set; }
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
        public int package_id { get; set; }
        public string package_code { get; set; }
        public Weight weight { get; set; }
        public Dimensions dimensions { get; set; }
        public InsuredValue insured_value { get; set; }
        public string tracking_number { get; set; }
        public LabelDownload label_download { get; set; }
        public LabelMessages label_messages { get; set; }
        public object external_package_id { get; set; }
        public int sequence { get; set; }
    }

    public class Root
    {
        public string label_id { get; set; }
        public string status { get; set; }
        public string shipment_id { get; set; }
        public DateTime ship_date { get; set; }
        public DateTime created_at { get; set; }
        public ShipmentCost shipment_cost { get; set; }
        public InsuranceCost insurance_cost { get; set; }
        public string tracking_number { get; set; }
        public bool is_return_label { get; set; }
        public object rma_number { get; set; }
        public bool is_international { get; set; }
        public string batch_id { get; set; }
        public string carrier_id { get; set; }
        public string service_code { get; set; }
        public string package_code { get; set; }
        public bool voided { get; set; }
        public object voided_at { get; set; }
        public string label_format { get; set; }
        public string display_scheme { get; set; }
        public string label_layout { get; set; }
        public bool trackable { get; set; }
        public object label_image_id { get; set; }
        public string carrier_code { get; set; }
        public string tracking_status { get; set; }
        public LabelDownload label_download { get; set; }
        public object form_download { get; set; }
        public object insurance_claim { get; set; }
        public List<Package> packages { get; set; }
        public string charge_event { get; set; }
    }
   
}