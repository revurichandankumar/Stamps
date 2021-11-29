using Newtonsoft.Json;
using OneposStamps.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;

namespace OneposStamps.Controllers
{
    [RoutePrefix("GetZone")]
    public class GetZoneApiController : BaseApiController
    {
        [HttpGet]
        [Route("GetZones")]
        public async Task<HttpResponseMessage> GetZones(string store_Id, string zipCode)
        {
            HttpResponseMessage httpResponseMessage;
            //try
            //{
                
                if (!string.IsNullOrWhiteSpace(zipCode))
                {

                    DataSet ds = db.GetZoneData("USP_GetZoneData", store_Id, zipCode);
                GetZonesResponse GetZoneResponse = new GetZonesResponse();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {


                        GetZoneResponse.ZoneId = (row["ZoneId"]).ToString();
                        GetZoneResponse.Shipmentfee = Convert.ToDecimal((row["ShipmentFee"]));
                        GetZoneResponse.Zonename= (row["Zonename"]).ToString();



                        }
                        
                    }
                    else
                    {
                    httpResponseMessage = new HttpResponseMessage()
                    {
                        Content = new ObjectContent<object>(new { message = "Please Enter valid Zipcode" }, new JsonMediaTypeFormatter()),
                        StatusCode = HttpStatusCode.NotFound
                    };
                    return httpResponseMessage;

                    }
                    httpResponseMessage = new HttpResponseMessage()
                    {
                        Content = new ObjectContent<object>(new { message = new { GetZoneResponse } }, new JsonMediaTypeFormatter()),
                        StatusCode = HttpStatusCode.OK
                    };
                    return httpResponseMessage;
                }
                else
                {
                    httpResponseMessage = new HttpResponseMessage()
                    {
                        Content = new ObjectContent<object>(new { message = "Please Enter valid Zipcode"}, new JsonMediaTypeFormatter()),
                        StatusCode = HttpStatusCode.BadRequest
                    };
                    return httpResponseMessage;
                }

            //}
            //catch (Exception ex)
            //{
            //    //return Request.CreateResponse(HttpStatusCode.BadRequest, "Failure");
            //}
        }
        [HttpGet]
        [Route("GetBlockoutDates")]
        public async Task<HttpResponseMessage> GetBlockoutDates(string store_Id, string zipCode)
        {
            HttpResponseMessage httpResponseMessage;
           

            if (!string.IsNullOrWhiteSpace(zipCode))
            {

                DataSet ds = db.GetZoneData("USP_GetBlockoutdates", store_Id, zipCode);
                List<DeliveryDate> dates = new List<DeliveryDate>();
                GetZonesAvailabiltyResponse Response = new GetZonesAvailabiltyResponse();
                if (ds.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {


                        Response.ZoneId = (row["ZoneId"]).ToString();
                        Response.Shipmentfee = Convert.ToDecimal((row["ShipmentFee"]));
                        Response.Zonename = (row["Zonename"]).ToString();
                        Response.City = (row["City"]).ToString();
                        Response.State= (row["State"]).ToString();
                    }
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        DeliveryDate d = new DeliveryDate();
                        d.Date = Convert.ToDateTime(row["Deliverydate"]).ToString("MM-dd-yyyy");
                        d.IsActive= (row["IsActive"]).ToString();
                        d.Reason= (row["Reason"]).ToString();                      
                        dates.Add(d);

                    }
                    Response.Dates = dates;
                    
                }
                else
                {
                    httpResponseMessage = new HttpResponseMessage()
                    {
                        Content = new ObjectContent<object>(new { message = "Please Enter valid Zipcode" }, new JsonMediaTypeFormatter()),
                        StatusCode = HttpStatusCode.NotFound
                    };
                    return httpResponseMessage;

                }
                

                httpResponseMessage = new HttpResponseMessage()
                {
                    Content = new ObjectContent<object>(new { message = new { Response } }, new JsonMediaTypeFormatter()),
                    StatusCode = HttpStatusCode.OK
                };
                return httpResponseMessage;
            }
            else
            {
                httpResponseMessage = new HttpResponseMessage()
                {
                    Content = new ObjectContent<object>(new { message = "Please Enter valid Zipcode" }, new JsonMediaTypeFormatter()),
                    StatusCode = HttpStatusCode.BadRequest
                };
                return httpResponseMessage;
            }

            //}
            //catch (Exception ex)
            //{
            //    //return Request.CreateResponse(HttpStatusCode.BadRequest, "Failure");
            //}
        }
    }
}
