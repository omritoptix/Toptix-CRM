using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Toptix.Models

{
    public class Product
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name = "Product Type")]
        public int ProductTypeID { get; set; }
        public virtual ProductType ProductType { get; set; }
        [StringLength(5)]
        public string Version { get; set; }
    }


}
