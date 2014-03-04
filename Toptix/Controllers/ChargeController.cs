using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toptix.Models;
using PagedList;
using Toptix.Utilities;
using Elmah;

namespace Toptix.Controllers
{
    public class ChargeController : Controller
    {
        private TCRMDBContext db = new TCRMDBContext();

        //
        // GET: /Charge/

        public ActionResult IndexFromOrderScreen(int id = 0)
        {
            var charges = db.Charges.Include(c => c.Client).Include(c => c.Order).Where(c => c.OrderID == id);
            if (charges.Count() == 0)
            {
                return View("NoChargesForOrder");
            }
            TempData["CameFromIndexFromOrderScreen"] = true;
            return View(charges.ToList());
        }

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //Sorting
            TempData["CameFromIndexFromOrderScreen"] = false;
            ViewBag.IDSortParam = String.IsNullOrEmpty(sortOrder) ? "ID desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date desc" : "Date";
            ViewBag.ClientSortParm = sortOrder == "Client" ? "Client desc" : "Client";
            var chargesQueryable = db.Charges.Include(c => c.Client).Include(c => c.Order);
            var charges = db.Charges.Include(c => c.Client).Include(c => c.Order).Take(50);
            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else page = 1;

            ViewBag.CurrentFilter = searchString;
            //searching 
            if (!String.IsNullOrEmpty(searchString))
            {
                charges = (System.Linq.IOrderedQueryable<Toptix.Models.Charge>)chargesQueryable.Where(o => o.Client.Name.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "ID desc":
                    charges = charges.OrderByDescending(c => c.ID);
                    break;
                case "Date desc":
                    charges = charges.OrderByDescending(c => c.ChargeDate);
                    break;
                case "Date":
                    charges = charges.OrderBy(c => c.ChargeDate);
                    break;
                case "Client desc":
                    charges = charges.OrderByDescending(c => c.Client.Name);
                    break;
                case "Client":
                    charges = charges.OrderBy(c => c.Client.Name);
                    break;
                default:
                    charges = charges.OrderBy(c => c.ID);
                    break;
            }

            //Paging
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(charges.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Charge/Details/5

        public ActionResult Details(int id = 0)
        {
            Charge charge = db.Charges.Find(id);
            if (charge == null)
            {
                return HttpNotFound();
            }
            return View(charge);
        }

        
         //GET: /Charge/Create

        public ActionResult Create()
        {
            ViewBag.OrderID = new SelectList(db.Orders.Where(o => o.EnumChargeFrequencyID != null), "ID", "ID");
            return View();
        }

        
         //POST: /Charge/Create

        [HttpPost]
        public ActionResult Create(Charge charge)
        {
            //check if the order is valid 
            Order orderForCharge = db.Orders.Find(charge.OrderID);
            try
            {
                if (orderForCharge.Status == false)
                    throw new Exception("Can't Create a charge for an unactive Order");
            }
            catch (Exception e)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
                ViewBag.errorMessage = e.Message;
                ViewBag.route = "Index";
                return View("Exception");
            }
            //else connect client to charge using order ID
            int ClientID = orderForCharge.ClientID;
            charge.ClientID = ClientID;
            //check if model is valid
            if (ModelState.IsValid)
            {
                db.Charges.Add(charge);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientID = new SelectList(db.Clients, "ID", "Name", charge.ClientID);
            // populate orders dropdown only with orders which "IsPartOfService" = true
            ViewBag.OrderID = new SelectList(db.Orders.Where(o => o.EnumChargeFrequencyID != null), "ID", "ID", charge.OrderID);
            return View(charge);
        }

        //
        // GET: /Charge/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Charge charge = db.Charges.Find(id);
            if (charge == null)
            {
                return HttpNotFound();
            }
            TempData["ChargeStatus"] = charge.IsValid;
            return View(charge);
        }

        //
        // POST: /Charge/Edit/5

        [HttpPost]
        public ActionResult Edit(Charge charge)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // if the charge status has changed to active check that the order is active
                    bool chargeStatus = (bool)TempData["ChargeStatus"];
                    if ((charge.IsValid != chargeStatus) && charge.IsValid == true)
                    {
                        Order orderForCharge = db.Orders.Find(charge.OrderID);
                        if (orderForCharge.Status == false)
                            throw new Exception("Can't Change charge to active - charge order is not Active.");
                    }
                    OrderUtilities.IsChargeAlert(charge);
                    db.Entry(charge).State = EntityState.Modified;
                    db.SaveChanges();
                    if (TempData["CameFromIndexFromOrderScreen"] != null)
                    {
                        if ((bool)TempData["CameFromIndexFromOrderScreen"] == true)
                            return RedirectToAction("IndexFromOrderScreen", "Charge", new { id = charge.OrderID });
                        else return RedirectToAction("Index");
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ErrorSignal.FromCurrentContext().Raise(e);
                    ViewBag.errorMessage = e.Message;
                    ViewBag.route = "Index";
                    return View("Exception");
                }
            }
            return View(charge);
        }

        //
        // GET: /Charge/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Charge charge = db.Charges.Find(id);
            if (charge == null)
            {
                return HttpNotFound();
            }
            return View(charge);
        }

        //
        // POST: /Charge/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Charge charge = db.Charges.Find(id);
            db.Charges.Remove(charge);
            Order order = db.Orders.Find(charge.OrderID);
            //List<Charge> chargeList = (List<Charge>)order.Charge.ToList();
            if (order.Charge == null)
            {
                db.Orders.Remove(order);
            }
            db.SaveChanges();
            if (TempData["CameFromIndexFromOrderScreen"] != null)
            {
                if ((bool)TempData["CameFromIndexFromOrderScreen"] == true)
                    return RedirectToAction("IndexFromOrderScreen", "Charge", new { id = charge.OrderID });
                else return RedirectToAction("Index");
            }
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