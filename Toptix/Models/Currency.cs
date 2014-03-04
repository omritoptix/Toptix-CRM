using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Toptix.Models
{
    public class Currency
    {
        public int ID { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Symbol {get;set;}
        public string FullDescription
        {
            get
            {
                return string.Format("{0} {1}", Description, Country);
            }
        }
    }
}