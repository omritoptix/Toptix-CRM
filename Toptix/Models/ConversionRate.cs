using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Toptix.Models
{
    public class ConversionRate
    {
        public int ID { get; set; }
        public int CurrencyID { get; set; }
        public virtual Currency Currency { get; set; }
        [Required]
        [Display(Name = "Conversion Rate")]
        [Range(0, Int32.MaxValue)]
        public double ConversionValue { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
    }
}