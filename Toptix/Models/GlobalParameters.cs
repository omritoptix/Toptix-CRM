using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Toptix.Models
{
    public class GlobalParameters
    {
        public int ID { get; set; }
        [Required]
        [Range(0, Int32.MaxValue)]
        [Display(Name = "Charge is An Alert X Days Before It's Due")]
        public int AlertRangeInDays { get; set; }
        [EmailAddress]
        [Display(Name = "Email Notification Address")]
        public String EmailNotificationAddress { get; set; }
        [Range(0, 12)]
        [Display(Name = "Create New Charges X Months Before Last Charge is Due")]
        public int CreateNewChargesPeriod { get; set; }
        [Display(Name = "System Currency")]
        public int CurrencyID { get; set; }
        public virtual Currency Currency { get; set; }
    }
}