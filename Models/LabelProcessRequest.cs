using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models.LabelProcessRequest
{
    public class Root
    {
        public DateTime ship_date { get; set; }
        public string label_layout { get; set; }
        public string label_format { get; set; }
    }

}