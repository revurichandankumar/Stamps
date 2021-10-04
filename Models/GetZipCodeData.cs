using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models
{
    public class GetZipCodeData
    {
        public List<ZipCodes> ZipCodeList { get; set; }
    }
    public class ZipCodes
    {
        public string Name { get; set; }
        public string Zipcode { get; set; }
    }

}