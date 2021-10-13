using Newtonsoft.Json;
using OneposStamps.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace OneposStamps.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Order

        public ActionResult OrderDetails(string StoreId, string DeliverDate = "")
        {
            OrdersDetail od = new OrdersDetail();
            od.StoreId = StoreId;
            if (string.IsNullOrEmpty(DeliverDate))
            {
                return View(od);
            }

            DbDetails dbdetails = db.GetDbDetails(StoreId);
            DataSet ds = db.GetOrders("USP_GetordersShip", StoreId, DeliverDate, dbdetails.Address, dbdetails.Password, dbdetails.DatabaseName, dbdetails.Username);


            List<GetordersData> getorderslist = new List<GetordersData>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                GetordersData value = new GetordersData();
                value.OrderId = (row["OrderId"]).ToString();
                value.OrderDate = (row["OrderDate"]).ToString();
                value.OrderTotal = (row["OrderTotal"]).ToString();
                value.CustomerName = (row["CustomerName"]).ToString();
                value.Qty = Convert.ToDecimal(row["Qty"]);
                getorderslist.Add(value);

            }

            od.GetorderList = getorderslist;

            return View(od);
        }
        public ActionResult OrderShipmentDetails(string StoreId, string OrderId = "")
        {
            OrderDetails od = new OrderDetails();
            DbDetails dbdetails = db.GetDbDetails(StoreId);
            DataSet ds = db.GetOrders("USP_GetordersShip", StoreId, OrderId, dbdetails.Address, dbdetails.Password, dbdetails.DatabaseName, dbdetails.Username);
            OrderDetail orderdetais = new OrderDetail();
            foreach (DataRow row in ds.Tables[0].Rows)
            {

                orderdetais.storeName = (row["StoreName"]).ToString();
                orderdetais.orderDate = (row["OrderDate"]).ToString();
                orderdetais.paidDate = (row["PaidDate"]).ToString();
                orderdetais.shippingPaid = 0;
                orderdetais.taxPaid = Convert.ToDecimal(row["TaxPaid"]);
                orderdetais.productTotal = Convert.ToDecimal(row["ProductTotal"]);
                orderdetais.totalOrder = Convert.ToDecimal(row["TotalOrder"]);
                orderdetais.totalPaid = Convert.ToDecimal(row["TotalPaid"]);
                orderdetais.holdUntil = "";


            }
            od.orderSummary = orderdetais;
            ShippingDetail ShippingDetails = new ShippingDetail();
            foreach (DataRow row in ds.Tables[1].Rows)
            {

                ShippingDetails.name = (row["DeliveryName"]).ToString();
                ShippingDetails.address = (row["Adress"]).ToString();
                ShippingDetails.phoneNo = (row["Phone"]).ToString();
                ShippingDetails.city = (row["City"]).ToString();
                ShippingDetails.state = (row["State"]).ToString();
                ShippingDetails.zipcode = (row["ZipCode"]).ToString();
                ShippingDetails.country = (row["Country"]).ToString();
                ShippingDetails.email = (row["EmailAddress"]).ToString();
                ShippingDetails.landMark = (row["LandMark"]).ToString();
                ShippingDetails.street = (row["Street"]).ToString();


            }
            od.BuyersDetails = ShippingDetails;
            List<OrderItemDetail> orderdetails = new List<OrderItemDetail>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                OrderItemDetail value = new OrderItemDetail();
                value.name = (row["ProductName"]).ToString();
                value.sku = (row["SKU"]).ToString();
                value.Itemlist = (row["ProductMetaDetails"]).ToString();

                orderdetails.Add(value);

            }
            od.OrderItemDetails = orderdetails;


            return View();
        }
        public ActionResult CheckAddress(string StoreId, AddressVerifyRequest Addressrequest = null)
        {
            DbDetails dbdetails = db.GetDbDetails(StoreId);

            
            AuthenticateUserResponse response = GetAuthentication(dbdetails, "AuthenticateUser");
            Addressrequest.AuthenticationId = response.Authenticator;
            AddressVerifyResponse Addressresponse= CleanseAddress(Addressrequest, "CleanseAddress");

            return View();
        }
        public ActionResult GetShipRates(string StoreId, GetRates getrates = null)
        {
            DbDetails dbdetails = db.GetDbDetails(StoreId);
            getrates.fromZipcode = "92128";
            getrates.toZipcode = "90245";
            getrates.PackageType = "Package";
            getrates.shipdate = "2021-10-09";
            getrates.WeightLb = 1;
            getrates.WeightOz = 0;
            getrates.servicetype = "US-PM";
            AuthenticateUserResponse response = GetAuthentication(dbdetails, "AuthenticateUser");
            getrates.AuthenticationId = response.Authenticator;
            GetRatesResponse val = GetRates(getrates, "GetRates");
            return View();
        }
        public ActionResult GetLabels(string StoreId, CreateLabelRequest getlabel = null)
        {
            DbDetails dbdetails = db.GetDbDetails(StoreId);

            getlabel.IntegratorTxID = "1234567890ABCGHI";
            getlabel.FromFullName = "SWSIM API";
            getlabel.Fromaddress = "1990 E GRAND AVE";
            getlabel.FromCity = "EL SEGUNDO";
            getlabel.FromState = "CA";
            getlabel.FromZIPCode = "90245";
            getlabel.FromCountry = "US";
            getlabel.ToFullName = "SWSIM API";
            getlabel.Toaddress = "1990 E GRAND AVE";
            getlabel.ToCity = "EL SEGUNDO";
            getlabel.ToState = "CA";
            getlabel.ToZIPCode = "90245";
            getlabel.ToCountry = "US";
            getlabel.WeightLb = 0;
            getlabel.WeightOz = 1;
            getlabel.PackageType = "Package";
            getlabel.shipdate = "2021-10-10";
            getlabel.Length = "1";
            getlabel.Width = "1";
            getlabel.Height = "1";
            getlabel.ServiceType = "US-PM";// "US-FC";
            getlabel.Amount = "7.36";
            AuthenticateUserResponse response = GetAuthentication(dbdetails, "AuthenticateUser");
            getlabel.AuthenticationId = response.Authenticator;
            CreateLabelResponse res = GetLabel(getlabel, "CreateIndicium");
            return View();
        }
      

        public AuthenticateUserResponse GetAuthentication(DbDetails dbdetails,string Apiname)
        {
            AuthenticateUserResponse Responses = new AuthenticateUserResponse();
            //Calling CreateSOAPWebRequest method  
            HttpWebRequest request = CreateSOAPWebRequest(Apiname);

            XmlDocument SOAPReqBody = new XmlDocument();
            //SOAP Body Request  
            SOAPReqBody.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>  
            <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""
                xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">  
             <soap:Body> 
                <AuthenticateUser xmlns=""http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111"">  
                <Credentials>
                  <IntegrationID>" + dbdetails.IntegrationId + @"</IntegrationID>  
                  <Username>" + dbdetails.StampsUserName + @"</Username> 
                  <Password>" + dbdetails.StampsUserPassword + @"</Password>
                 </Credentials>
                </AuthenticateUser>
              </soap:Body>  
            </soap:Envelope>");


            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }
            //Geting response from request  
            using (WebResponse Serviceres = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                {
                    //reading stream  
                    var ServiceResult = rd.ReadToEnd();
                    //var otp = JsonConvert.DeserializeObject<AuthenticateUser>(ServiceResult);
                    XDocument doc = XDocument.Parse(ServiceResult);
                    XNamespace ns = "http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111";
                    IEnumerable<XElement> responses = doc.Descendants(ns + "AuthenticateUserResponse");
                    foreach (XElement response in responses)
                    {
                        var value = (string)response.Element(ns + "Authenticator");
                        Responses.Authenticator = value.Replace("&", "&amp;");

                    }

                }
            }

                return Responses;

        }

       
        public CreateLabelResponse GetLabel(CreateLabelRequest getlabel,string Apiname)
        {
            CreateLabelResponse Responses = new CreateLabelResponse();
            //Calling CreateSOAPWebRequest method  
            HttpWebRequest request = CreateSOAPWebRequest(Apiname);

            XmlDocument SOAPReqBody = new XmlDocument();
            //SOAP Body Request  
            SOAPReqBody.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>  
            <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""
                xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">  
             <soap:Body>               
             <CreateIndicium xmlns=""http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111"">
            <Authenticator>" + getlabel.AuthenticationId + @"</Authenticator>                                                                                                           
            <IntegratorTxID>" + getlabel.IntegratorTxID + @"</IntegratorTxID>                                                                                                           
            <Rate>                                                                                                              
                <From>                                                                                                            
                    <FullName>" + getlabel.FromFullName + @"</FullName>                                                                                                                
                    <Address1>" + getlabel.Fromaddress + @"</Address1>
                    <City>" + getlabel.FromCity + @"</City>
                    <State>" + getlabel.FromCity + @"</State>
                    <ZIPCode>" + getlabel.FromZIPCode + @"</ZIPCode>
                    <Country>" + getlabel.FromCountry + @"</Country>
                </From>
                <To>
                    <FullName>" + getlabel.ToFullName + @"</FullName>                                                                                                                          
                    <Address1>" + getlabel.Toaddress + @"</Address1>                                                                                                                              
                    <City>" + getlabel.ToCity + @"</City>                                                                                                                                 
                    <State>" + getlabel.ToState + @"</State>                                                                                                                                  
                    <ZIPCode>" + getlabel.ToZIPCode + @"</ZIPCode>                                                                                                                                                                                                                                                                  
                    <Country>" + getlabel.ToCountry + @"</Country>                                                                                                                                  
                </To>                                                                                                                                                                                                                                                    
                <Amount>" + getlabel.Amount + @"</Amount>                                                                                                                                                                                                                                                             
                <ServiceType>" + getlabel.ServiceType + @"</ServiceType>                                                                                                                                                                                                                                                            
                <WeightLb>" + getlabel.WeightLb + @"</WeightLb>                                                                                                                                
                <WeightOz>" + getlabel.WeightOz + @"</WeightOz>                                                                                                                               
                <PackageType>" + getlabel.PackageType + @"</PackageType>                                                                                                                               
                <Length>" + getlabel.Length + @"</Length>                                                                                                                        
                <Width>" + getlabel.Width + @"</Width>                                                                                                                            
                <Height>" + getlabel.Height + @"</Height>                                                                                                                            
                <ShipDate>" + getlabel.shipdate + @"</ShipDate>                                                                                                                            
                </Rate>                                                                                                                              
                </CreateIndicium>                                                                                                                                
                </soap:Body>  
                </soap:Envelope>");


            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }
            //Geting response from request  
            using (WebResponse Serviceres = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                {
                    //reading stream  
                    var ServiceResult = rd.ReadToEnd();
                  
                    //var otp = JsonConvert.DeserializeObject<AuthenticateUser>(ServiceResult);
                    XDocument doc = XDocument.Parse(ServiceResult);
                    XNamespace ns = "http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111";
                    IEnumerable<XElement> responses = doc.Descendants(ns + "CreateIndiciumResponse");
                    IEnumerable<XElement> responses1 = responses.Descendants(ns + "Rate");
                    IEnumerable<XElement> responses2 = responses1.Descendants(ns + "From");
                    //IEnumerable<XElement> responses = doc.Descendants(ns + "CreateIndiciumResponse");


                    foreach (XElement response in responses)
                    {

                        Responses.Url = (string)response.Element(ns + "URL");
                        Responses.TrackingNumber = (string)response.Element(ns + "TrackingNumber");

                    }
                    foreach(XElement response in responses1)
                    {
                        Responses.ServiceType = (string)response.Element(ns + "ServiceType");
                        Responses.DeliveryDate = (string)response.Element(ns + "DeliveryDate");
                    }

                }

            }
            return Responses;
        }
        public GetRatesResponse GetRates(GetRates getrates,string Apiname)
        {
            GetRatesResponse Responses = new GetRatesResponse();
            //Calling CreateSOAPWebRequest method  
            HttpWebRequest request = CreateSOAPWebRequest(Apiname); 

            XmlDocument SOAPReqBody = new XmlDocument();
            //SOAP Body Request  
            SOAPReqBody.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>  
            <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""
                xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">  
             <soap:Body>               
            <GetRates xmlns=""http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111"">
            <Authenticator>" + getrates.AuthenticationId + @"</Authenticator>
            <Rate >
                <From>
                    <ZIPCode>" + getrates.fromZipcode + @"</ZIPCode>
                </From>
                <To>
                    <ZIPCode>" + getrates.toZipcode + @"</ZIPCode>
                </To>
                <ServiceType>" + getrates.servicetype + @"</ServiceType>
               <WeightLb>" + getrates.WeightLb + @"</WeightLb>
                <WeightOz>" + getrates.WeightOz + @"</WeightOz>
                <PackageType>" + getrates.PackageType + @"</PackageType>
                <ShipDate>" + getrates.shipdate + @"</ShipDate>
            </Rate >
            </GetRates >
               </soap:Body>  
            </soap:Envelope>");


            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }
            //Geting response from request  
            using (WebResponse Serviceres = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                {
                    //reading stream  
                    var ServiceResult = rd.ReadToEnd();
                    //var otp = JsonConvert.DeserializeObject<AuthenticateUser>(ServiceResult);
                    XDocument doc = XDocument.Parse(ServiceResult);
                    XNamespace ns = "http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111";
                    IEnumerable<XElement> responses = doc.Descendants(ns + "GetRatesResponse");

                    IEnumerable<XElement> responses1 = responses.Descendants(ns + "Rates");
                    IEnumerable<XElement> responses2 = responses1.Descendants(ns + "Rate");
                    bool amountvalue = true;
                    foreach (XElement response in responses2)
                    {
                        if(amountvalue)
                        {
                            Responses.Amount = (string)response.Element(ns + "Amount");
                            amountvalue = false;
                        }
                       
                    }
                }

            }
            return Responses;
        }
        public AddressVerifyResponse CleanseAddress(AddressVerifyRequest Addressrequest,string ApiName)
        {
            AddressVerifyResponse Responses = new AddressVerifyResponse();

            //Calling CreateSOAPWebRequest method  
            HttpWebRequest request = CreateSOAPWebRequest(ApiName);

            XmlDocument SOAPReqBody = new XmlDocument();
            //SOAP Body Request  
            SOAPReqBody.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>  
            <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""
                xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">  
             <soap:Body> 
                <CleanseAddress xmlns=""http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111""> 
                <Authenticator>" + Addressrequest.AuthenticationId + @"</Authenticator>
                 <Address>
                <FullName>" + Addressrequest.name + @"</FullName>
                <Address1>" + Addressrequest.address1 + @"</Address1>
                <City>" + Addressrequest.city + @"</City>
                <State>" + Addressrequest.state + @"</State>
                <ZIPCode>" + Addressrequest.zipcode + @"</ZIPCode>
                </Address>             
                </CleanseAddress>
              </soap:Body>  
            </soap:Envelope>");


            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }
            //Geting response from request  
            using (WebResponse Serviceres = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                {
                    //reading stream  
                    var ServiceResult = rd.ReadToEnd();
                    //var otp = JsonConvert.DeserializeObject<AuthenticateUser>(ServiceResult);
                    XDocument doc = XDocument.Parse(ServiceResult);
                    XNamespace ns = "http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111";
                    IEnumerable<XElement> responses = doc.Descendants(ns + "CleanseAddressResponse");

                    foreach (XElement response in responses)
                    {

                        Responses.AddressMatched = (string)response.Element(ns + "AddressMatch");

                    }

                }
            }
            return Responses;
        }
        public HttpWebRequest CreateSOAPWebRequest( string Apiname)
        {
            //Making Web Request  
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"https://swsim.testing.stamps.com/swsim/swsimv111.asmx");
            //SOAPAction  
            Req.Headers.Add(@"SOAPAction:http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111/"+ Apiname +"");//AuthenticateUser
            //Content_type  
            // Req.ContentLength = "length";
            Req.ContentType = "text/xml;charset=\"utf-8\"";
            Req.Accept = "text/xml";
            //HTTP method  
            Req.Method = "POST";
            Req.Host = "swsim.testing.stamps.com";

            //return HttpWebRequest  
            return Req;
        }
       
    }
}