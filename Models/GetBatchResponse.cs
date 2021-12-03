using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models.GetBatchResponse
{
    
    public class BatchShipmentsUrl
    {
        public string href { get; set; }
    }

    public class BatchLabelsUrl
    {
        public string href { get; set; }
    }

    public class BatchErrorsUrl
    {
        public string href { get; set; }
    }

    public class LabelDownload
    {
        public string pdf { get; set; }
        public string zpl { get; set; }
        public string href { get; set; }
    }

    public class FormDownload
    {
        public string href { get; set; }
    }

    public class Root
    {
        public string label_layout { get; set; }
        public string label_format { get; set; }
        public string batch_id { get; set; }
        public string external_batch_id { get; set; }
        public string batch_number { get; set; }
        public string batch_notes { get; set; }
        public DateTime created_at { get; set; }
        public DateTime processed_at { get; set; }
        public int errors { get; set; }
        public int warnings { get; set; }
        public int completed { get; set; }
        public int forms { get; set; }
        public int count { get; set; }
        public BatchShipmentsUrl batch_shipments_url { get; set; }
        public BatchLabelsUrl batch_labels_url { get; set; }
        public BatchErrorsUrl batch_errors_url { get; set; }
        public LabelDownload label_download { get; set; }
        public FormDownload form_download { get; set; }
        public string status { get; set; }
    }

}