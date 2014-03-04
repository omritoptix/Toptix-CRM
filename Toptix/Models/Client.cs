using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Toptix.Models
{
    public class Client
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name = "Distributor Name")]
        public int DistributorID { get; set; }
        public virtual Distributor Distributor { get; set; }
        [Display(Name = "Activity Type")]
        public int ActivityTypeID { get; set; }
        public virtual ActivityType ActivityType { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int CountryID { get; set; }
        public virtual Country Country { get; set; }
        public string Phone { get; set; }
        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }
        [Display(Name = "Contact Phone")]
        public string ContactPhone { get; set; }
        [Display(Name = "Is There Lisence For Site")]
        public bool isThereLisenceForSite { get; set; }
        [Display(Name = "Joining Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime JoiningDate { get; set; }
        [Display(Name = "Contact Mail")]
        [EmailAddress]
        public string ContactMail { get; set; }
        public int? PayTypeID { get; set; }
        public virtual PayType PayType { get; set; }
        public int CurrencyID { get; set; }
        public virtual Currency Currency { get; set; }
        public DateTime? LisenceValidTill { get; set; }
        [Display(Name = "Lisence Number")]
        public string LisenceNumber { get; set; }
        public bool Status { get; set; }
    }

}

