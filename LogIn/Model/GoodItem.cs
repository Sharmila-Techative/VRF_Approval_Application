using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogIn.Model
{
    public class GoodItem
    {
        public int SerialNo { get; set; }
        public string Product { get; set; }
        public string Brand { get; set; }
        public string Size { get; set; }
        public string MaterialDescription { get; set; }
        public string HSNCode { get; set; }
        public string TaxPercentage { get; set; }
    }
}