using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toptix.Models;
using Toptix.Utilities;
using PagedList;
using Elmah;
using Newtonsoft.Json.Linq;

namespace Toptix.Controllers
{
    public class OrderController : Controller
    {
        private TCRMDBContext db = new TCRMDBContext();       
        //
        // GET: /Order/

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //Sorting
            ViewBag.OrderIDSortParm = String.IsNullOrEmpty(sortOrder) ? "ID desc" : "";
            ViewBag.ClientSortParm = sortOrder == "Client" ? "Client desc" : "Client";
            var ordersQueryable = db.Orders.Include(o => o.Client).Include(o => o.EnumClaculationType).Include(o => o.PayCondition).OrderByDescending(o => o.ID);
            var orders = db.Orders.Include(o => o.Client).Include(o => o.EnumClaculationType).Include(o => o.PayCondition).OrderByDescending(o => o.ID).Take(50);
            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else page = 1;

            ViewBag.CurrentFilter = searchString;
            //searching 
            if (!String.IsNullOrEmpty(searchString))
            {
                orders = (System.Linq.IOrderedQueryable<Toptix.Models.Order>)ordersQueryable.Where(o => o.Client.Name.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "ID desc":
                    orders = orders.OrderByDescending(o => o.ID);
                    break;
                case "Client desc":
                    orders = orders.OrderByDescending(o => o.Client.Name);
                    break;
                case "Client":
                    orders = orders.OrderByDescending(o => o.Client.Name);
                    break;
                default:
                    orders = orders.OrderBy(o => o.ID);
                    break;
            }

            //Paging
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(orders.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Order/Details/5

        public ActionResult Details(int id = 0)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        //
        // GET: /Order/SalePhase1

        public ActionResult SalePhase1()
        {
            ViewBag.ClientID = new SelectList(db.Clients.Where(c => c.Status == true), "ID", "Name");
            ViewBag.EnumChargeFrequencyID = new SelectList(db.EnumChargeFrequencies, "ID", "Description");
            ViewBag.PayConditionID = new SelectList(db.PayConditions, "ID", "Description");
            return View();
        }

        //
        // POST: /Order/SalePhase1

        [HttpPost, ActionName("SalePhase1")]
        public ActionResult SalePhase1Complete(Order order)
        {
            if (ModelState.IsValid)
            {
                TempData["orderPhase1"] = order;
                TempData["RedirectFromPhase1"] = true;
                //get default currency
                var client = db.Clients.Where(c => c.ID == order.ClientID).First();
                TempData["DefaultCurrency"] = client.CurrencyID;
                return RedirectToAction("SalePhase2");
            }

            ViewBag.ClientID = new SelectList(db.Clients, "ID", "Name", order.ClientID);
            ViewBag.PayConditionID = new SelectList(db.PayConditions, "ID", "Description", order.PayConditionID);
            return View(order);
        }

        // GET: /Order/SalePhase2

        public ActionResult SalePhase2()
        {
            if (TempData["RedirectFromPhase1"] != null)
            {
                //get default currency
                int currencyID = (int)TempData["DefaultCurrency"];
                string currency = db.Currencies.Where(c => c.ID == currencyID).Select(c => c.Description).First();
                ViewBag.defaultCurrency = TempData["DefaultCurrency"];
                return View();
            }
            else return RedirectToAction("Index");
        }

        // GET: SendJson
        public JsonResult Products()
        {
            var products = db.Products;
            return Json(products.ToList(), JsonRequestBehavior.AllowGet);
        }

        // GET: SendJson
        public JsonResult Discounts()
        {
            var discounts = db.EnumCalculationTypes;
            return Json(discounts.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChargeFrequency()
        {
            var chargeFrequency = db.EnumChargeFrequencies;
            return Json(chargeFrequency.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Currencies ()
        {
            var currencies = db.Currencies;
            return Json(currencies.ToList(), JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Order/SalePhase2

        [HttpPost]
        public ActionResult SalePhase2(string orderItems)
        {
            List<OrderItem> orderItemsList = new List<OrderItem>();
            try
            {
                orderItemsList = JsonConvert.DeserializeObject<List<OrderItem>>(orderItems);
                //calculate orderItem net price based on conversion rates 
                OrderUtilities.caculateOrderItemNetPriceWithConversion(orderItemsList);
            }
            catch (Exception e)
            {
                //write exception to elmah log
                ErrorSignal.FromCurrentContext().Raise(e);  
                ViewBag.errorMessage = e.Message;
                ViewBag.route = "SalePhase2";
                return View("Exception");
            }
            TempData["OrderItems"] = orderItemsList;
            double totalNetPrice = 0;
            double partOfServicePrice = 0;
            foreach (OrderItem orderItem in orderItemsList)
            {
                totalNetPrice += orderItem.NetPrice;
                if (orderItem.IsPartOfService == true)
                    partOfServicePrice += orderItem.NetPrice;
            }
            TempData["partOfServicePrice"] = partOfServicePrice;
            TempData["totalNetPrice"] = totalNetPrice;

            //incase the totalNetPriceForSupport=0 , we need to skip the 
            // the charges screen and finish the order.

            if (partOfServicePrice == 0)
            {
                try
                {
                    Order orderWithoutCharges = new Order();
                    orderWithoutCharges = (Order)TempData["orderPhase1"];
                    orderWithoutCharges.Status = true;
                    orderWithoutCharges.NetPrice = totalNetPrice;
                    orderWithoutCharges.orderItem = orderItemsList;

                    if (ModelState.IsValid)
                    {
                        db.Orders.Add(orderWithoutCharges);
                        foreach (OrderItem orderItem in orderItemsList)
                        {
                            db.OrderItems.Add(orderItem);
                        }
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Index");

                    }
                }
                catch (Exception e)
                {
                    //write exception to elmah log
                    ErrorSignal.FromCurrentContext().Raise(e);  
                    ViewBag.errorMessage = e.Message;
                    ViewBag.route = "Index";
                    return View("Exception");
                }
            }
            TempData["RedirectFromPhase2"] = true;
            return RedirectToAction("SalePhase3");
        }

        //
        // GET: /Order/SalePhase3
        
        public ActionResult SalePhase3()
        {
            if (TempData["RedirectFromPhase2"] == null || TempData["partOfServicePrice"] == null)
                return RedirectToAction("Index");
                
                ViewBag.partOfServicePrice = TempData["partOfServicePrice"];
                //so that tempdata will be available when refreshing the page
                TempData["partOfServicePrice"] = ViewBag.partOfServicePrice;
            
       
            return View();
        }


        //
        // POST: /Order/SalePhase3

        [HttpPost]
        public ActionResult SalePhase3(string orderPhase3Json)
        {
            
            Order orderPhase3 = new Order();
            try
            {
                orderPhase3 = JsonConvert.DeserializeObject<Order>(orderPhase3Json);
                //Format SupportFirstChargeDate to dd/mm/yy
                JObject o = JObject.Parse(orderPhase3Json);
                string SupportFirstChargeDate = (string)o["SupportFirstChargeDateUnformatted"];
                DateTime formattedDate = Convert.ToDateTime(SupportFirstChargeDate);
                orderPhase3.SupportFirstChargeDate = formattedDate;
                //check if totalNetPrice received a null due to redirection.
                orderPhase3.NetPrice = (double)TempData["totalNetPrice"];
            }
            catch (JsonReaderException e)
            {
                //write exception to elmah log
                ErrorSignal.FromCurrentContext().Raise(e);  
                ViewBag.errorMessage = e.Message;
                ViewBag.route = "SalePhase3";
                return View("Exception");
            }
            try
            {
                Order orderPhase1 = (Order)TempData["orderPhase1"];
                List<OrderItem> orderItems = (List<OrderItem>)TempData["orderItems"];
                Charge charge = new Charge();
                OrderUtilities.updateOrderOnSale(orderPhase1, orderPhase3);
                orderPhase1.orderItem = orderItems;
                db.Orders.Add(orderPhase1);
                foreach (OrderItem orderItem in orderItems)
                {
                    db.OrderItems.Add(orderItem);
                }
                List<Charge> chargesList = OrderUtilities.calcChargesPeriodsOnSale(orderPhase1, charge);
                foreach (Charge chargeItem in chargesList)
                {
                    db.Charges.Add(chargeItem);
                }
                if (ModelState.IsValid)
                {
                    db.SaveChanges();
                }
                else
                {
                    //display exception to user
                    return RedirectToAction("Index");
                }

                //catch exception if can't save data

                return RedirectToAction("IndexFromOrderScreen", "Charge", new { id = orderPhase1.ID });
            }

            catch (Exception e)
            {
                //write exception to elmah log
                ErrorSignal.FromCurrentContext().Raise(e);  
                ViewBag.errorMessage = e.Message;
                ViewBag.route = "Index";
                return View("Exception");
            }
         }



        //
        // GET: /Order/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            TempData["uneditedOrderStatus"] = order.Status;
            ViewBag.PayConditionID = new SelectList(db.PayConditions, "ID", "Description", order.PayConditionID);
            //ViewBag.CurrencyID = new SelectList(db.Currencies, "ID", "Description", order.CurrencyID);
            return View(order);
        }

        //
        // POST: /Order/Edit/5

        [HttpPost]
        public ActionResult Edit(Order order)
        {
        bool uneditedOrderStatus;
        try
        {
            uneditedOrderStatus = (bool)TempData["uneditedOrderStatus"];
            if (ModelState.IsValid)
            {
                if (uneditedOrderStatus != order.Status)
                {
                    Client clientOrder = db.Clients.Find(order.ClientID);

                    if ((order.Status == true) && (clientOrder.Status == false))
                        throw new Exception("Can't Change order Status To Active if the Order's Client is not Active");
                    else OrderUtilities.UpdateChargesOnOrderEdit(order);
                }
                db.Entry(order).State = EntityState.Modified;
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        catch (Exception e)
        {
            ErrorSignal.FromCurrentContext().Raise(e);
            ViewBag.errorMessage = e.Message;
            ViewBag.route = "Index";
            return View("Exception");
        }
            ViewBag.ClientID = new SelectList(db.Clients, "ID", "Name", order.ClientID);
            ViewBag.PayConditionID = new SelectList(db.PayConditions, "ID", "Description", order.PayConditionID);
            //ViewBag.CurrencyID = new SelectList(db.Currencies, "ID", "Description", order.CurrencyID);
            return View(order);
        }

        //
        // GET: /Order/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        //
        // POST: /Order/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
         {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CustomErrorFilter(string routeFrom)
        {
            //routeFrom is one of the actions : Index,SalePhase2,SalePhase3
            return RedirectToAction(routeFrom);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}