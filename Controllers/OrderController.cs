using BarcodeLib;
//using iText.Layout;
using OneposStamps.Models;
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
using System.Web.Helpers;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Aspose.BarCode;
using System.Windows.Media.Imaging;
using iText.Kernel.Pdf.Canvas;
using Rectangle = iTextSharp.text.Rectangle;
using Font = System.Drawing.Font;
using RestSharp;
using Newtonsoft.Json;


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
        public  ActionResult InhouseLabel(OneposStamps.Models.CreateLabelRequest.CreateLabelRequest getlabel = null, string StoreName=null, string OrderId=null)
        {
            OneposStamps.Models.CreateLabelRequest.CreateLabelResponse response = new OneposStamps.Models.CreateLabelRequest.CreateLabelResponse();
            // var a= AddAddresDetails();
            var pgSize = new iTextSharp.text.Rectangle(288, 432);

            Document doc = new Document(pgSize, 0, 0, 0, 0);
            string path_pdf = AppDomain.CurrentDomain.BaseDirectory;
            string date = (DateTime.Today).ToString("MM-dd-yyyy");
            string pdfname = StoreName + date;
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(path_pdf + @"Pdf/"+ pdfname + ".pdf", FileMode.Create));
            string FromAddress1 = getlabel.shipment.ship_from.address_line1;
            string FromAddress2 = getlabel.shipment.ship_from.address_line2;
            string Fromaddress3 = getlabel.shipment.ship_from.city_locality +" "+ getlabel.shipment.ship_from.state_province+" " + getlabel.shipment.ship_from.postal_code;
               // "Roseville CA 95661";

            doc.Open();
            int i = 101;
            
            //foreach (var b in a)
            //{
                string Drivername = "C";
                var No = i.ToString();
                //string barCode = "94055";
                string Dname = Drivername + No;
                string path = AppDomain.CurrentDomain.BaseDirectory;
                string name = "Mylapore Logo – 1";
                string filename = path + @"Images/" + name + ".png";

                Paragraph ph = new Paragraph();
                PdfPCell cell = new PdfPCell(ph);
                cell.Border = Rectangle.ALIGN_BASELINE;
               
                cell.BorderWidth = 5f;
                Paragraph ph2 = new Paragraph();
                PdfPCell cell2 = new PdfPCell(ph);
                cell2.Border = Rectangle.BOTTOM_BORDER;
                
                cell2.BorderWidth = 1f;

                PdfPTable table = new PdfPTable(1);
                table.AddCell(cell);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                table.WidthPercentage = 100f;
                PdfPTable table2 = new PdfPTable(1);
                table2.AddCell(cell2);
                table2.HorizontalAlignment = Element.ALIGN_RIGHT;
                table2.WidthPercentage = 100f;
                
                Paragraph p1 = new Paragraph();
                p1.Font = FontFactory.GetFont("Arial", 9);
                p1.Add("Shipment Date:");
                p1.Add("\n");
                p1.Add(date);
                p1.IndentationLeft = 40f;
               
                Paragraph p2 = new Paragraph();
                //if (a.Count <= 9)
                //{
                    p2.Font = FontFactory.GetFont("Corbel Light", 85);
                //}
                //else if (a.Count >= 10 & a.Count < 99)
                //{
                //    p2.Font = FontFactory.GetFont("Corbel Light", 75);
                //}
                //else if (a.Count >= 100 & a.Count < 999)
                //{
                //    p2.Font = FontFactory.GetFont("Corbel Light", 58);
                //}

                p2.Add("\n");
                p2.Add("\n");
                p2.Add("\n");
                p2.Add(Dname);
                
                Paragraph p3 = new Paragraph();
                p3.IndentationLeft = 20f;
                p3.Font = FontFactory.GetFont("Arial", 22);
                p3.Add(StoreName);
               
                BarCodeBuilder builder = new BarCodeBuilder(OrderId, Symbology.Code128);
                builder.CodeTextFont = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular);
                BitmapImage bmp = new BitmapImage();
                bmp = GetBitmapImage(new System.Drawing.Bitmap(builder.BarCodeImage));
                iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(getJPGFromImageControl(bmp));
                png.ScaleAbsolute(125f, 5f);
                png.Border = 0;

                PdfPTable maintable = new PdfPTable(2);
                maintable.SpacingBefore = 5f;
                maintable.DefaultCell.Border = Rectangle.NO_BORDER;
                PdfPCell cell1 = new PdfPCell();

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(filename);
                image.ScalePercent(13f);
                cell1 = new PdfPCell();
                cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                cell1.BorderWidth = 0;
                cell1.AddElement(image);
                maintable.AddCell(cell1);
                cell1 = new PdfPCell();
                cell1.AddElement(p1);
                cell1.BorderWidth = 0;
                maintable.AddCell(cell1);


                PdfPTable maintable2 = new PdfPTable(1);
                maintable2.SpacingBefore = 5f;
                maintable2.DefaultCell.Border = Rectangle.NO_BORDER;
                PdfPCell cell5 = new PdfPCell();
                cell5.HorizontalAlignment = Element.ALIGN_CENTER;
                cell5.BorderWidth = 0;
                cell5.AddElement(p3);
                maintable2.SpacingAfter = 5f;
                maintable2.AddCell(cell5);

                PdfPTable maintable3 = new PdfPTable(1);
                maintable3.SpacingBefore = 5f;
                maintable3.DefaultCell.Border = Rectangle.NO_BORDER;
                PdfPCell cell6 = new PdfPCell();
                cell6.PaddingLeft = 30f;
                           
                cell6.BorderWidth = 0;
                cell6.AddElement(png);              
                maintable3.SpacingAfter = 5f;
                maintable3.AddCell(cell6);

                Paragraph c1 = new Paragraph();
                c1.Font = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD);
                c1.IndentationLeft = 10f;
                c1.Add("Ship To: ");
                Paragraph c2 = new Paragraph();
                c2.IndentationLeft = 70;
                c2.Font = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD);
                c2.Add("www.mylaporeexpress.com");
                c2.Add("\n");
                Paragraph c3 = new Paragraph();
                c3.IndentationLeft = 80;
                c3.Font = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD);
                c3.Add("Toll Free:9169256200");

                Paragraph l1 = new Paragraph();
                l1.Font = FontFactory.GetFont("Arial", 7);
                l1.IndentationLeft = 12f;
                l1.Add(FromAddress1);
                l1.Add("\n");
                l1.Add(FromAddress2);
                l1.Add("\n");
                l1.Add(Fromaddress3);
                l1.Add("\n");

                Paragraph l2 = new Paragraph();
                l2.IndentationLeft = 12f;
                l2.SpacingBefore = 5f;
                l2.Font= FontFactory.GetFont("Arial", 12);
                l2.Add(getlabel.shipment.ship_from.name);
                l2.Add("\n");
                l2.Add(getlabel.shipment.ship_from.address_line1);
                l2.Add("\n");
                 var address= getlabel.shipment.ship_from.city_locality +" "+ getlabel.shipment.ship_from.state_province+" " + getlabel.shipment.ship_from.postal_code;
                 Paragraph l3 = new Paragraph();
                l3.IndentationLeft = 12f;
                l3.SpacingBefore = 5f;
                l3.Font = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD);
                l3.Add(address);
                l3.SpacingAfter = 14f;

                PdfPTable maintable4 = new PdfPTable(2);
                maintable4.WidthPercentage = 100f;
                maintable4.SpacingBefore = 5f;
                maintable4.DefaultCell.Border = Rectangle.NO_BORDER;
                PdfPCell cell7 = new PdfPCell();

                cell7.BorderWidth = 0;
                cell7.AddElement(l1);
                maintable4.SpacingAfter = 5f;
                maintable4.AddCell(cell7);
                cell7 = new PdfPCell();
                cell7.BorderWidth = 0;
                cell7.AddElement(p2);
                cell7.PaddingTop = 20f;
                cell7.Rowspan = 3;
                maintable4.AddCell(cell7);
                cell7 = new PdfPCell();
                cell7.BorderWidth = 0;
                cell7.AddElement(c1);
                cell7.AddElement(l2);
                maintable4.AddCell(cell7);
                cell7 = new PdfPCell();
                cell7.BorderWidth = 0;
                cell7.AddElement(l3);
                maintable4.AddCell(cell7);

                doc.Add(maintable);
                doc.Add(maintable2);
                doc.Add(maintable4);
                doc.Add(table);
                doc.Add(maintable3);
                doc.Add(table);
                doc.Add(c2);
                doc.Add(c3);
                doc.NewPage();
                i++;
            // }
            response.ServiceType = "Mylapore Express";
            response.ShipDate=
            
            doc.Close();
            //var pdfPath = Path.Combine(Server.MapPath(path_name));

            return PartialView();

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
                a.CarrierId= (row["CarrierId"]).ToString();
                a.Service_Code= (row["service_Code"]).ToString();
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
            AddressVerifyRequest addressrequest = new AddressVerifyRequest();
            addressrequest.name = ShippingDetails.name;
            addressrequest.address1 = ShippingDetails.address;
            addressrequest.city = ShippingDetails.city;
            addressrequest.state = ShippingDetails.state;
            addressrequest.zipcode = ShippingDetails.zipcode;
            addressrequest.AuthenticationId = dbdetails.IntegrationId;
            CheckAddress(StoreId, addressrequest);
            od.AddressVerified = Addressresponse.AddressMatched;

            return View("OrderShipmentDetails", od);
            
        }

        
        AddressVerifyResponse Addressresponse = new AddressVerifyResponse();
        public ActionResult CheckAddress(string StoreId, AddressVerifyRequest Addressrequest = null)
        {
            DbDetails dbdetails = db.GetDbDetails(StoreId);


            //AuthenticateUserResponse response = GetAuthentication(dbdetails, "AuthenticateUser");
            Addressrequest.AuthenticationId = dbdetails.IntegrationId;
            Addressresponse = CleanseAddress(Addressrequest, "CleanseAddress");
            return Json(Addressresponse.AddressMatched);
        }

        public ActionResult GetShipRates(string StoreId, GetRates getrates = null)
        {
            DbDetails dbdetails = db.GetDbDetails(StoreId);
            //getrates.fromZipcode = getrates.fromZipcode;
            //getrates.toZipcode = getrates.toZipcode;
            //getrates.PackageType = getrates.PackageType;
            ////DateTime shipDate = DateTime.ParseExact(getrates.shipdate, "MM/dd/yyyy", null).AddDays(1);
            //DateTime shipDate = DateTime.Now.AddDays(1);
            //getrates.shipdate = shipDate.ToString("yyyy/MM/dd");
            //getrates.WeightLb = getrates.WeightLb;
            //getrates.WeightOz = getrates.WeightOz;
            //getrates.servicetype = "US-PM";
            //AuthenticateUserResponse response = GetAuthentication(dbdetails, "AuthenticateUser");
            // = dbdetails.IntegrationId;
            GetRatesResponse val = GetRates(getrates, dbdetails.IntegrationId);

            decimal RateAmount = 0;
            if ((val.Amount) !=0)
            {
                RateAmount = Math.Round(Convert.ToDecimal(val.Amount), 2);
            }

            return Json(RateAmount);
        }

       

        public ActionResult GetLabels(string StoreId, OneposStamps.Models.CreateLabelRequest.CreateLabelRequest getlabel = null)
        {
            DbDetails dbdetails = db.GetDbDetails(StoreId);

            ////getlabel.IntegratorTxID = ;
            ////getlabel.FromFullName = "SWSIM API";
            ////getlabel.Fromaddress = "1990 E GRAND AVE";
            ////getlabel.FromCity = "EL SEGUNDO";
            ////getlabel.FromState = "CA";
            ////getlabel.FromZIPCode = "90245";
            //if (getlabel.FromCountry.ToLower() == "united states" || getlabel.FromCountry.ToLower() == "usa" || getlabel.FromCountry.ToLower() == "unitedstates" || getlabel.FromCountry.ToLower() == "us")
            //{
            //    getlabel.FromCountry = "US";
            //}
            ////getlabel.ToFullName = "SWSIM API";
            ////getlabel.Toaddress = "1990 E GRAND AVE";
            ////getlabel.ToCity = "EL SEGUNDO";
            ////getlabel.ToState = "CA";
            ////getlabel.ToZIPCode = "90245";
            //if (getlabel.ToCountry.ToLower() == "united states" || getlabel.ToCountry.ToLower() == "usa" || getlabel.ToCountry.ToLower() == "unitedstates" || getlabel.ToCountry.ToLower() == "us")
            //{
            //    getlabel.ToCountry = "US";
            //}
            ////getlabel.WeightLb = 0;
            ////getlabel.WeightOz = 1;
            ////getlabel.PackageType = "Package";
            ////getlabel.shipdate = "2021-10-11";
            //DateTime shipDate = DateTime.Now.AddDays(1);
            //getlabel.shipdate = shipDate.ToString("yyyy/MM/dd");
            
            OneposStamps.Models.CreateLabelRequest.CreateLabelResponse res = GetLabel(getlabel, dbdetails.IntegrationId);

            //DateTime del = DateTime.ParseExact(res.DeliveryDate, "yyyy/MM/dd", null);
           // res.DeliveryDate = del.ToString("MM/dd/yyyy");
            //res.ShipDate = shipDate.ToString("MM/dd/yyyy");
            //res.ZoneName = getlabel.ZoneName;
            return Json(res);
        }

        
        public List<ClientAddress> AddAddresDetails()
        {
            List<ClientAddress> list = new List<ClientAddress>();
            ClientAddress list1 = new ClientAddress();
            list1.Username = "Priyanka Vivek";
            list1.Address1 = "19363 Brockton Lane";
            list1.Address2 = "Saratoga CA 95070";
            list1.BarcodeNumber = "904556";
            list.Add(list1);
            ClientAddress list2 = new ClientAddress();
            list2.Username = "Amit Shukla";
            list2.Address1 = "1612 Stemel Way";
            list2.Address2 = "Milpitas CA 95035";
            list2.BarcodeNumber = "904557";
            list.Add(list2);
            ClientAddress list3 = new ClientAddress();
            list3.Username = "Kausik Rajgopal";
            list3.Address1 = "4008 Scottfield Street";
            list3.Address2 = "Dublin CA 94568";
            list3.BarcodeNumber = "904558";
            list.Add(list3);
            ClientAddress list4 = new ClientAddress();
            list4.Username = "Neerja Jain";
            list4.Address1 = "37794 Taro Terrace";
            list4.Address2 = "Newark CA 94560";
            list4.BarcodeNumber = "904559";
            list.Add(list4);
            ClientAddress list5 = new ClientAddress();
            list5.Username = "Srividya Murali";
            list5.Address1 = "19363 Brockton Lane";
            list5.Address2 = "Saratoga CA 95070";
            list5.BarcodeNumber = "904560";
            list.Add(list5);
            ClientAddress list6 = new ClientAddress();
            list6.Username = "Divya Iyer";
            list6.Address1 = "3164 Joanne Circle";
            list6.Address2 = "Pleasanton CA 94588";
            list6.BarcodeNumber = "904561";
            list.Add(list6);
            ClientAddress list7 = new ClientAddress();
            list7.Username = "Ramesh Durairaj";
            list7.Address1 = "283 Margarita Ave";
            list7.Address2 = "Palo Alto CA 94306";
            list7.BarcodeNumber = "904562";
            list.Add(list7);
            ClientAddress list8 = new ClientAddress();
            list8.Username = "Prakash Krishna";
            list8.Address1 = "1227 Fairview Avenue";
            list8.Address2 = "San Jose CA 95125";
            list8.BarcodeNumber = "904563";
            list.Add(list8);
            ClientAddress list9 = new ClientAddress();
            list9.Username = "Leena Antony";
            list9.Address1 = "34 Dias Dorados";
            list9.Address2 = "Orinda CA 94563";
            list9.BarcodeNumber = "904564";
            list.Add(list9);


            return list;

        }

        public OneposStamps.Models.CreateLabelRequest.CreateLabelResponse GetLabel(OneposStamps.Models.CreateLabelRequest.CreateLabelRequest getlabel, string AuthenticationId)
        {
            OneposStamps.Models.CreateLabelRequest.CreateLabelResponse Responses = new OneposStamps.Models.CreateLabelRequest.CreateLabelResponse();
            var client = new RestClient("https://api.shipengine.com/v1/labels");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("API-Key", AuthenticationId);
            request.AddHeader("Content-Type", "application/json");
            OneposStamps.Models.CreateLabelRequest.CreateLabelRequest lbl = new OneposStamps.Models.CreateLabelRequest.CreateLabelRequest();
            lbl.shipment =new Models.CreateLabelRequest.Shipment();
            lbl.shipment.ship_from = new Models.CreateLabelRequest.ShipFrom();
            lbl.shipment.ship_to = new Models.CreateLabelRequest.ShipTo();
            lbl.shipment.service_code = "ups_ground";
            lbl.shipment.ship_from.company_name = "Example Corp.";
            lbl.shipment.ship_from.name = "John Doe";
            lbl.shipment.ship_from.phone = "111-111-1111";
            lbl.shipment.ship_from.address_line1 = "4009 Marathon Blvd";
            lbl.shipment.ship_from.address_line2 = "Suite 300";
            lbl.shipment.ship_from.city_locality = "Austin";
            lbl.shipment.ship_from.state_province = "TX";
            lbl.shipment.ship_from.postal_code = "78756";
            lbl.shipment.ship_from.country_code = "US";
            lbl.shipment.ship_from.address_residential_indicator = "no";
            lbl.shipment.ship_to.name = "Amanda Miller";
            lbl.shipment.ship_to.phone = "555-555-5555";
            lbl.shipment.ship_to.address_line1 = "525 S Winchester Blvd";
            lbl.shipment.ship_to.city_locality = "San Jose";
            lbl.shipment.ship_to.state_province = "CA";
            lbl.shipment.ship_to.postal_code = "95128";
            lbl.shipment.ship_to.country_code = "US";
            lbl.shipment.ship_to.address_residential_indicator = "yes";
            lbl.shipment.packages = new List<Models.CreateLabelRequest.Package>();
            Models.CreateLabelRequest.Package p = new Models.CreateLabelRequest.Package();
            p.weight = new Models.CreateLabelRequest.Weight();
            p.dimensions = new Models.CreateLabelRequest.Dimensions();
            p.weight.unit = "ounce";
            p.weight.value = 1.0;
            p.dimensions.height = 6;
            p.dimensions.length = 12;
            p.dimensions.unit = "inch";
            p.dimensions.width = 7.1;
            lbl.shipment.packages.Add(p);
            var body = JsonConvert.SerializeObject(lbl);

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {

                string result = response.Content;
                OneposStamps.Models.CreateLabelResponse.Root value1 = JsonConvert.DeserializeObject<OneposStamps.Models.CreateLabelResponse.Root>(result);
                Responses.TrackingNumber = value1.tracking_number;
                Responses.ServiceType = value1.service_code;
                Responses.ShipDate = value1.ship_date.ToString("MM-dd-yyyy HH:mm:ss");

                //Responses.DeliveryDate= value1..ToString("MM-dd-yyyy HH:mm:ss");

                OneposStamps.Models.CreateLabelResponse.LabelDownload label = value1.label_download;
                Responses.Url = label.pdf;
                //OneposStamps.Models.GetRateResponse.Rate rate = rateResponse.rates.FirstOrDefault();
                //Responses.Amount = rate.confirmation_amount.amount + rate.insurance_amount.amount + rate.shipping_amount.amount + rate.other_amount.amount;
                //Responses.Deliverydate = rate.estimated_delivery_date.ToString("MM-dd-yyyy HH:mm:ss");
            }

            return Responses;
        }

      

       

        public GetRatesResponse GetRates(GetRates getrates,string AuthenticationId)
        {
            GetRatesResponse Responses = new GetRatesResponse();
            var client = new RestClient("https://api.shipengine.com/v1/rates");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("API-Key", AuthenticationId);
            request.AddHeader("Content-Type", "application/json");

            GetRates g1 = new GetRates();
            RateOptions r = new RateOptions();
            //r.carrier_ids.Add("se-972447");
            //g1.rate_options.carrier_ids = r.carrier_ids.Add(r);
            //ShipFrom sf = new ShipFrom();
            g1.rate_options = new RateOptions();
            g1.rate_options.carrier_ids = new List<string>();
            g1.rate_options.package_types = new List<string>();
            g1.rate_options.service_codes = new List<string>();
            g1.rate_options.carrier_ids.Add("se-972448");
            g1.rate_options.package_types.Add("package");
            g1.rate_options.service_codes.Add("ups_ground");
            g1.shipment = new Shipment();
            g1.shipment.ship_from = new ShipFrom();
            g1.shipment.ship_to = new ShipTo();
            g1.shipment.ship_from.company_name = "Example Corp.";
            g1.shipment.ship_from.name = "John Doe";
            g1.shipment.ship_from.phone = "111-111-1111";
            g1.shipment.ship_from.address_line1 = "4009 Marathon Blvd";
            g1.shipment.ship_from.address_line2 = "Suite 300";
            g1.shipment.ship_from.city_locality = "Austin";
            g1.shipment.ship_from.state_province = "TX";
            g1.shipment.ship_from.postal_code = "78756";
            g1.shipment.ship_from.country_code = "US";
            g1.shipment.ship_from.address_residential_indicator = "no";
            g1.shipment.ship_to.name = "Amanda Miller";
            g1.shipment.ship_to.phone = "555-555-5555";
            g1.shipment.ship_to.address_line1 = "525 S Winchester Blvd";          
            g1.shipment.ship_to.city_locality = "San Jose";
            g1.shipment.ship_to.state_province = "CA";
            g1.shipment.ship_to.postal_code = "95128";
            g1.shipment.ship_to.country_code = "US";
            g1.shipment.ship_to.address_residential_indicator = "yes";
            g1.shipment.validate_address = "no_validation";
            
            g1.shipment.packages = new List<Package>();
            Package p = new Package();
            p.weight = new Weight();
            p.dimensions = new Dimensions();
            p.weight.unit = "ounce";
            p.weight.value = 1.0;           
            p.dimensions.height = 6;
            p.dimensions.length = 12;
            p.dimensions.unit = "inch";
            p.dimensions.width = 7.1;
            g1.shipment.packages.Add(p);

            var body = JsonConvert.SerializeObject(g1);

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if(response.StatusCode==HttpStatusCode.OK)
            {
                
                string result = response.Content;
                OneposStamps.Models.GetRateResponse.Root value1 = JsonConvert.DeserializeObject<OneposStamps.Models.GetRateResponse.Root>(result);
                OneposStamps.Models.GetRateResponse.RateResponse rateResponse = value1.rate_response;
                OneposStamps.Models.GetRateResponse.Rate rate = rateResponse.rates.FirstOrDefault();
                Responses.Amount = rate.confirmation_amount.amount + rate.insurance_amount.amount + rate.shipping_amount.amount + rate.other_amount.amount;
                Responses.Deliverydate = rate.estimated_delivery_date.ToString("MM-dd-yyyy HH:mm:ss");
            }


            return Responses;
        }

      

        public AddressVerifyResponse CleanseAddress(AddressVerifyRequest Addressrequest, string ApiName)
        {
            AddressVerifyResponse Responses = new AddressVerifyResponse();
            var client = new RestClient("https://api.shipengine.com/v1/addresses/validate");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("API-Key", Addressrequest.AuthenticationId);
            request.AddHeader("Content-Type", "application/json");
            const string quote = "\"";
            var body = @"[" + "\n" +
            @"    {" + "\n" +
            @"        ""name"": "+ quote + ""+ Addressrequest.name + "" + quote + "," + "\n" +
            @"        ""address_line1"": " + quote + ""+ Addressrequest.address1 + "" + quote + "," + "\n" +
            @"        ""city_locality"": " + quote + ""+ Addressrequest.city + "" + quote + "," + "\n" +
            @"        ""state_province"": " + quote + ""+ Addressrequest.state + "" + quote + "," + "\n" +
            @"        ""postal_code"": "+ quote +""+ Addressrequest.zipcode + "" + quote + "," + "\n" +
            @"        ""country_code"": ""US""" + "\n" +
            @"    }" + "\n" +
            @"]";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            // Console.WriteLine(response.Content);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string result = response.Content;
                List<OneposStamps.Models.Root> value1 = JsonConvert.DeserializeObject<List<OneposStamps.Models.Root>>(result);
                var value = value1.FirstOrDefault();
                Responses.AddressMatched = value.status;
            }

            return Responses;
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
                    using (System.Drawing.Image image = System.Drawing.Image.FromFile(path))
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
        public static BitmapImage GetBitmapImage(System.Drawing.Bitmap bitmap)
        {
            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }
            return bitmapImage;
        }
        public byte[] getJPGFromImageControl(BitmapImage imageC)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }

        public string LoadImageForBarode(string orderno = null)
        {
            string base64String = null;
            if (!string.IsNullOrEmpty(orderno))
            {

                string prodCode = orderno;
                if (prodCode.Length > 0)
                {
                    System.Drawing.Image image = null;

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
    }
}