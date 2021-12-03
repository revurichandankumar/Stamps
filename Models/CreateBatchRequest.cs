using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models.CreateBatchRequest
{
    public class Root
    {
        public string external_batch_id { get; set; }
        public string batch_notes { get; set; }
        public List<string> shipment_ids { get; set; }
        public List<object> rate_ids { get; set; }
    }
}