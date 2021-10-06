using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Controllers
{
    public class DbDetails
    {
        public string Address { get; set; }
        public string DatabaseName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string StoreId { get; set; }
        public string StampsUserName { get; set; }
        public string StampsUserPassword { get; set; }
        public string IntegrationId { get; set; }
    }
}