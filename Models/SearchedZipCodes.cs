using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models
{
    public class SearchedZipCodes
    {
        public ZipcodeFilters zipcodeFilters { get; set; }

        public List<ZipCodes> ZipCodeList { get; set; }
    }
}