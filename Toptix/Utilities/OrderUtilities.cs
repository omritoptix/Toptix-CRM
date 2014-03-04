using Elmah;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toptix.Models;

namespace Toptix.Utilities
{
    static class Constants
    {
        //how many years ahead to insert charges into the DB
        public const int  chargeXYearsAhead = 1;
       
    }

    public class OrderUtilities
    {


        public OrderUtilities()
        {
        }

        public static void updateOrderOnSale(Order orderPhase1, Order orderPhase3)
        {
            orderPhase1.Status = true;
            orderPhase1.NetPrice = orderPhase3.NetPrice;
            orderPhase1.EnumCalculationTypeId = orderPhase3.EnumCalculationTypeId;
            orderPhase1.SupportCalculatedValue = orderPhase3.SupportCalculatedValue;
            orderPhase1.SupportPriceToCharge = orderPhase3.SupportPriceToCharge;
            orderPhase1.SupportDiscountValue = orderPhase3.SupportDiscountValue;
            orderPhase1.SupportNetPriceToCharge = orderPhase3.SupportNetPriceToCharge;
            orderPhase1.EnumChargeFrequencyID = orderPhase3.EnumChargeFrequencyID;
            orderPhase1.SupportFirstChargeDate = orderPhase3.SupportFirstChargeDate; 

        }

        public static List<Charge> calcChargesPeriodsOnSale(Order order, Charge charge)
        {
            List<Charge> chargesList = new List<Charge>();
            charge.OrderID = order.ID;
            charge.ChargeValue = (Double)order.SupportNetPriceToCharge;
            charge.ClientID = order.ClientID;
            charge.IsValid = true;
            charge.IsPaid = false;
            charge.IsInvoice = false;
            //after the charges will be added a function will run to check which are alerts and which are not.
            charge.IsAlert = false; 
            //calculate date to start bill the client (not charge date)
            var startBillDate = (DateTime)order.SupportFirstChargeDate;
            //if the first charge date is more then the 1st of the month, start charging from next month
            if (startBillDate.Day > 1)
            {
                startBillDate = new DateTime(startBillDate.Year, startBillDate.Month, 1);
                startBillDate = startBillDate.AddMonths(1);
            }

            //if we charge every month
            if (order.EnumChargeFrequencyID == 1) 
            {
                //insert charges for the next year
                charge.ChargeDate = startBillDate;
                for (int i = 0; i < (12*Constants.chargeXYearsAhead) ; i++)
                {
                    charge.ChargeDate = charge.ChargeDate.AddMonths(1);
                    charge.IsAlert = false;
                    IsChargeAlert(charge);
                    chargesList.Add(new Charge(charge));
                }
            }

            //if we charge every quarter
            if (order.EnumChargeFrequencyID == 2) 
            {
                //calculate first charge sum
                double chargePerMonth = charge.ChargeValue / 3;
                charge.ChargeValue = chargePerMonth * ((13 - startBillDate.Month) % 3);
                //incase we charge in the first to quarter, update the charge sum to full sum
                if (charge.ChargeValue == 0) charge.ChargeValue = (Double)order.SupportNetPriceToCharge;
                //calc first charge date
                DateTime firstChargeDate = new DateTime();
                if (startBillDate.Month < 11)
                    firstChargeDate = new DateTime(startBillDate.Year, ((13 - startBillDate.Month) % 3) + startBillDate.Month, startBillDate.Day);
                else
                {
                    int startChargeMonth = 14 - (((13 - startBillDate.Month) % 3) + startBillDate.Month);
                    firstChargeDate = new DateTime(startBillDate.Year + 1, startChargeMonth, startBillDate.Day);
                }
                charge.ChargeDate = firstChargeDate;
                charge.IsAlert = false;
                IsChargeAlert(charge);
                //insert first charge sum and charge date
                chargesList.Add(new Charge(charge));
                charge.ChargeValue = (Double)order.SupportNetPriceToCharge;
                //insert charges for the next year

                for (int i = 0; i < (4 * Constants.chargeXYearsAhead); i++)
                {
                    charge.ChargeDate = charge.ChargeDate.AddMonths(3);
                    charge.IsAlert = false;
                    IsChargeAlert(charge);
                    chargesList.Add(new Charge(charge));
                }
            }

            //if we charge every six month
            if (order.EnumChargeFrequencyID == 3)
            {
                //calc first charge sum
                double chargePerMonth = charge.ChargeValue / 6;
                charge.ChargeValue = chargePerMonth * ((13 - startBillDate.Month) % 6);
                //incase we charge in the first to quarter, update the charge sum to full sum
                if (charge.ChargeValue == 0) charge.ChargeValue = (Double)order.SupportNetPriceToCharge;
                //calc first charge date
                DateTime firstChargeDate = new DateTime();
                if (startBillDate.Month > 7) firstChargeDate = new DateTime(startBillDate.Year + 1, 1, 1);
                else firstChargeDate = new DateTime(startBillDate.Year, 7, 1);
                charge.ChargeDate = firstChargeDate;
                charge.IsAlert = false;
                IsChargeAlert(charge);
                // insert first charge sum and charge date
                chargesList.Add(new Charge(charge));
                charge.ChargeValue = (Double)order.SupportNetPriceToCharge;
                //insert charges for the next year
                for (int i = 0; i < (2 * Constants.chargeXYearsAhead); i++)
                {
                    charge.ChargeDate = charge.ChargeDate.AddMonths(6);
                    charge.IsAlert = false;
                    IsChargeAlert(charge);
                    chargesList.Add(new Charge(charge));
                }
            }

            //if we charge once a year
            if (order.EnumChargeFrequencyID == 4)
            {
                charge.ChargeDate = (DateTime)order.SupportFirstChargeDate;
                // insert one charge for next year 
                for (int i = 0; i < (1 * Constants.chargeXYearsAhead); i++)
                {
                    charge.ChargeDate = charge.ChargeDate.AddYears(1);
                    charge.IsAlert = false;
                    IsChargeAlert(charge);
                    chargesList.Add(new Charge(charge));
                }
              
            }

            return chargesList;
            
            //catch exception
        }

