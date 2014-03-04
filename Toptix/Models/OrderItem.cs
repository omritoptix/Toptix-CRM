using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Toptix.Models
{
    //[Bind(Exclude= "ID,OrderID")]


    public class OrderItem : IValidatableObject
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
        [Range(1, Int32.MaxValue, ErrorMessage = "the quantity must be a positive number nn")]
        public int Quantity { get; set; }
        [Display(Name="Price Before Conversion")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public double Price { get; set; }
        [Display(Name = "Is Part Of Service")]
        public bool IsPartOfService { get; set; }
        [Display(Name = "Discount Type")]
        public int EnumCalculationTypeID { get; set; }
        public virtual EnumCalculationType EnumCalculationType { get; set; }
        //[Remote("validateDiscount", "Validation", AdditionalFields = "EnumCalculationTypeID", ErrorMessage = "Precentage Discount must be between 0 and 100")]
        public double Discount { get; set; }
        [Display(Name = "Net Price After Conversion")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public double NetPrice { get; set; }
        [Required]
        [ScaffoldColumn(false)]
        public int OrderID { get; set; }
        public virtual Order Order { get; set; }
        public int CurrencyID { get; set; }
        public virtual Currency Currency { get; set; }

        //custom validations

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var field = new[] { "Discount" };
            if (EnumCalculationTypeID == 2 && (Discount > 100 || Discount < 0))
                yield return new ValidationResult("Precentage Discount must be between 0 and 100", field);
            if (EnumCalculationTypeID == 1 && (Discount > Price))
            {
                yield return new ValidationResult("Dicount can not  be greater then the price", field);

            }
        }
    }
}