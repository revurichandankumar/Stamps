using BarcodeLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneposStamps.Models;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
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

        public ActionResult OrderDetails(string StoreId, string DeliverDate = "", string redirectedDeliverDate = "")
        {
            OrdersDetail od = new OrdersDetail();
            od.StoreId = StoreId;
            od.DeliverDate = DeliverDate;
            if (string.IsNullOrEmpty(DeliverDate) && string.IsNullOrEmpty(redirectedDeliverDate))
            {
                od.DeliverDate = redirectedDeliverDate;
                return View(od);
            }

            if (string.IsNullOrEmpty(DeliverDate) && !string.IsNullOrEmpty(redirectedDeliverDate))
            {
                od.DeliverDate = redirectedDeliverDate;
                return View(od);
            }

            DbDetails dbdetails = db.GetDbDetails(StoreId);

            DateTime? deliverdate = null;
            try
            {
                deliverdate = DateTime.ParseExact(DeliverDate, "MM/dd/yyyy", null);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            string deldate = null;
            if (deliverdate != null)
            {
                deldate = deliverdate.Value.ToString("yyyy-MM-dd");
            }

            DataSet ds = db.GetOrders("USP_GetordersShip", StoreId, deldate, dbdetails.Address, dbdetails.Password, dbdetails.DatabaseName, dbdetails.Username);


            List<GetordersData> getorderslist = new List<GetordersData>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                GetordersData value = new GetordersData();
                value.OrderId = (row["OrderId"]).ToString();
                value.OrderDate = (row["OrderDate"]) != null ? Convert.ToDateTime(row["OrderDate"]) : (DateTime?)null;
                value.OrderTotal = (row["OrderTotal"]) != null ? Convert.ToDecimal(row["OrderTotal"]) : 0;
                value.CustomerName = (row["CustomerName"]).ToString();
                value.Qty = Convert.ToDecimal(row["Qty"]);
                getorderslist.Add(value);

            }

            od.OrderList = getorderslist;

            return PartialView("_OrderDetails", od);
        }

        public ActionResult OrderShipmentDetails(string StoreId, string OrderId = "", string DeliverDate = null)
        {
            OrderDetails od = new OrderDetails();
            DbDetails dbdetails = db.GetDbDetails(StoreId);
            od.StoreId = StoreId;
            od.OrderId = OrderId;
            od.DeliverDate = DeliverDate;
            DataSet ds = db.GetOrderShippingDetails("USP_GetOrderShippingDetails", StoreId, OrderId, dbdetails.Address, dbdetails.Password, dbdetails.DatabaseName, dbdetails.Username);
            OrderDetail orderdetais = new OrderDetail();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                orderdetais.storeName = (row["StoreName"]).ToString();
                orderdetais.StoreAddress = (row["Address"]).ToString();
                orderdetais.StoreCity = (row["City"]).ToString();
                orderdetais.StoreState = (row["State"]).ToString();
                orderdetais.StoreCountry = (row["Country"]).ToString();
                orderdetais.StorePhoneNo = (row["PhoneNo"]).ToString();
                orderdetais.StoreZipcode = (row["ZipCode"]).ToString();
                orderdetais.orderDate = Convert.ToDateTime(row["OrderDate"]);
                orderdetais.paidDate = Convert.ToDateTime(row["PaidDate"]);
                orderdetais.shippingPaid = 0;
                orderdetais.taxPaid = Convert.ToDecimal(row["TaxPaid"]);
                orderdetais.productTotal = Convert.ToDecimal(row["ProductTotal"]);
                orderdetais.totalOrder = Convert.ToDecimal(row["TotalOrder"]);
                orderdetais.totalPaid = Convert.ToDecimal(row["TotalPaid"]);
                orderdetais.holdUntil = "N/A";
                orderdetais.TransactionId = (row["TransactionId"]).ToString();
                orderdetais.OrderNotes = ds.Tables[0].Columns.Contains("OrderNotes") ? (row["OrderNotes"]).ToString() : null;


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
            foreach (DataRow row in ds.Tables[2].Rows)
            {
                OrderItemDetail value = new OrderItemDetail();
                value.name = (row["ProductName"]).ToString();
                value.sku = (row["SKU"]).ToString();
                value.Itemlist = (row["ProductMetaDetails"]).ToString();
                value.UnitPrice = Convert.ToDecimal(row["UnitPrice"]);
                value.OrderQty = (row["OrderQty"]).ToString();
                value.TotalPrice = Convert.ToDecimal(row["TotalPrice"]);

                orderdetails.Add(value);

            }
            od.OrderItemDetails = orderdetails;

            DataSet dsZone = db.GetCarrierdata("USP_GetCarrier");
            List<CarrierData> cd = new List<CarrierData>();
            List<ServicetypeData> sd = new List<ServicetypeData>();
            List<PackageData> pd = new List<PackageData>();
            foreach (DataRow row in dsZone.Tables[0].Rows)
            {

                CarrierData a = new CarrierData();
                a.Id = (row["CarrierId"]).ToString();
                a.Name = (row["CarrierName"]).ToString();

                cd.Add(a);

            }
            foreach (DataRow row in dsZone.Tables[1].Rows)
            {

                ServicetypeData a = new ServicetypeData();
                a.Id = (row["ServiceTypeId"]).ToString();
                a.Name = (row["ServiceTypeName"]).ToString();

                sd.Add(a);

            }
            foreach (DataRow row in dsZone.Tables[2].Rows)
            {

                PackageData a = new PackageData();
                a.Id = (row["PackageId"]).ToString();
                a.Name = (row["PackageaName"]).ToString();

                pd.Add(a);

            }
            od.CarrierList = cd;
            od.PackageList = pd;
            od.ServiceList = sd;

            DataSet dsZoneList = db.GetMysqlDataSet("USP_GetZones", StoreId);

            if (dsZoneList.Tables.Count > 0)
            {
                List<Zonelist> zl = new List<Zonelist>();
                foreach (DataRow row in dsZoneList.Tables[0].Rows)
                {

                    Zonelist a = new Zonelist();
                    a.ZoneId = (row["Id"]).ToString();
                    a.Carrier = (row["Carrier"]).ToString();
                    a.ZoneName = (row["ZoneName"]).ToString();

                    zl.Add(a);

                }
                od.ZoneList = zl;
            }

            string logoimagestring = null;
            string barcodeimagestring = null;

            logoimagestring = LoadImageForStore();
            barcodeimagestring = LoadImageForBarode(od.OrderId);

            od.LogoBase64String = logoimagestring;
            od.BarcodeBase64String = barcodeimagestring;

            return View("OrderShipmentDetails", od);
        }

        //public ActionResult CheckAddress(string StoreId, AddressVerify Addressrequest = null)
        //{
        //    DbDetails dbdetails = db.GetDbDetails(StoreId);

        //    AddressVerifyResponse arv =  InvokeService(dbdetails, Addressrequest);

        //    return Json(arv.AddressMatched);
        //}

        public ActionResult CheckAddress(string StoreId, AddressVerifyRequest Addressrequest = null)
        {
            DbDetails dbdetails = db.GetDbDetails(StoreId);


            AuthenticateUserResponse response = GetAuthentication(dbdetails, "AuthenticateUser");
            Addressrequest.AuthenticationId = response.Authenticator;
            AddressVerifyResponse Addressresponse = CleanseAddress(Addressrequest, "CleanseAddress");

            return Json(Addressresponse.AddressMatched);
        }

        public ActionResult GetShipRates(string StoreId, GetRates getrates = null)
        {
            DbDetails dbdetails = db.GetDbDetails(StoreId);
            getrates.fromZipcode = getrates.fromZipcode;
            getrates.toZipcode = getrates.toZipcode;
            getrates.PackageType = getrates.PackageType;
            //DateTime shipDate = DateTime.ParseExact(getrates.shipdate, "MM/dd/yyyy", null).AddDays(1);
            DateTime shipDate = DateTime.Now.AddDays(1);
            getrates.shipdate = shipDate.ToString("yyyy/MM/dd");
            getrates.WeightLb = getrates.WeightLb;
            getrates.WeightOz = getrates.WeightOz;
            getrates.servicetype = "US-PM";
            AuthenticateUserResponse response = GetAuthentication(dbdetails, "AuthenticateUser");
            getrates.AuthenticationId = response.Authenticator;
            GetRatesResponse val = GetRates(getrates, "GetRates");

            decimal RateAmount = 0;
            if (!string.IsNullOrEmpty(val.Amount))
            {
                RateAmount = Math.Round(Convert.ToDecimal(val.Amount), 2);
            }

            return Json(RateAmount);
        }

        //public ActionResult GetShipRates(string StoreId, GetRates getrates = null)
        //{
        //    DbDetails dbdetails = db.GetDbDetails(StoreId);
        //    getrates.fromZipcode = getrates.fromZipcode;
        //    getrates.toZipcode = getrates.toZipcode;
        //    getrates.PackageType = getrates.PackageType;
        //    //DateTime shipDate = DateTime.ParseExact(getrates.shipdate, "MM/dd/yyyy", null).AddDays(1);
        //    DateTime shipDate = DateTime.Now.AddDays(1);
        //    getrates.shipdate = shipDate.ToString("yyyy/MM/dd");
        //    getrates.WeightLb = getrates.WeightLb;
        //    getrates.WeightOz = getrates.WeightOz;

        //    GetRatesResponse val = GetShiprateService(dbdetails, getrates);

        //    decimal RateAmount = 0;
        //    if (!string.IsNullOrEmpty(val.Amount))
        //    {
        //        RateAmount = Math.Round(Convert.ToDecimal(val.Amount), 2);
        //    }

        //    return Json(RateAmount);
        //}

        public ActionResult GetLabels(string StoreId, CreateLabelRequest getlabel = null)
        {
            DbDetails dbdetails = db.GetDbDetails(StoreId);

            //getlabel.IntegratorTxID = ;
            //getlabel.FromFullName = "SWSIM API";
            //getlabel.Fromaddress = "1990 E GRAND AVE";
            //getlabel.FromCity = "EL SEGUNDO";
            //getlabel.FromState = "CA";
            //getlabel.FromZIPCode = "90245";
            if (getlabel.FromCountry.ToLower() == "united states" || getlabel.FromCountry.ToLower() == "usa" || getlabel.FromCountry.ToLower() == "unitedstates" || getlabel.FromCountry.ToLower() == "us")
            {
                getlabel.FromCountry = "US";
            }
            //getlabel.ToFullName = "SWSIM API";
            //getlabel.Toaddress = "1990 E GRAND AVE";
            //getlabel.ToCity = "EL SEGUNDO";
            //getlabel.ToState = "CA";
            //getlabel.ToZIPCode = "90245";
            if (getlabel.ToCountry.ToLower() == "united states" || getlabel.ToCountry.ToLower() == "usa" || getlabel.ToCountry.ToLower() == "unitedstates" || getlabel.ToCountry.ToLower() == "us")
            {
                getlabel.ToCountry = "US";
            }
            //getlabel.WeightLb = 0;
            //getlabel.WeightOz = 1;
            //getlabel.PackageType = "Package";
            //getlabel.shipdate = "2021-10-11";
            DateTime shipDate = DateTime.Now.AddDays(1);
            getlabel.shipdate = shipDate.ToString("yyyy/MM/dd");
            //getlabel.Length = "1";
            //getlabel.Width = "1";
            //getlabel.Height = "1";
            //getlabel.ServiceType = "US-PM";// "US-FC";
            //getlabel.Amount = "7.36";



            AuthenticateUserResponse response = GetAuthentication(dbdetails, "AuthenticateUser");
            getlabel.AuthenticationId = response.Authenticator;
            CreateLabelResponse res = GetLabel(getlabel, "CreateIndicium");

            DateTime del = res.DeliveryDate != null ? DateTime.ParseExact(res.DeliveryDate, "yyyy/MM/dd", null) : DateTime.Now;
            res.DeliveryDate = del.ToString("MM/dd/yyyy");
            res.ShipDate = shipDate.ToString("MM/dd/yyyy");
            res.ZoneName = getlabel.ZoneName;
            //return Json(res);

            Session["pdfData"] = null;
            if (res.Url != null)
            {
                string url = res.Url;

                string pdf_page_size = PdfPageSize.A4.ToString();
                PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
                    pdf_page_size, true);

                string pdf_orientation = PdfPageOrientation.Portrait.ToString();
                PdfPageOrientation pdfOrientation =
                    (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                    pdf_orientation, true);

                int webPageWidth = 1024;
                try
                {
                    //webPageWidth = Convert.ToInt32(TxtWidth.Text);
                }
                catch { }

                int webPageHeight = 0;
                try
                {
                    //webPageHeight = Convert.ToInt32(TxtHeight.Text);
                }
                catch { }

                // instantiate a html to pdf converter object
                HtmlToPdf converter = new HtmlToPdf();

                // set converter options
                converter.Options.PdfPageSize = pageSize;
                converter.Options.PdfPageOrientation = pdfOrientation;
                converter.Options.WebPageWidth = webPageWidth;
                converter.Options.WebPageHeight = webPageHeight;
                converter.Options.MarginLeft = 150;
                //converter.Options.MarginRight = 10;
                converter.Options.MarginTop = 50;
                //converter.Options.MarginBottom = 20;

                // create a new pdf document converting an url
                PdfDocument doc = converter.ConvertUrl(url);

                // save pdf document
                byte[] pdf = doc.Save();

                // close pdf document
                doc.Close();
                Session["pdfData"] = pdf;
                //// return resulted pdf document
                //FileResult fileResult = new FileContentResult(pdf, "application/pdf");
                //fileResult.FileDownloadName = "Document.pdf";


                //string base64String = "data: application/pdf; base64, " + Convert.ToBase64String(pdf);
                //return Json(pdf);
                //return File(pdf, "application/pdf");
                //return fileResult;
                //return File((byte[])pdf, "application/pdf", fileResult.FileDownloadName);
            }
            return Json(res);
        }

        public virtual ActionResult DownloadPDF(string fileName)
        {
            string fullFileName = fileName + "_" + DateTime.Now.ToFileTime() + ".pdf";
            return File((byte[])Session["pdfData"], "application/pdf", fullFileName);            
        }

        public AuthenticateUserResponse GetAuthentication(DbDetails dbdetails, string Apiname)
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

        public CreateLabelResponse GetLabel(CreateLabelRequest getlabel, string Apiname)
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
            try
            {
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
                        foreach (XElement response in responses1)
                        {
                            Responses.ServiceType = (string)response.Element(ns + "ServiceType");
                            Responses.DeliveryDate = (string)response.Element(ns + "DeliveryDate");
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                //Responses.Url = null;
                return Responses;
            }

            return Responses;
        }

        //public GetRatesResponse GetShiprateService(DbDetails dbdetails, GetRates getratesrequest)
        //{


        //    AuthenticateUserResponse user = new AuthenticateUserResponse();
        //    //Calling CreateSOAPWebRequest method  
        //    HttpWebRequest request = CreateSOAPWebRequest();

        //    XmlDocument SOAPReqBody = new XmlDocument();
        //    //SOAP Body Request  
        //    SOAPReqBody.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>  
        //    <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""
        //        xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
        //        xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">  
        //     <soap:Body> 
        //        <AuthenticateUser xmlns=""http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111"">  
        //        <Credentials>
        //          <IntegrationID>" + dbdetails.IntegrationId + @"</IntegrationID>  
        //          <Username>" + dbdetails.StampsUserName + @"</Username> 
        //          <Password>" + dbdetails.StampsUserPassword + @"</Password>
        //         </Credentials>
        //        </AuthenticateUser>
        //      </soap:Body>  
        //    </soap:Envelope>");


        //    using (Stream stream = request.GetRequestStream())
        //    {
        //        SOAPReqBody.Save(stream);
        //    }
        //    //Geting response from request  
        //    using (WebResponse Serviceres = request.GetResponse())
        //    {
        //        using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
        //        {
        //            //reading stream  
        //            var ServiceResult = rd.ReadToEnd();
        //            //var otp = JsonConvert.DeserializeObject<AuthenticateUser>(ServiceResult);
        //            XDocument doc = XDocument.Parse(ServiceResult);
        //            XNamespace ns = "http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111";
        //            IEnumerable<XElement> responses = doc.Descendants(ns + "AuthenticateUserResponse");
        //            foreach (XElement response in responses)
        //            {
        //                var value = (string)response.Element(ns + "Authenticator");
        //                user.Authenticator = value.Replace("&", "&amp;");
        //            }

        //        }

        //    }



        //    getratesrequest.AuthenticationId = user.Authenticator;
        //    GetRatesResponse gr = GetRates(getratesrequest);
        //    return gr;



        //}

        //public AddressVerifyResponse InvokeService(DbDetails dbdetails, AddressVerifyRequest Addressrequest)
        //{


        //    AuthenticateUserResponse user = new AuthenticateUserResponse();
        //    //Calling CreateSOAPWebRequest method  
        //    HttpWebRequest request = CreateSOAPWebRequest();

        //    XmlDocument SOAPReqBody = new XmlDocument();
        //    //SOAP Body Request  
        //    SOAPReqBody.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>  
        //    <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""
        //        xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
        //        xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">  
        //     <soap:Body> 
        //        <AuthenticateUser xmlns=""http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111"">  
        //        <Credentials>
        //          <IntegrationID>" + dbdetails.IntegrationId + @"</IntegrationID>  
        //          <Username>" + dbdetails.StampsUserName + @"</Username> 
        //          <Password>" + dbdetails.StampsUserPassword + @"</Password>
        //         </Credentials>
        //        </AuthenticateUser>
        //      </soap:Body>  
        //    </soap:Envelope>");


        //    using (Stream stream = request.GetRequestStream())
        //    {
        //        SOAPReqBody.Save(stream);
        //    }
        //    //Geting response from request  
        //    using (WebResponse Serviceres = request.GetResponse())
        //    {
        //        using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
        //        {
        //            //reading stream  
        //            var ServiceResult = rd.ReadToEnd();
        //            //var otp = JsonConvert.DeserializeObject<AuthenticateUser>(ServiceResult);
        //            XDocument doc = XDocument.Parse(ServiceResult);
        //            XNamespace ns = "http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111";
        //            IEnumerable<XElement> responses = doc.Descendants(ns + "AuthenticateUserResponse");
        //            foreach (XElement response in responses)
        //            {
        //                var value = (string)response.Element(ns + "Authenticator");
        //                user.Authenticator = value.Replace("&", "&amp;");
        //            }

        //        }

        //    }
        //    AddressVerifyResponse avr = new AddressVerifyResponse();

        //        Addressrequest.AuthenticationId = user.Authenticator;
        //        avr = CleanseAddress(Addressrequest);

        //    return avr;
        //}

        public GetRatesResponse GetRates(GetRates getrates, string Apiname)
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
            try
            {
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
                            if (amountvalue)
                            {
                                Responses.Amount = (string)response.Element(ns + "Amount");
                                amountvalue = false;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Responses.Amount = "0";
                return Responses;
            }

            return Responses;
        }

        //public GetRatesResponse GetRates(GetRates getrates)
        //{
        //    GetRatesResponse Responses = new GetRatesResponse();
        //    //Calling CreateSOAPWebRequest method  
        //    HttpWebRequest request = GetRatesSOAPWebRequest();

        //    XmlDocument SOAPReqBody = new XmlDocument();
        //    //SOAP Body Request  
        //    SOAPReqBody.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>  
        //    <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""
        //        xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
        //        xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">  
        //     <soap:Body>               
        //    <GetRates xmlns=""http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111"">
        //    <Authenticator>" + getrates.AuthenticationId + @"</Authenticator>
        //    <Rate >
        //        <From>
        //            <ZIPCode>" + getrates.fromZipcode + @"</ZIPCode>
        //        </From>
        //        <To>
        //            <ZIPCode>" + getrates.toZipcode + @"</ZIPCode>
        //        </To>
        //       <WeightLb>" + getrates.WeightLb + @"</WeightLb>
        //        <WeightOz>" + getrates.WeightOz + @"</WeightOz>
        //        <PackageType>" + getrates.PackageType + @"</PackageType>
        //        <ShipDate>" + getrates.shipdate + @"</ShipDate>
        //    </Rate >
        //    </GetRates >
        //       </soap:Body>  
        //    </soap:Envelope>");


        //    using (Stream stream = request.GetRequestStream())
        //    {
        //        SOAPReqBody.Save(stream);
        //    }
        //    //Geting response from request  
        //    using (WebResponse Serviceres = request.GetResponse())
        //    {
        //        using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
        //        {
        //            //reading stream  
        //            var ServiceResult = rd.ReadToEnd();
        //            //var otp = JsonConvert.DeserializeObject<AuthenticateUser>(ServiceResult);
        //            XDocument doc = XDocument.Parse(ServiceResult);
        //            XNamespace ns = "http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111";
        //            IEnumerable<XElement> responses = doc.Descendants(ns + "GetRatesResponse");

        //            IEnumerable<XElement> responses1 = responses.Descendants(ns + "Rates");
        //            IEnumerable<XElement> responses2 = responses1.Descendants(ns + "Rate");
        //            IEnumerable<XElement> responses3 = responses.Descendants(ns + "Amount");
        //            var value = responses3.FirstOrDefault();
        //            string[] valu2 = value.ToString().Split('>');
        //            Responses.Amount = valu2[1].Replace("</Amount", "");

        //        }
        //    }
        //    return Responses;
        //}

        public AddressVerifyResponse CleanseAddress(AddressVerifyRequest Addressrequest, string ApiName)
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
            try
            {
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
            }
            catch (Exception ex)
            {
                Responses.AddressMatched = "false";
                return Responses;
            }

            return Responses;
        }

        //public AddressVerifyResponse CleanseAddress(AddressVerify Addressrequest)
        //{
        //    AddressVerifyResponse Responses = new AddressVerifyResponse();

        //    //Calling CreateSOAPWebRequest method  
        //    HttpWebRequest request = CleanseAddressSOAPWebRequest();

        //    XmlDocument SOAPReqBody = new XmlDocument();
        //    //SOAP Body Request  
        //    SOAPReqBody.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>  
        //    <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""
        //        xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
        //        xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">  
        //     <soap:Body> 
        //        <CleanseAddress xmlns=""http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111""> 
        //        <Authenticator>" + Addressrequest.AuthenticationId + @"</Authenticator>
        //         <Address>
        //        <FullName>" + Addressrequest.name + @"</FullName>
        //        <Address1>" + Addressrequest.address1 + @"</Address1>
        //        <City>" + Addressrequest.city + @"</City>
        //        <State>" + Addressrequest.state + @"</State>
        //        <ZIPCode>" + Addressrequest.zipcode + @"</ZIPCode>
        //        </Address>             
        //        </CleanseAddress>
        //      </soap:Body>  
        //    </soap:Envelope>");


        //    using (Stream stream = request.GetRequestStream())
        //    {
        //        SOAPReqBody.Save(stream);
        //    }
        //    //Geting response from request  
        //    using (WebResponse Serviceres = request.GetResponse())
        //    {
        //        using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
        //        {
        //            //reading stream  
        //            var ServiceResult = rd.ReadToEnd();
        //            //var otp = JsonConvert.DeserializeObject<AuthenticateUser>(ServiceResult);
        //            XDocument doc = XDocument.Parse(ServiceResult);
        //            XNamespace ns = "http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111";
        //            IEnumerable<XElement> responses = doc.Descendants(ns + "CleanseAddressResponse");

        //            foreach (XElement response in responses)
        //            {

        //                Responses.AddressMatched = (string)response.Element(ns + "AddressMatch");

        //            }

        //        }
        //    }
        //    return Responses;
        //}

        public HttpWebRequest CreateSOAPWebRequest(string Apiname)
        {
            //Making Web Request  
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"https://swsim.testing.stamps.com/swsim/swsimv111.asmx");
            //SOAPAction  
            Req.Headers.Add(@"SOAPAction:http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111/" + Apiname + "");//AuthenticateUser
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

        //public HttpWebRequest CreateSOAPWebRequest()
        //{
        //    //Making Web Request  
        //    HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"https://swsim.testing.stamps.com/swsim/swsimv111.asmx");
        //    //SOAPAction  
        //    Req.Headers.Add(@"SOAPAction:http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111/AuthenticateUser");
        //    //Content_type  
        //    // Req.ContentLength = "length";
        //    Req.ContentType = "text/xml;charset=\"utf-8\"";
        //    Req.Accept = "text/xml";
        //    //HTTP method  
        //    Req.Method = "POST";
        //    Req.Host = "swsim.testing.stamps.com";

        //    //return HttpWebRequest  
        //    return Req;
        //}
        public HttpWebRequest CleanseAddressSOAPWebRequest()
        {
            //Making Web Request  
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"https://swsim.testing.stamps.com/swsim/swsimv111.asmx");
            //SOAPAction  
            Req.Headers.Add(@"SOAPAction:http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111/CleanseAddress");
            Req.ContentType = "text/xml;charset=\"utf-8\"";
            Req.Accept = "text/xml";
            //HTTP method  
            Req.Method = "POST";
            Req.Host = "swsim.testing.stamps.com";

            //return HttpWebRequest  
            return Req;
        }
        public HttpWebRequest GetRatesSOAPWebRequest()
        {
            //Making Web Request  
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"https://swsim.testing.stamps.com/swsim/swsimv111.asmx");
            //SOAPAction  
            Req.Headers.Add(@"SOAPAction:http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111/GetRates");
            Req.ContentType = "text/xml;charset=\"utf-8\"";
            Req.Accept = "text/xml";
            //HTTP method  
            Req.Method = "POST";
            Req.Host = "swsim.testing.stamps.com";

            //return HttpWebRequest  
            return Req;
        }

        public string LoadImageForStore()
        {
            string path = null;
            string base64String = null;
            if (Session["StoreId"] != null)
            {
                string storeid = Session["StoreId"].ToString();


                if (storeid == "d73add35-876a-4c82-82f9-9591baf2c20d")
                {
                    path = Server.MapPath("~/Content/assets/images/logo.png");
                }
                else if (storeid == "f575a340-44a8-4f68-b5fc-efba3350a264")
                {
                    path = Server.MapPath("~/Content/StoreLogo/png/Mylapore Logo – 2.png");
                }
                else if (storeid == "2cfb7b87-3e7d-486f-b14a-356730689fbd")
                {
                    path = Server.MapPath("~/Content/StoreLogo/png/Logo.png");
                }
                try
                {
                    using (Image image = Image.FromFile(path))
                    {
                        //Bitmap b = new Bitmap(image);

                        //Image i = resizeImage(b, new Size(134, 30));

                        using (MemoryStream m = new MemoryStream())
                        {
                            image.Save(m, image.RawFormat);
                            byte[] imageBytes = m.ToArray();

                            // Convert byte[] to Base64 String
                            base64String = "data: image / png; base64, " + Convert.ToBase64String(imageBytes);
                            //return base64String;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            return base64String;

        }

        public string LoadImageForBarode(string orderno = null)
        {
            string base64String = null;
            if (!string.IsNullOrEmpty(orderno))
            {

                string prodCode = orderno;
                if (prodCode.Length > 0)
                {
                    Image image = null;

                    BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                    b.BackColor = System.Drawing.Color.White;
                    b.ForeColor = System.Drawing.Color.Black;
                    b.IncludeLabel = true;
                    b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                    b.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER;
                    b.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                    System.Drawing.Font font = new System.Drawing.Font("verdana", 8f);
                    b.LabelFont = font;
                    b.Height = 70;
                    b.Width = 300;
                    //b.AspectRatio = 2f;

                    image = b.Encode(BarcodeLib.TYPE.CODE39, prodCode);

                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, System.Drawing.Imaging.ImageFormat.Gif);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        base64String = "data: image / png; base64, " + Convert.ToBase64String(imageBytes);
                    }

                }
            }
            return base64String;
        }

        public ActionResult CreateMultiLabel(OrderIdList OrderIdList)
        {
            List<string> uncreatedLabel = new List<string>();
            foreach (var item in OrderIdList.OrderIds)
            {
                var shipDetails = OrderShipmentDetails(OrderIdList.StoreId, item, OrderIdList.DeliverDate);

                OrderDetails shipResult = db.ModelFromActionResult<OrderDetails>(shipDetails);

                AddressVerifyRequest addressVerifyRequest = new AddressVerifyRequest();

                if (shipResult.BuyersDetails != null)
                {
                    addressVerifyRequest.name = shipResult.BuyersDetails.name;
                    addressVerifyRequest.address1 = shipResult.BuyersDetails.address;
                    addressVerifyRequest.city = shipResult.BuyersDetails.city;
                    addressVerifyRequest.state = shipResult.BuyersDetails.state;
                    addressVerifyRequest.zipcode = shipResult.BuyersDetails.zipcode;
                }

                var objAddressVerified = CheckAddress(OrderIdList.StoreId, addressVerifyRequest);

                bool addressVerifiedStatus = false;

                if (objAddressVerified.GetType() == typeof(JsonResult))
                {
                    JsonResult jsonResultAddress = (JsonResult)objAddressVerified;
                    if (jsonResultAddress.Data.ToString() == "true" || jsonResultAddress.Data.ToString() == "false")
                    {
                        addressVerifiedStatus = jsonResultAddress.Data != null ? Convert.ToBoolean(jsonResultAddress.Data) : false;
                    }
                }

                GetRates getrates = new GetRates();

                getrates.fromZipcode = shipResult.orderSummary.StoreZipcode;
                getrates.toZipcode = shipResult.BuyersDetails.zipcode;
                getrates.PackageType = "Package";
                //DateTime shipDate = DateTime.ParseExact(getrates.shipdate, "MM/dd/yyyy", null).AddDays(1);
                DateTime shipDate = DateTime.Now.AddDays(1);
                getrates.shipdate = shipDate.ToString("yyyy/MM/dd");
                getrates.WeightLb = 10;
                getrates.WeightOz = 5;

                var shipRateDetails = GetShipRates(OrderIdList.StoreId, getrates);

                decimal shipRateAmount = 0;

                if (shipRateDetails.GetType() == typeof(JsonResult))
                {
                    JsonResult jsonResultShiprate = (JsonResult)shipRateDetails;
                    shipRateAmount = jsonResultShiprate.Data != null ? Convert.ToDecimal(jsonResultShiprate.Data) : 0;
                }

                CreateLabelRequest getlabel = new CreateLabelRequest();

                getlabel.IntegratorTxID = shipResult.orderSummary.TransactionId;
                getlabel.FromFullName = shipResult.orderSummary.storeName;
                getlabel.Fromaddress = shipResult.orderSummary.StoreAddress;
                getlabel.FromCity = shipResult.orderSummary.StoreCity;
                getlabel.FromState = shipResult.orderSummary.StoreState;
                getlabel.FromZIPCode = shipResult.orderSummary.StoreZipcode;
                getlabel.FromCountry = shipResult.orderSummary.StoreCountry;
                if (getlabel.FromCountry.ToLower() == "united states" || getlabel.FromCountry.ToLower() == "usa" || getlabel.FromCountry.ToLower() == "unitedstates" || getlabel.FromCountry.ToLower() == "us")
                {
                    getlabel.FromCountry = "US";
                }

                getlabel.ToFullName = shipResult.BuyersDetails.name;
                getlabel.Toaddress = shipResult.BuyersDetails.address;
                getlabel.ToCity = shipResult.BuyersDetails.city;
                getlabel.ToState = shipResult.BuyersDetails.state;
                getlabel.ToZIPCode = shipResult.BuyersDetails.zipcode;
                getlabel.ToCountry = shipResult.BuyersDetails.country;
                if (getlabel.ToCountry.ToLower() == "united states" || getlabel.ToCountry.ToLower() == "usa" || getlabel.ToCountry.ToLower() == "unitedstates" || getlabel.ToCountry.ToLower() == "us")
                {
                    getlabel.ToCountry = "US";
                }

                getlabel.WeightLb = 10;
                getlabel.WeightOz = 5;
                getlabel.PackageType = "Package";
                DateTime shipDate2 = DateTime.Now.AddDays(1);
                getlabel.shipdate = shipDate2.ToString("yyyy/MM/dd");
                getlabel.Length = "1";
                getlabel.Width = "1";
                getlabel.Height = "1";
                getlabel.ServiceType = "US-PM";// "US-FC";
                getlabel.Amount = shipRateAmount.ToString();

                var getLabelDetails = GetLabels(OrderIdList.StoreId, getlabel);

                CreateLabelResponse labelRespose = new CreateLabelResponse();
                dynamic LabelData;
                if (getLabelDetails.GetType() == typeof(JsonResult))
                {
                    JsonResult jsonResultLabel = (JsonResult)getLabelDetails;

                    if (jsonResultLabel.Data != null)
                    {
                        LabelData = jsonResultLabel.Data;
                        labelRespose = LabelData;
                    }
                }

                if (labelRespose.Url != null)
                {
                    string url = labelRespose.Url;

                    string pdf_page_size = PdfPageSize.A4.ToString();
                    PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
                        pdf_page_size, true);

                    string pdf_orientation = PdfPageOrientation.Portrait.ToString();
                    PdfPageOrientation pdfOrientation =
                        (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                        pdf_orientation, true);

                    int webPageWidth = 1024;
                    try
                    {
                        //webPageWidth = Convert.ToInt32(TxtWidth.Text);
                    }
                    catch { }

                    int webPageHeight = 0;
                    try
                    {
                        //webPageHeight = Convert.ToInt32(TxtHeight.Text);
                    }
                    catch { }

                    // instantiate a html to pdf converter object
                    HtmlToPdf converter = new HtmlToPdf();

                    // set converter options
                    converter.Options.PdfPageSize = pageSize;
                    converter.Options.PdfPageOrientation = pdfOrientation;
                    converter.Options.WebPageWidth = webPageWidth;
                    converter.Options.WebPageHeight = webPageHeight;

                    // create a new pdf document converting an url
                    PdfDocument doc = converter.ConvertUrl(url);

                    // save pdf document
                    byte[] pdf = doc.Save();

                    // close pdf document
                    doc.Close();

                    // return resulted pdf document
                    FileResult fileResult = new FileContentResult(pdf, "application/pdf");
                    fileResult.FileDownloadName = "Document.pdf";


                    //string base64String = "data: application/pdf; base64, " + Convert.ToBase64String(pdf);
                    //return Json(pdf);
                    //return File(pdf, "application/pdf");
                    return fileResult;
                }
                else
                {
                    uncreatedLabel.Add(item);                    
                }

            }
            return null;
        }


    }
}