        public static void IsChargeAlert(Charge charge)
        {
            TCRMDBContext _db = new TCRMDBContext();
            //add try catch in higher level
            GlobalParameters globalParameters = new GlobalParameters();
            globalParameters = _db.GlobalParameteres.First();
            int AlertRangeInDays = globalParameters.AlertRangeInDays;
            int daysToChargeDue = (int)(charge.ChargeDate - DateTime.Now).TotalDays;
            if (daysToChargeDue < globalParameters.AlertRangeInDays)
                charge.IsAlert = true;
        }

        public static void UpdateChargesOnOrderEdit(Order order)
        {
            TCRMDBContext _db = new TCRMDBContext();
            List<Charge> chargesList = _db.Charges.Where(c => c.OrderID == order.ID).ToList<Charge>();
            if (chargesList == null) return;
            if (order.Status == false)
            {
                foreach (Charge chargeItem in chargesList)
                {
                    chargeItem.IsValid = false;
                    _db.Entry(chargeItem).State = EntityState.Modified;

                }
            }
            if (order.Status == true)
            {
                foreach (Charge chargeItem in chargesList)
                {
                    chargeItem.IsValid = true;
                    _db.Entry(chargeItem).State = EntityState.Modified;
                }
            }
            //save changes to DB
           _db.SaveChanges();
        }

        public static void UpdateOrderOnClientEdit(Client client)
        {
            TCRMDBContext _db = new TCRMDBContext();
            List<Order> ordersList = _db.Orders.Where(o => o.ClientID == client.ID).ToList<Order>();
            if (ordersList == null) return;
                foreach (Order order in ordersList)
                {
                    order.Status  = false;
                    //also make all the charges for this order not active
                    UpdateChargesOnOrderEdit(order);
                    _db.Entry(order).State = EntityState.Modified;

                }

            //save changes to DB
            _db.SaveChanges();
        }

        public static void caculateOrderItemNetPriceWithConversion(List<OrderItem> orderItemsList)
        {
            TCRMDBContext _db = new TCRMDBContext();
            foreach (OrderItem orderItem in orderItemsList)
            {
                //check if currency defined as system currency
                if (orderItem.CurrencyID != _db.GlobalParameteres.First().CurrencyID)
                orderItem.NetPrice = OrderUtilities.caculateCurrentConversion(orderItem.CurrencyID) * orderItem.NetPrice;
            }
        }


        public static double caculateCurrentConversion(int currencyID)
        {
            TCRMDBContext _db = new TCRMDBContext();
            List<ConversionRate> conversionRatesList = _db.ConversionRates.Where(c => c.CurrencyID == currencyID).ToList();
            //if list is empty , throw an exception
            if (conversionRatesList.Count == 0) throw new System.ArgumentException("There are no conversion rates for this currency");
            //find the right conversion - which is the Max {earlier dates then today}
            List<ConversionRate> earlierDatesThenNow = new List<ConversionRate>();
            foreach (ConversionRate conversionRate in conversionRatesList)
            {
                if (conversionRate.Date <= DateTime.Now) earlierDatesThenNow.Add(conversionRate);
            }
            //if conversion list is empty now, throw an exception
            if (earlierDatesThenNow.Count == 0) throw new System.ArgumentException("There are no conversion rates earlier or equal to today for this currency");
            //find the max of the earlier or equal dates of today
            DateTime maxDate = new DateTime(1900, 1, 1);
            ConversionRate rightConversionRate = new ConversionRate();
            foreach (ConversionRate conversionRate in earlierDatesThenNow)
            {
                if (conversionRate.Date > maxDate)
                {
                    maxDate = conversionRate.Date;
                    rightConversionRate = conversionRate;
                }
            }
            
            //if no dates, or date is (1900,1,1) throw exception
            if (maxDate == new DateTime(1900,1,1)) throw new System.ArgumentException("There are no conversion rates earlier or equal to today for this currency");
            return rightConversionRate.ConversionValue;
        } 


    }
}