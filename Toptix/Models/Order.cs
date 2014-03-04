using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Toptix.Models
{
     //[Bind(Exclude = "ID")]
    public class Order : IValidatableObject
    {
        public int ID { get; set; }
        public int ClientID { get; set; }
        public virtual Client Client { get; set; }
        [Display(Name = "Charge Frequency")]
        public int? EnumChargeFrequencyID { get; set; }
        public virtual EnumChargeFrequency EnumChargeFrequency { get; set; }
        [Display(Name = "Pay Type")]
        public int? PayConditionID { get; set; }
        public virtual PayCondition PayCondition { get; set; }
        [Display(Name = "Order Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Net Products Price")]
        public double NetPrice { get; set; }
        [Display(Name = "Support Calculated Value in Precentage")]
        [Range(0,100)]
        public double? SupportCalculatedValue { get; set; }
        [Display(Name = "Support Price To Charge")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public double? SupportPriceToCharge { get; set; }
        [Display(Name = "Support Discount Calculation Type")]
        public int? EnumCalculationTypeId { get; set; }
        public virtual EnumCalculationType EnumClaculationType { get; set; }
        [Display(Name = "Support Discount Value")]
        public double? SupportDiscountValue { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Support Net Price To Charge")]
        public double? SupportNetPriceToCharge { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Support First Charge Date")]
        public DateTime? SupportFirstChargeDate { get; set; }
        public bool Status { get; set; }
        public virtual ICollection<OrderItem> orderItem { get; set; }
        public virtual ICollection<Charge> Charge { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var field = new[] { "SupportDiscountValue" };
            if (EnumCalculationTypeId == 2 && (SupportDiscountValue > 100 || SupportDiscountValue < 0))
                yield return new ValidationResult("Percentage Discount must be between 0 and 100", field);
            if (EnumCalculationTypeId == 1)
            {
                if (SupportDiscountValue > (NetPrice * (SupportCalculatedValue / 100)))
                    yield return new ValidationResult("Discount can not  be greater then the price", field);
            }
        }

    }
}