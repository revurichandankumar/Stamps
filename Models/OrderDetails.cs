using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models
{
    public class OrderDetails
    {
        public OrderDetail orderSummary { get; set; }
        public ShippingDetail BuyersDetails { get; set; }
        public List<OrderItemDetail> OrderItemDetails { get; set; }
        public ShippingInfo ShippingInfodetails { get; set; }
        public List<CarrierData> CarrierList { get; set; }
        public List<ServicetypeData> ServiceList { get; set; }
        public List<PackageData> PackageList { get; set; }
        public List<Zonelist> ZoneList { get; set; }
        public string StoreId { get; set; }
        public string OrderId { get; set; }
        public string SelectedZoneId { get; set; }
        public string SelectedServiceId { get; set; }
        public string DeliverDate { get; set; }
        public string LogoBase64String { get; set; }
        public string BarcodeBase64String { get; set; }
        public string AddressVerified { get; set; }
    }
    public class OrderDetail
    {
        public string storeName { get; set; }
        public string StoreAddress { get; set; }
        public string StoreCity { get; set; }
        public string StoreState { get; set; }
        public string StoreCountry { get; set; }
        public string StoreZipcode { get; set; }
        public string StorePhoneNo { get; set; }
        public DateTime? orderDate { get; set; }
        public DateTime? paidDate { get; set; }
        public string holdUntil { get; set; }
        public decimal productTotal { get; set; }
        public decimal shippingPaid { get; set; }
        public decimal taxPaid { get; set; }
        public decimal totalOrder { get; set; }
        public decimal totalPaid { get; set; }
        public string TransactionId { get; set; }
        public string OrderNotes { get; set; }

    }
    public class ShippingDetail
    {
        public string name { get; set; }
        public string phoneNo { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zipcode { get; set; }
        public string email { get; set; }
        public string landMark { get; set; }
        public string street { get; set; }
    }
    public class OrderItemDetail
    {
        public string name { get; set; }
        public string sku { get; set; }
        public string Itemlist { get; set; }
        public decimal UnitPrice { get; set; }
        public string OrderQty { get; set; }
        public decimal TotalPrice { get; set; }
    }
    //public class AddonList
    //{
    //    public string name { get; set; }
    //}
    //public class OrderNotes
    //{

    //}
    public class ShippingInfo
    {
        public string requested { get; set; }
        public string shipfrom { get; set; }
        public string serviceName { get; set; }
        public string packageName { get; set; }
        public decimal length { get; set; }
        public decimal breadth { get; set; }
        public decimal height { get; set; }
        public string confirm { get; set; }
        public string insurance { get; set; }
        public decimal rate { get; set; }
        public string deliverytime { get; set; }
    }
}