using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Toptix.Models
{
    public class Charge
    {
        public int ID { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime ChargeDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public double ChargeValue { get; set; }
        public int ClientID { get; set; }
        public virtual Client Client { get; set; }
        public int OrderID { get; set; }
        public virtual Order Order { get; set; }
        public bool IsAlert { get; set; }
        public bool IsValid { get; set; }
        public bool IsPaid { get; set; }
        public bool IsInvoice { get; set; }


        //default constructor

        public Charge()
        {
        }

        //copy Constructor

        public Charge(Charge charge)
        {
            this.ID = charge.ID;
            this.ChargeDate = charge.ChargeDate;
            this.ChargeValue = charge.ChargeValue;
            this.ClientID = charge.ClientID;
            this.Client = charge.Client;
            this.OrderID = charge.OrderID;
            this.Order = charge.Order;
            this.IsAlert = charge.IsAlert;
            this.IsValid = charge.IsValid;
            this.IsPaid = charge.IsPaid;
            this.IsInvoice = charge.IsInvoice;
        }
    }

   



}