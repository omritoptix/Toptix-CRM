using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Toptix.Models
{
    public class Distributor
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [Display(Name = "Is Part Of Subsidiary")]
        public bool isPartOfSubsidiary { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int CountryID { get; set; }
        public virtual Country Country { get; set; }
        public string Phone { get; set; }
        [EmailAddress]
        public string Mail { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        [Display(Name = "Support Fee %")]
        [Range(0, 100)]
        public double? SupportFee { get; set; }
        [Display(Name = "Lisence Fee %")]
        [Range(0, 100)]
        public double? LisenceFee { get; set; }
        [Display(Name = "Proffesional Services Fee %")]
        [Range(0, 100)]
        public double? ProffesionalServicesFee { get; set; }
        public int CurrencyID { get; set; }
        public virtual Currency Currency { get; set; }
    }
}