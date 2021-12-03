using BarcodeLib;
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
using PdfDocument = SelectPdf.PdfDocument;
using iTextSharp.text.pdf.draw;
using NReco.PdfGenerator;

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

                return View(od);
            }

            if (string.IsNullOrEmpty(DeliverDate) && !string.IsNullOrEmpty(redirectedDeliverDate))
            {
                od.DeliverDate = redirectedDeliverDate;

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
                value.ZoneId = (row["ZoneId"]).ToString();
                value.OrderDate = (row["OrderDate"]) != null ? Convert.ToDateTime(row["OrderDate"]) : (DateTime?)null;
                value.OrderTotal = (row["OrderTotal"]) != null ? Convert.ToDecimal(row["OrderTotal"]) : 0;
                value.CustomerName = (row["CustomerName"]).ToString();
                value.Qty = Convert.ToDecimal(row["Qty"]);
                value.storeName = (row["StoreName"]).ToString();
                value.StoreAddress = (row["StoreAddress"]).ToString();
                value.StoreCity = (row["StoreCity"]).ToString();
                value.StoreState = (row["StoreState"]).ToString();
                value.StoreCountry = (row["StoreCountry"]).ToString();
                value.StorePhoneNo = (row["StorePhoneNo"]).ToString();
                value.StoreZipcode = (row["StoreZipCode"]).ToString();
                value.name = (row["DeliveryName"]).ToString();
                value.address = (row["Adress"]).ToString();
                value.phoneNo = (row["Phone"]).ToString();
                value.city = (row["City"]).ToString();
                value.state = (row["State"]).ToString().Trim();
                value.zipcode = (row["ZipCode"]).ToString();
                value.country = (row["Country"]).ToString();
                value.email = (row["EmailAddress"]).ToString();
                value.landMark = (row["LandMark"]).ToString();
                value.street = (row["Street"]).ToString();

                getorderslist.Add(value);

            }
            od.OrderList = getorderslist;

            return PartialView("_OrderDetails", od);
        }

        //public ActionResult InhouseLabel(OneposStamps.Models.CreateLabelRequest.CreateLabelRequest getlabel = null, string StoreId = null, string OrderId = null)
        //{
        //    Session["pdfData"] = null;
        //    OneposStamps.Models.CreateLabelRequest.CreateLabelResponse response = new OneposStamps.Models.CreateLabelRequest.CreateLabelResponse();
        //    try
        //    {
        //        // var a= AddAddresDetails();
        //        var pgSize = new iTextSharp.text.Rectangle(288, 432);

        //        Document doc = new Document(pgSize, 0, 0, 0, 0);
        //        string path_pdf = AppDomain.CurrentDomain.BaseDirectory;
        //        string date = (DateTime.Today).ToString("MM-dd-yyyy");
        //        string pdfname = OrderId + "_" + DateTime.Now.ToFileTime();
        //        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(path_pdf + @"Pdf/" + pdfname + ".pdf", FileMode.Create));
        //        string FromAddress1 = getlabel.shipment.ship_from.address_line1;
        //        // string FromAddress2 = getlabel.shipment.ship_from.address_line2;
        //        string Fromaddress3 = getlabel.shipment.ship_from.city_locality + " " + getlabel.shipment.ship_from.state_province + " " + getlabel.shipment.ship_from.postal_code;


        //        doc.Open();
        //        int i = 1;

        //        //foreach (var b in a)
        //        //{
        //        string Drivername = "C";
        //        var No = i.ToString();

        //        string Dname = Drivername + No;
        //        string path = AppDomain.CurrentDomain.BaseDirectory;
        //        string name = "Mylapore_Logo___3";
        //        string filename = path + @"Images/" + name + ".png";

        //        Paragraph ph = new Paragraph();
        //        PdfPCell cell = new PdfPCell(ph);
        //        //cell.Border = Rectangle.ALIGN_BASELINE;
        //        cell.Border = Rectangle.ALIGN_BASELINE;

        //        cell.BorderWidth = 5f;
        //        Paragraph ph2 = new Paragraph();
        //        PdfPCell cell2 = new PdfPCell(ph);
        //        cell2.Border = Rectangle.BOTTOM_BORDER;

        //        cell2.BorderWidth = 1f;

        //        PdfPTable table = new PdfPTable(1);
        //        table.AddCell(cell);
        //        table.HorizontalAlignment = Element.ALIGN_LEFT;
        //        table.WidthPercentage = 100f;

        //        PdfPTable table2 = new PdfPTable(1);
        //        table2.AddCell(cell2);
        //        table2.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        table2.WidthPercentage = 100f;

        //        Paragraph p1 = new Paragraph();
        //        p1.Font = FontFactory.GetFont("Arial", 9);
        //        p1.Add("Shipment Date:");
        //        p1.Add("\n");
        //        p1.Add(date);
        //        p1.IndentationLeft = 40f;

        //        Paragraph p2 = new Paragraph();
        //        //if (a.Count <= 9)
        //        //{
        //        p2.Font = FontFactory.GetFont("Corbel Light", 85);
        //        //}
        //        //else if (a.Count >= 10 & a.Count < 99)
        //        //{
        //        //    p2.Font = FontFactory.GetFont("Corbel Light", 75);
        //        //}
        //        //else if (a.Count >= 100 & a.Count < 999)
        //        //{
        //        //    p2.Font = FontFactory.GetFont("Corbel Light", 58);
        //        //}

        //        p2.Add("\n");
        //        p2.Add("\n");
        //        p2.Add("\n");
        //        p2.Add(Dname);

        //        Paragraph p3 = new Paragraph();
        //        p3.IndentationLeft = 20f;
        //        p3.Font = FontFactory.GetFont("Arial", 22);
        //        p3.Add(getlabel.shipment.ship_from.company_name);

        //        BarCodeBuilder builder = new BarCodeBuilder(OrderId, Symbology.Code128);
        //        builder.CodeTextFont = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular);
        //        BitmapImage bmp = new BitmapImage();
        //        bmp = GetBitmapImage(new System.Drawing.Bitmap(builder.BarCodeImage));
        //        iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(getJPGFromImageControl(bmp));
        //        png.ScaleAbsolute(125f, 5f);
        //        png.Border = 0;
        //        Paragraph l1 = new Paragraph();
        //        l1.Font = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.ITALIC);

        //        l1.IndentationLeft = 12f;
        //        l1.Add(FromAddress1);
        //        //l1.Add("\n");
        //        //l1.Add(FromAddress2);
        //        l1.Add("\n");
        //        l1.Add(Fromaddress3);
        //        l1.Add("\n");
        //        PdfPTable maintable = new PdfPTable(2);
        //        maintable.SpacingBefore = 5f;
        //        maintable.WidthPercentage = 100f;
        //        maintable.DefaultCell.Border = Rectangle.NO_BORDER;
        //        PdfPCell cell1 = new PdfPCell();

        //        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(filename);
        //        image.ScalePercent(20f);
        //        cell1 = new PdfPCell();
        //        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell1.BorderWidth = 0;
        //        cell1.AddElement(image);
        //        maintable.AddCell(cell1);
        //        cell1 = new PdfPCell();
        //        cell1.PaddingTop = 30f;
        //        cell1.AddElement(p1);
        //        cell1.Rowspan = 2;
        //        cell1.BorderWidth = 0;
        //        maintable.AddCell(cell1);
        //        cell1 = new PdfPCell();
        //        cell1.AddElement(l1);
        //        cell1.BorderWidth = 0;
        //        maintable.SpacingAfter = 10f;
        //        maintable.AddCell(cell1);
        //        LineSeparator line = new LineSeparator(0.1f, 100f, iTextSharp.text.BaseColor.GRAY, Element.ALIGN_LEFT, 1);



        //        PdfPTable maintable2 = new PdfPTable(1);
        //        maintable2.SpacingBefore = 5f;
        //        maintable2.DefaultCell.Border = Rectangle.NO_BORDER;
        //        PdfPCell cell5 = new PdfPCell();
        //        cell5.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell5.BorderWidth = 0;
        //        cell5.AddElement(p3);
        //        maintable2.SpacingAfter = 5f;
        //        maintable2.AddCell(cell5);

        //        PdfPTable maintable3 = new PdfPTable(1);
        //        maintable3.SpacingBefore = 5f;
        //        maintable3.DefaultCell.Border = Rectangle.NO_BORDER;
        //        PdfPCell cell6 = new PdfPCell();
        //        cell6.PaddingLeft = 30f;

        //        cell6.BorderWidth = 0;
        //        cell6.AddElement(png);
        //        maintable3.SpacingAfter = 5f;
        //        maintable3.AddCell(cell6);

        //        Paragraph c1 = new Paragraph();
        //        c1.Font = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD);
        //        c1.IndentationLeft = 10f;
        //        c1.Add("Ship To: ");
        //        Paragraph c2 = new Paragraph();
        //        c2.IndentationLeft = 70;
        //        c2.Font = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD);
        //        c2.Add("www.mylaporeexpress.com");
        //        c2.Add("\n");
        //        Paragraph c3 = new Paragraph();
        //        c3.IndentationLeft = 80;
        //        c3.Font = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD);
        //        c3.Add("Toll Free:9169256200");



        //        Paragraph l2 = new Paragraph();
        //        l2.IndentationLeft = 12f;
        //        l2.SpacingBefore = 5f;
        //        l2.Font = FontFactory.GetFont("Arial", 12);
        //        l2.Add(getlabel.shipment.ship_to.name);
        //        l2.Add("\n");
        //        l2.Add(getlabel.shipment.ship_to.address_line1);
        //        l2.Add("\n");
        //        var address = getlabel.shipment.ship_to.city_locality + " " + getlabel.shipment.ship_to.state_province + " " + getlabel.shipment.ship_to.postal_code;
        //        Paragraph l3 = new Paragraph();
        //        l3.IndentationLeft = 12f;
        //        l3.SpacingBefore = 5f;
        //        l3.Font = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD);
        //        l3.Add(address);
        //        l3.SpacingAfter = 14f;

        //        PdfPTable maintable4 = new PdfPTable(2);
        //        maintable4.WidthPercentage = 100f;
        //        maintable4.SpacingBefore = 5f;
        //        maintable4.DefaultCell.Border = Rectangle.NO_BORDER;
        //        PdfPCell cell7 = new PdfPCell();

        //        //cell7.BorderWidth = 0;
        //        //cell7.AddElement(l1);
        //        //maintable4.SpacingAfter = 5f;
        //        //maintable4.AddCell(cell7);
        //        cell7.BorderWidth = 0;
        //        cell7.AddElement(c1);
        //        cell7.AddElement(l2);
        //        maintable4.AddCell(cell7);

        //        cell7 = new PdfPCell();
        //        cell7.BorderWidth = 0;
        //        cell7.AddElement(p2);
        //        cell7.PaddingTop = 20f;
        //        cell7.Rowspan = 2;
        //        maintable4.AddCell(cell7);
        //        // cell7 = new PdfPCell();
        //        //cell7.BorderWidth = 0;
        //        //cell7.AddElement(c1);
        //        //cell7.AddElement(l2);
        //        //maintable4.AddCell(cell7);
        //        cell7 = new PdfPCell();
        //        cell7.BorderWidth = 0;
        //        cell7.AddElement(l3);
        //        maintable4.AddCell(cell7);

        //        doc.Add(maintable);
        //        // doc.Add(maintable2);
        //        doc.Add(line);
        //        doc.Add(maintable4);
        //        doc.Add(line);
        //        doc.Add(maintable3);
        //        //doc.Add(line);
        //        doc.Add(c2);
        //        doc.Add(c3);
        //        //doc.NewPage();
        //        //i++;
        //        // }

        //        doc.Close();
        //        byte[] bytes = System.IO.File.ReadAllBytes(path_pdf + @"Pdf/" + pdfname + ".pdf");
        //        //var pdfPath = Path.Combine(Server.MapPath(path_name));
        //        Session["pdfData"] = bytes;
        //        //response.DeliveryDate = null;
        //        //response.ServiceType = ""
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { success = false });
        //    }
        //    return Json(new { success = true });
        //}

        public ActionResult InhouseLabel(OneposStamps.Models.CreateLabelRequest.CreateLabelRequest getlabel = null, string StoreId = null, string OrderId = null, bool _download = false)
        {
            Session["pdfData"] = null;
            OneposStamps.Models.CreateLabelRequest.CreateLabelResponse response = new OneposStamps.Models.CreateLabelRequest.CreateLabelResponse();
            try
            {
                string path = null;
                string base64String = null;
                getlabel.TodayDate = DateTime.Now.ToString("MM-dd-yyyy");
                getlabel.OrderId = OrderId;
                if (StoreId == "d73add35-876a-4c82-82f9-9591baf2c20d")
                {
                    path = Server.MapPath("~/Content/assets/images/logo.png");
                }
                else if (StoreId == "f575a340-44a8-4f68-b5fc-efba3350a264")
                {
                    path = Server.MapPath("~/Content/StoreLogo/png/Mylapore Logo – 2.png");
                }
                else if (StoreId == "2cfb7b87-3e7d-486f-b14a-356730689fbd")
                {
                    path = Server.MapPath("~/Images/logo-pdf.png");
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
                            getlabel.logoBase64String = Convert.ToBase64String(imageBytes);
                            //return base64String;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                try
                {
                    getlabel.barcodeBase64String = LoadImageForBarode(OrderId);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
            if (_download)
            {
                return Json(getlabel, JsonRequestBehavior.AllowGet);
            }
            return PartialView("_InHouseLabel", getlabel);
        }

        [HttpPost]
        public ActionResult getpdfHtmlStringData(string Html)
        {
            try
            {
                Session["pdfData"] = null;
                byte[] stream = null;
                iTextSharp.text.Font blackFont = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                var htmlToPdfConverter = new HtmlToPdfConverter();
                htmlToPdfConverter.Margins = new PageMargins { Left = 5, Right = 5 };
                var str = htmlToPdfConverter.GeneratePdf(Html);
                using (var mstr = new MemoryStream())
                {
                    PdfReader reader = new PdfReader(str);
                    int totalPages = reader.NumberOfPages;
                    using (PdfStamper stamper = new PdfStamper(reader, mstr))
                    {
                        for (int i = 1; i <= totalPages; i++)
                        {
                            ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                @Element.ALIGN_BOTTOM, new Phrase("Page " + i.ToString() + " of " + totalPages, blackFont), 300f, 4f, 0);
                        }
                    }
                    stream = mstr.ToArray();
                    Session["pdfData"] = stream;
                }
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });

        }

        public ActionResult OrderShipmentDetails(string StoreId, string OrderId = "", string ZoneId = "", string DeliverDate = null)
        {
            OrderDetails od = new OrderDetails();
            DbDetails dbdetails = db.GetDbDetails(StoreId);
            od.StoreId = StoreId;
            od.OrderId = OrderId;
            od.SelectedZoneId = ZoneId;
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
                orderdetais.shippingPaid = Convert.ToDecimal(row["DeliveryChargeAmt"]);
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
                ShippingDetails.state = (row["State"]).ToString().Trim();
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
                a.CarrierId = (row["CarrierId"]).ToString();
                a.Service_Code = (row["service_Code"]).ToString();
                a.INHouse = Convert.ToBoolean(row["INHouse"]);

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

            var Zonename = string.Empty;
            if (!string.IsNullOrWhiteSpace(od.SelectedZoneId))
            {
                Zonename = od.ZoneList.Where(x => x.ZoneId == od.SelectedZoneId).Select(y => y.ZoneName).FirstOrDefault();
                if (Zonename.ToLower() == "Countrywide".ToLower())
                {
                    od.SelectedServiceId = od.ServiceList.Where(x => x.Service_Code == "ups_next_day_air_saver").Select(y => y.Id).FirstOrDefault();
                }
                else if (Zonename.ToLower() == "SoCal".ToLower())
                {
                    od.SelectedServiceId = od.ServiceList.Where(x => x.Service_Code == "ups_ground").Select(y => y.Id).FirstOrDefault();
                }
                else if (Zonename.ToLower() == "ME Van".ToLower())
                {
                    od.SelectedServiceId = od.ServiceList.Where(x => x.Service_Code == "mylapore_express").Select(y => y.Id).FirstOrDefault();
                }
            }
            else
            {
                od.SelectedServiceId = od.ServiceList.Where(x => x.Service_Code == "ups_ground").Select(y => y.Id).FirstOrDefault();
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
            try
            {
                DbDetails dbdetails = db.GetDbDetails(StoreId);


                //AuthenticateUserResponse response = GetAuthentication(dbdetails, "AuthenticateUser");
                Addressrequest.AuthenticationId = dbdetails.IntegrationId;
                Addressresponse = CleanseAddress(Addressrequest, "CleanseAddress");
                return Json(Addressresponse.AddressMatched);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetShipRates(string StoreId, GetRates getrates = null)
        {
            try
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
                // AuthenticateUserResponse response = GetAuthentication(dbdetails, "AuthenticateUser");
                //getrates.AuthenticationId = response.Authenticator;
                GetRatesResponse val = GetRates(getrates, dbdetails.IntegrationId);

                decimal RateAmount = 0;
                if (val.Amount != 0)
                {
                    RateAmount = Math.Round(Convert.ToDecimal(val.Amount), 2);
                }

                return Json(val);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400, "Sorry, This service is not available for this route."); // Bad Request
                //return Json("Sorry, This service is not availabl for this route.");
            }
        }

        public GetRatesResponse GetRates(GetRates getrates, string AuthenticationId)
        {
            GetRatesResponse Responses = new GetRatesResponse();
            var client = new RestClient("https://api.shipengine.com/v1/rates");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("API-Key", AuthenticationId);
            request.AddHeader("Content-Type", "application/json");

            if (getrates.shipment.ship_from.country_code.ToLower() == "united states" || getrates.shipment.ship_from.country_code.ToLower() == "usa" || getrates.shipment.ship_from.country_code.ToLower() == "unitedstates" || getrates.shipment.ship_from.country_code.ToLower() == "us")
            {
                getrates.shipment.ship_from.country_code = "US";
            }

            if (getrates.shipment.ship_to.country_code.ToLower() == "united states" || getrates.shipment.ship_to.country_code.ToLower() == "usa" || getrates.shipment.ship_to.country_code.ToLower() == "unitedstates" || getrates.shipment.ship_to.country_code.ToLower() == "us")
            {
                getrates.shipment.ship_to.country_code = "US";
            }
            var body = JsonConvert.SerializeObject(getrates);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            try
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string result = response.Content;
                    OneposStamps.Models.GetRateResponse.Root value1 = JsonConvert.DeserializeObject<OneposStamps.Models.GetRateResponse.Root>(result);
                    OneposStamps.Models.GetRateResponse.RateResponse rateResponse = value1.rate_response;
                    OneposStamps.Models.GetRateResponse.Rate rate = rateResponse.rates.FirstOrDefault();
                    Responses.Amount = rate.confirmation_amount.amount + rate.insurance_amount.amount + rate.shipping_amount.amount + rate.other_amount.amount;
                    Responses.Deliverydate = rate.estimated_delivery_date.ToString("MM-dd-yyyy HH:mm:ss");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Responses;
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

        public ActionResult GetLabels(string StoreId, OneposStamps.Models.CreateLabelRequest.CreateLabelRequest getlabel = null)
        {
            try
            {

                DbDetails dbdetails = db.GetDbDetails(StoreId);
                //getlabel.IntegratorTxID = ;
                //getlabel.FromFullName = "SWSIM API";
                //getlabel.Fromaddress = "1990 E GRAND AVE";
                //getlabel.FromCity = "EL SEGUNDO";
                //getlabel.FromState = "CA";
                //getlabel.FromZIPCode = "90245";
                if (getlabel.shipment.ship_from.country_code.ToLower() == "united states" || getlabel.shipment.ship_from.country_code.ToLower() == "usa" || getlabel.shipment.ship_from.country_code.ToLower() == "unitedstates" || getlabel.shipment.ship_from.country_code.ToLower() == "us")
                {
                    getlabel.shipment.ship_from.country_code = "US";
                }
                //getlabel.ToFullName = "SWSIM API";
                //getlabel.Toaddress = "1990 E GRAND AVE";
                //getlabel.ToCity = "EL SEGUNDO";
                //getlabel.ToState = "CA";
                //getlabel.ToZIPCode = "90245";
                if (getlabel.shipment.ship_to.country_code.ToLower() == "united states" || getlabel.shipment.ship_to.country_code.ToLower() == "usa" || getlabel.shipment.ship_to.country_code.ToLower() == "unitedstates" || getlabel.shipment.ship_to.country_code.ToLower() == "us")
                {
                    getlabel.shipment.ship_to.country_code = "US";
                }
                //getlabel.WeightLb = 0;
                //getlabel.WeightOz = 1;
                //getlabel.PackageType = "Package";
                //getlabel.shipdate = "2021-10-11";
                //DateTime shipDate = DateTime.Now.AddDays(1);
                //getlabel.shipdate = shipDate.ToString("yyyy/MM/dd");
                OneposStamps.Models.CreateLabelRequest.CreateLabelResponse res = GetLabel(getlabel, dbdetails.IntegrationId);
                //DateTime del = DateTime.ParseExact(res.DeliveryDate, "yyyy/MM/dd", null);
                //res.DeliveryDate = del.ToString("MM/dd/yyyy");
                //res.ShipDate = shipDate.ToString("MM/dd/yyyy");
                //res.ZoneName = getlabel.ZoneName;
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
                    //PdfDocument doc = converter.ConvertUrl(url);
                    string pdfFilePath = url;
                    WebClient myClient = new WebClient();
                    byte[] bytes = myClient.DownloadData(url);
                    //byte[] bytes = System.IO.File.ReadAllBytes(pdfFilePath);
                    // save pdf document
                    // byte[] pdf = doc.Save();

                    // close pdf document
                    //doc.Close();
                    Session["pdfData"] = bytes;
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual ActionResult DownloadPDF(string fileName)
        {
            string fullFileName = fileName + "_" + DateTime.Now.ToFileTime() + ".pdf";
            return File((byte[])Session["pdfData"], "application/pdf", fullFileName);
        }

        public AuthenticateUserResponse GetAuthentication(DbDetails dbdetails, string Apiname)
        {
            AuthenticateUserResponse Responses = new AuthenticateUserResponse();
            ////Calling CreateSOAPWebRequest method  
            //HttpWebRequest request = CreateSOAPWebRequest(Apiname);

            //XmlDocument SOAPReqBody = new XmlDocument();
            ////SOAP Body Request  
            //SOAPReqBody.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>  
            //<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""
            //    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
            //    xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">  
            // <soap:Body> 
            //    <AuthenticateUser xmlns=""http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111"">  
            //    <Credentials>
            //      <IntegrationID>" + dbdetails.IntegrationId + @"</IntegrationID>  
            //      <Username>" + dbdetails.StampsUserName + @"</Username> 
            //      <Password>" + dbdetails.StampsUserPassword + @"</Password>
            //     </Credentials>
            //    </AuthenticateUser>
            //  </soap:Body>  
            //</soap:Envelope>");


            //using (Stream stream = request.GetRequestStream())
            //{
            //    SOAPReqBody.Save(stream);
            //}
            ////Geting response from request  
            //using (WebResponse Serviceres = request.GetResponse())
            //{
            //    using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
            //    {
            //        //reading stream  
            //        var ServiceResult = rd.ReadToEnd();
            //        //var otp = JsonConvert.DeserializeObject<AuthenticateUser>(ServiceResult);
            //        XDocument doc = XDocument.Parse(ServiceResult);
            //        XNamespace ns = "http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111";
            //        IEnumerable<XElement> responses = doc.Descendants(ns + "AuthenticateUserResponse");
            //        foreach (XElement response in responses)
            //        {
            //            var value = (string)response.Element(ns + "Authenticator");
            //            Responses.Authenticator = value.Replace("&", "&amp;");

            //        }

            //    }
            //}

            return Responses;

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

            var body = JsonConvert.SerializeObject(getlabel);
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

        //public GetRatesResponse GetRates(GetRates getrates, string Apiname)
        //{
        //    GetRatesResponse Responses = new GetRatesResponse();
        //    //Calling CreateSOAPWebRequest method  
        //    HttpWebRequest request = CreateSOAPWebRequest(Apiname);

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
        //        <ServiceType>" + getrates.servicetype + @"</ServiceType>
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
        //    try
        //    {
        //        using (WebResponse Serviceres = request.GetResponse())
        //        {
        //            using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
        //            {
        //                //reading stream  
        //                var ServiceResult = rd.ReadToEnd();
        //                //var otp = JsonConvert.DeserializeObject<AuthenticateUser>(ServiceResult);
        //                XDocument doc = XDocument.Parse(ServiceResult);
        //                XNamespace ns = "http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111";
        //                IEnumerable<XElement> responses = doc.Descendants(ns + "GetRatesResponse");

        //                IEnumerable<XElement> responses1 = responses.Descendants(ns + "Rates");
        //                IEnumerable<XElement> responses2 = responses1.Descendants(ns + "Rate");
        //                bool amountvalue = true;
        //                foreach (XElement response in responses2)
        //                {
        //                    if (amountvalue)
        //                    {
        //                        Responses.Amount = (string)response.Element(ns + "Amount");
        //                        amountvalue = false;
        //                    }

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Responses.Amount = "0";
        //        return Responses;
        //    }

        //    return Responses;
        //}

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
            AddressVerifyResponse Responses = new AddressVerifyResponse(); string name = Addressrequest.name;

            var client = new RestClient("https://api.shipengine.com/v1/addresses/validate");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("API-Key", Addressrequest.AuthenticationId);
            request.AddHeader("Content-Type", "application/json");
            const string quote = "\"";
            var body = @"[" + "\n" +
            @"    {" + "\n" +
            @"        ""name"": " + quote + "" + Addressrequest.name + "" + quote + "," + "\n" +
            @"        ""address_line1"": " + quote + "" + Addressrequest.address1 + "" + quote + "," + "\n" +
            @"        ""city_locality"": " + quote + "" + Addressrequest.city + "" + quote + "," + "\n" +
            @"        ""state_province"": " + quote + "" + Addressrequest.state + "" + quote + "," + "\n" +
            @"        ""postal_code"": " + quote + "" + Addressrequest.zipcode + "" + quote + "," + "\n" +
            @"        ""country_code"": ""US""" + "\n" +
            @"    }" + "\n" +
            @"]";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            // Console.WriteLine(response.Content);
            string result = response.Content;
            List<ShipengineAddressResponse> value1 = JsonConvert.DeserializeObject<List<ShipengineAddressResponse>>(result);
            var value = value1.FirstOrDefault();
            Responses.AddressMatched = value.status;

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

        //public HttpWebRequest CreateSOAPWebRequest(string Apiname)
        //{
        //    //Making Web Request  
        //    HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"https://swsim.testing.stamps.com/swsim/swsimv111.asmx");
        //    //SOAPAction  
        //    Req.Headers.Add(@"SOAPAction:http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111/" + Apiname + "");//AuthenticateUser
        //                                                                                                           //Content_type  
        //                                                                                                           // Req.ContentLength = "length";
        //    Req.ContentType = "text/xml;charset=\"utf-8\"";
        //    Req.Accept = "text/xml";
        //    //HTTP method  
        //    Req.Method = "POST";
        //    Req.Host = "swsim.testing.stamps.com";

        //    //return HttpWebRequest  
        //    return Req;
        //}

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
        //public HttpWebRequest CleanseAddressSOAPWebRequest()
        //{
        //    //Making Web Request  
        //    HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"https://swsim.testing.stamps.com/swsim/swsimv111.asmx");
        //    //SOAPAction  
        //    Req.Headers.Add(@"SOAPAction:http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111/CleanseAddress");
        //    Req.ContentType = "text/xml;charset=\"utf-8\"";
        //    Req.Accept = "text/xml";
        //    //HTTP method  
        //    Req.Method = "POST";
        //    Req.Host = "swsim.testing.stamps.com";

        //    //return HttpWebRequest  
        //    return Req;
        //}
        //public HttpWebRequest GetRatesSOAPWebRequest()
        //{
        //    //Making Web Request  
        //    HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"https://swsim.testing.stamps.com/swsim/swsimv111.asmx");
        //    //SOAPAction  
        //    Req.Headers.Add(@"SOAPAction:http://stamps.com/xml/namespace/2021/01/swsim/SwsimV111/GetRates");
        //    Req.ContentType = "text/xml;charset=\"utf-8\"";
        //    Req.Accept = "text/xml";
        //    //HTTP method  
        //    Req.Method = "POST";
        //    Req.Host = "swsim.testing.stamps.com";

        //    //return HttpWebRequest  
        //    return Req;
        //}

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
                        base64String = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                    }

                }
            }
            return base64String;
        }

        public ActionResult CreateMultiLabel(OrderIdList OrderIdList)
        {
            List<String> shipmentids = new List<string>();
            DbDetails dbdetails = db.GetDbDetails(OrderIdList.StoreId);

            DataSet ds1 = db.GetMysqlDataSet("USP_GetWareHouseId", OrderIdList.StoreId); //StoreId
            var WareHouse_Id = string.Empty;
            var BatchId = string.Empty;
            var Pdf = string.Empty;
            foreach (DataRow row in ds1.Tables[0].Rows)
            {

                WareHouse_Id = (row["WareHouseId"]).ToString();
            }
          
            foreach (var o in OrderIdList.OrderIds)
            {
               
                DataSet ds = db.GetOrderShippingDetails("USP_GetOrderShippingDetails", OrderIdList.StoreId, o.OrderId, dbdetails.Address, dbdetails.Password, dbdetails.DatabaseName, dbdetails.Username);

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
                    orderdetais.shippingPaid = Convert.ToDecimal(row["DeliveryChargeAmt"]);
                    orderdetais.taxPaid = Convert.ToDecimal(row["TaxPaid"]);
                    orderdetais.productTotal = Convert.ToDecimal(row["ProductTotal"]);
                    orderdetais.totalOrder = Convert.ToDecimal(row["TotalOrder"]);
                    orderdetais.totalPaid = Convert.ToDecimal(row["TotalPaid"]);
                    orderdetais.holdUntil = "N/A";
                    orderdetais.TransactionId = (row["TransactionId"]).ToString();
                    orderdetais.OrderNotes = ds.Tables[0].Columns.Contains("OrderNotes") ? (row["OrderNotes"]).ToString() : null;


                }
                OneposStamps.Models.ShipmentRequest.ShipTo ShippingDetails = new OneposStamps.Models.ShipmentRequest.ShipTo();
                foreach (DataRow row in ds.Tables[1].Rows)
                {

                    ShippingDetails.name = (row["DeliveryName"]).ToString();
                    ShippingDetails.address_line1 = (row["Adress"]).ToString();
                    ShippingDetails.phone = (row["Phone"]).ToString();
                    ShippingDetails.city_locality = (row["City"]).ToString();
                    ShippingDetails.state_province = (row["State"]).ToString().Trim();
                    ShippingDetails.postal_code = (row["ZipCode"]).ToString();
                    ShippingDetails.country_code = (row["Country"]).ToString();
                    ShippingDetails.address_residential_indicator = "Yes";
                   
                }
                OneposStamps.Models.ShipmentRequest.AdvancedOptions advancedoptions = new Models.ShipmentRequest.AdvancedOptions();
                List<object> tag = new List<object>();
                OneposStamps.Models.ShipmentRequest.Root request = new OneposStamps.Models.ShipmentRequest.Root();
                List<OneposStamps.Models.ShipmentRequest.Shipment> r1 = new List<OneposStamps.Models.ShipmentRequest.Shipment>();
                OneposStamps.Models.ShipmentRequest.Shipment s1 = new OneposStamps.Models.ShipmentRequest.Shipment();
                s1.warehouse_id = WareHouse_Id;
                s1.validate_address = "no_validation";
                s1.service_code = "ups_next_day_air_saver";
                s1.external_shipment_id = Guid.NewGuid().ToString();
                s1.confirmation = "none";
                s1.insurance_provider = "none";
                List<OneposStamps.Models.ShipmentRequest.Package> packagelist = new List<Models.ShipmentRequest.Package>();
                OneposStamps.Models.ShipmentRequest.Package p = new Models.ShipmentRequest.Package();
                OneposStamps.Models.ShipmentRequest.Weight w = new Models.ShipmentRequest.Weight();
              
                w.unit = "ounce";
                w.value= 1.0;
                p.weight = w;
                s1.ship_to = ShippingDetails;
                packagelist.Add(p);
                s1.packages = packagelist;
                s1.advanced_options = advancedoptions;
                s1.tags = tag;
                r1.Add(s1);
                request.shipments=r1;

                var body = JsonConvert.SerializeObject(request);
                var client = new RestClient("https://api.shipengine.com/v1/shipments");
                client.Timeout = -1;
                var apirequest = new RestRequest(Method.POST);
                apirequest.AddHeader("API-Key", dbdetails.IntegrationId);
                apirequest.AddHeader("Content-Type", "application/json");
                apirequest.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(apirequest);
                try
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string result = response.Content;
                        OneposStamps.Models.ShipmentResponse.Root resp = JsonConvert.DeserializeObject <OneposStamps.Models.ShipmentResponse.Root>(result);
                        OneposStamps.Models.ShipmentResponse.Shipment ship1 = resp.shipments.FirstOrDefault();
                        shipmentids.Add(ship1.shipment_id);                      
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                

            }
            OneposStamps.Models.CreateBatchRequest.Root batchrequest = new OneposStamps.Models.CreateBatchRequest.Root();
            List<object> rateids = new List<object>();
            batchrequest.external_batch_id = Guid.NewGuid().ToString();
            batchrequest.batch_notes = DateTime.Now.ToString("MM-dd-yyyy");
            batchrequest.shipment_ids = shipmentids;
            batchrequest.rate_ids = rateids;
            var batchrequestbody = JsonConvert.SerializeObject(batchrequest);
            var batchrequestclient = new RestClient("https://api.shipengine.com/v1/batches");
            batchrequestclient.Timeout = -1;
            var batchrequestapirequest = new RestRequest(Method.POST);
            batchrequestapirequest.AddHeader("API-Key", dbdetails.IntegrationId);
            batchrequestapirequest.AddHeader("Content-Type", "application/json");
            batchrequestapirequest.AddParameter("application/json", batchrequestbody, ParameterType.RequestBody);
            IRestResponse batchrequestapiresponse = batchrequestclient.Execute(batchrequestapirequest);
            try
            {
                if (batchrequestapiresponse.StatusCode == HttpStatusCode.OK)
                {
                    string result = batchrequestapiresponse.Content;
                    OneposStamps.Models.CreateBatchResponse.Root resp = JsonConvert.DeserializeObject<OneposStamps.Models.CreateBatchResponse.Root>(result);
                    BatchId = resp.batch_id;
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            OneposStamps.Models.LabelProcessRequest.Root lpr = new OneposStamps.Models.LabelProcessRequest.Root();
            CultureInfo provider = CultureInfo.InvariantCulture;
            lpr.label_format = "pdf";
            lpr.label_layout = "4x6";
            lpr.ship_date = DateTime.Now; //DateTime.ParseExact(OrderIdList.DeliverDate, "MM/dd/yyyy", provider);
            var LabelProcessbody = JsonConvert.SerializeObject(lpr);
            var LabelProcessclient = new RestClient("https://api.shipengine.com/v1/batches/"+BatchId+"/process/labels");
            batchrequestclient.Timeout = -1;
            var LabelProcessapirequest = new RestRequest(Method.POST);
            LabelProcessapirequest.AddHeader("API-Key", dbdetails.IntegrationId);
            LabelProcessapirequest.AddHeader("Content-Type", "application/json");
            LabelProcessapirequest.AddParameter("application/json", LabelProcessbody, ParameterType.RequestBody);
            IRestResponse LabelProcessapiresponse = LabelProcessclient.Execute(LabelProcessapirequest);
            try
            {
                if (batchrequestapiresponse.StatusCode == HttpStatusCode.NoContent)
                {
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            var Getlabelclient = new RestClient("https://api.shipengine.com/v1/batches/" + BatchId +"/");
            Getlabelclient.Timeout = -1;
            var Getlabelrequest = new RestRequest(Method.GET);
            Getlabelrequest.AddHeader("API-Key", dbdetails.IntegrationId);
            Getlabelrequest.AddHeader("Content-Type", "application/json");
            Getlabelrequest.AddParameter("application/json", "", ParameterType.RequestBody);
            IRestResponse Getlabelresponse = Getlabelclient.Execute(Getlabelrequest);
            try
            {
                if (Getlabelresponse.StatusCode == HttpStatusCode.OK)
                {
                    string result = Getlabelresponse.Content;
                    OneposStamps.Models.GetBatchResponse.Root resp = JsonConvert.DeserializeObject<OneposStamps.Models.GetBatchResponse.Root>(result);
                    OneposStamps.Models.GetBatchResponse.LabelDownload label = resp.label_download;
                    Pdf = label.pdf;


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return null;
        }



        //public ActionResult CreateMultiLabel(OrderIdList OrderIdList)
        //{
        //    List<string> uncreatedLabel = new List<string>();
        //    foreach (var item in OrderIdList.OrderIds)
        //    {
        //        var shipDetails = OrderShipmentDetails(OrderIdList.StoreId, item, OrderIdList.DeliverDate);

        //        OrderDetails shipResult = db.ModelFromActionResult<OrderDetails>(shipDetails);

        //        AddressVerifyRequest addressVerifyRequest = new AddressVerifyRequest();

        //        if (shipResult.BuyersDetails != null)
        //        {
        //            addressVerifyRequest.name = shipResult.BuyersDetails.name;
        //            addressVerifyRequest.address1 = shipResult.BuyersDetails.address;
        //            addressVerifyRequest.city = shipResult.BuyersDetails.city;
        //            addressVerifyRequest.state = shipResult.BuyersDetails.state;
        //            addressVerifyRequest.zipcode = shipResult.BuyersDetails.zipcode;
        //        }

        //        var objAddressVerified = CheckAddress(OrderIdList.StoreId, addressVerifyRequest);

        //        bool addressVerifiedStatus = false;

        //        if (objAddressVerified.GetType() == typeof(JsonResult))
        //        {
        //            JsonResult jsonResultAddress = (JsonResult)objAddressVerified;
        //            if (jsonResultAddress.Data.ToString() == "true" || jsonResultAddress.Data.ToString() == "false")
        //            {
        //                addressVerifiedStatus = jsonResultAddress.Data != null ? Convert.ToBoolean(jsonResultAddress.Data) : false;
        //            }
        //        }

        //        GetRates getrates = new GetRates();

        //        getrates.fromZipcode = shipResult.orderSummary.StoreZipcode;
        //        getrates.toZipcode = shipResult.BuyersDetails.zipcode;
        //        getrates.PackageType = "Package";
        //        //DateTime shipDate = DateTime.ParseExact(getrates.shipdate, "MM/dd/yyyy", null).AddDays(1);
        //        DateTime shipDate = DateTime.Now.AddDays(1);
        //        getrates.shipdate = shipDate.ToString("yyyy/MM/dd");
        //        getrates.WeightLb = 10;
        //        getrates.WeightOz = 5;

        //        var shipRateDetails = GetShipRates(OrderIdList.StoreId, getrates);

        //        decimal shipRateAmount = 0;

        //        if (shipRateDetails.GetType() == typeof(JsonResult))
        //        {
        //            JsonResult jsonResultShiprate = (JsonResult)shipRateDetails;
        //            shipRateAmount = jsonResultShiprate.Data != null ? Convert.ToDecimal(jsonResultShiprate.Data) : 0;
        //        }

        //        CreateLabelRequest getlabel = new CreateLabelRequest();

        //        getlabel.IntegratorTxID = shipResult.orderSummary.TransactionId;
        //        getlabel.FromFullName = shipResult.orderSummary.storeName;
        //        getlabel.Fromaddress = shipResult.orderSummary.StoreAddress;
        //        getlabel.FromCity = shipResult.orderSummary.StoreCity;
        //        getlabel.FromState = shipResult.orderSummary.StoreState;
        //        getlabel.FromZIPCode = shipResult.orderSummary.StoreZipcode;
        //        getlabel.FromCountry = shipResult.orderSummary.StoreCountry;
        //        if (getlabel.FromCountry.ToLower() == "united states" || getlabel.FromCountry.ToLower() == "usa" || getlabel.FromCountry.ToLower() == "unitedstates" || getlabel.FromCountry.ToLower() == "us")
        //        {
        //            getlabel.FromCountry = "US";
        //        }

        //        getlabel.ToFullName = shipResult.BuyersDetails.name;
        //        getlabel.Toaddress = shipResult.BuyersDetails.address;
        //        getlabel.ToCity = shipResult.BuyersDetails.city;
        //        getlabel.ToState = shipResult.BuyersDetails.state;
        //        getlabel.ToZIPCode = shipResult.BuyersDetails.zipcode;
        //        getlabel.ToCountry = shipResult.BuyersDetails.country;
        //        if (getlabel.ToCountry.ToLower() == "united states" || getlabel.ToCountry.ToLower() == "usa" || getlabel.ToCountry.ToLower() == "unitedstates" || getlabel.ToCountry.ToLower() == "us")
        //        {
        //            getlabel.ToCountry = "US";
        //        }

        //        getlabel.WeightLb = 10;
        //        getlabel.WeightOz = 5;
        //        getlabel.PackageType = "Package";
        //        DateTime shipDate2 = DateTime.Now.AddDays(1);
        //        getlabel.shipdate = shipDate2.ToString("yyyy/MM/dd");
        //        getlabel.Length = "1";
        //        getlabel.Width = "1";
        //        getlabel.Height = "1";
        //        getlabel.ServiceType = "US-PM";// "US-FC";
        //        getlabel.Amount = shipRateAmount.ToString();

        //        var getLabelDetails = GetLabels(OrderIdList.StoreId, getlabel);

        //        CreateLabelResponse labelRespose = new CreateLabelResponse();
        //        dynamic LabelData;
        //        if (getLabelDetails.GetType() == typeof(JsonResult))
        //        {
        //            JsonResult jsonResultLabel = (JsonResult)getLabelDetails;

        //            if (jsonResultLabel.Data != null)
        //            {
        //                LabelData = jsonResultLabel.Data;
        //                labelRespose = LabelData;
        //            }
        //        }

        //        if (labelRespose.Url != null)
        //        {
        //            string url = labelRespose.Url;

        //            string pdf_page_size = PdfPageSize.A4.ToString();
        //            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
        //                pdf_page_size, true);

        //            string pdf_orientation = PdfPageOrientation.Portrait.ToString();
        //            PdfPageOrientation pdfOrientation =
        //                (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
        //                pdf_orientation, true);

        //            int webPageWidth = 1024;
        //            try
        //            {
        //                //webPageWidth = Convert.ToInt32(TxtWidth.Text);
        //            }
        //            catch { }

        //            int webPageHeight = 0;
        //            try
        //            {
        //                //webPageHeight = Convert.ToInt32(TxtHeight.Text);
        //            }
        //            catch { }

        //            // instantiate a html to pdf converter object
        //            HtmlToPdf converter = new HtmlToPdf();

        //            // set converter options
        //            converter.Options.PdfPageSize = pageSize;
        //            converter.Options.PdfPageOrientation = pdfOrientation;
        //            converter.Options.WebPageWidth = webPageWidth;
        //            converter.Options.WebPageHeight = webPageHeight;

        //            // create a new pdf document converting an url
        //            PdfDocument doc = converter.ConvertUrl(url);

        //            // save pdf document
        //            byte[] pdf = doc.Save();

        //            // close pdf document
        //            doc.Close();

        //            // return resulted pdf document
        //            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
        //            fileResult.FileDownloadName = "Document.pdf";


        //            //string base64String = "data: application/pdf; base64, " + Convert.ToBase64String(pdf);
        //            //return Json(pdf);
        //            //return File(pdf, "application/pdf");
        //            return fileResult;
        //        }
        //        else
        //        {
        //            uncreatedLabel.Add(item);                    
        //        }

        //    }
        //    return null;
        //}

        public ActionResult getPdf(OneposStamps.Models.CreateLabelRequest.CreateLabelRequest getlabel = null, string StoreId = null, string OrderId = null, string viewPath = null)
        {
            byte[] stream = null;
            string fileName = string.Empty;
            var filePath = string.Empty;
            try
            {
                dynamic model = null;
                string view;
                model = (OneposStamps.Models.CreateLabelRequest.CreateLabelRequest)((JsonResult)new OrderController().InhouseLabel(getlabel, StoreId, OrderId, true)).Data;
                fileName = $"MasterReport_{fileName}";
                model.type = "pdf";
                iTextSharp.text.Font blackFont = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                ViewData.Model = model;
                using (var writer = new StringWriter())
                {
                    var vResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewPath);
                    var vContext = new ViewContext(ControllerContext, vResult.View, ViewData,
                        new TempDataDictionary(), writer);
                    vResult.View.Render(vContext, writer);
                    view = writer.ToString();
                }
                var htmlToPdfConverter = new HtmlToPdfConverter();
                htmlToPdfConverter.Margins = new PageMargins { Left = 5, Right = 5 };
                var str = htmlToPdfConverter.GeneratePdf(view);
                using (var mstr = new MemoryStream())
                {
                    PdfReader reader = new PdfReader(str);
                    int totalPages = reader.NumberOfPages;
                    using (PdfStamper stamper = new PdfStamper(reader, mstr))
                    {
                        for (int i = 1; i <= totalPages; i++)
                        {
                            ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                @Element.ALIGN_BOTTOM, new Phrase("Page " + i.ToString() + " of " + totalPages, blackFont), 300f, 4f, 0);
                        }
                    }
                    stream = mstr.ToArray();
                }
                Session["ReportList"] = stream;
            }
            catch (Exception ex)
            {

            }
            return Json(new { file = $"{fileName}.pdf" }, JsonRequestBehavior.AllowGet);
        }
    }
}