using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toptix.Models;
using PagedList;

namespace Toptix.Controllers
{
    public class ConversionRateController : Controller
    {
        private TCRMDBContext db = new TCRMDBContext();

        //
        // GET: /ConversionRate/


        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //Sorting
            ViewBag.IDSortParam = String.IsNullOrEmpty(sortOrder) ? "ID desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "Name desc" : "Name";
            var conversionrates = db.ConversionRates.Include(c => c.Currency).Take(50);

            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else page = 1;

            ViewBag.CurrentFilter = searchString;
            //searching 
            if (!String.IsNullOrEmpty(searchString))
            {
                conversionrates = conversionrates.Where(c => c.Currency.Description.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "ID desc":
                    conversionrates = conversionrates.OrderByDescending(c => c.ID);
                    break;
                case "Name":
                    conversionrates = conversionrates.OrderBy(c => c.Currency.Description);
                    break;
                case "Name desc":
                    conversionrates = conversionrates.OrderByDescending(c => c.Currency.Description);
                    break;
                default:
                    conversionrates = conversionrates.OrderBy(c => c.ID);
                    break;
            }

            //Paging
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(conversionrates.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /ConversionRate/Details/5

        public ActionResult Details(int id = 0)
        {
            ConversionRate conversionrate = db.ConversionRates.Find(id);
            if (conversionrate == null)
            {
                return HttpNotFound();
            }
            return View(conversionrate);
        }

        //
        // GET: /ConversionRate/Create

        public ActionResult Create()
        {
            ViewBag.CurrencyID = new SelectList(db.Currencies, "ID", "FullDescription");
            return View();
        }

        //
        // POST: /ConversionRate/Create

        [HttpPost]
        public ActionResult Create(ConversionRate conversionrate)
        {
            if (ModelState.IsValid)
            {
                db.ConversionRates.Add(conversionrate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CurrencyID = new SelectList(db.Currencies, "ID", "FullDescription", conversionrate.CurrencyID);
            return View(conversionrate);
        }

        //
        // GET: /ConversionRate/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ConversionRate conversionrate = db.ConversionRates.Find(id);
            if (conversionrate == null)
            {
                return HttpNotFound();
            }
            ViewBag.CurrencyID = new SelectList(db.Currencies, "ID", "FullDescription", conversionrate.CurrencyID);
            return View(conversionrate);
        }

        //
        // POST: /ConversionRate/Edit/5

        [HttpPost]
        public ActionResult Edit(ConversionRate conversionrate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(conversionrate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CurrencyID = new SelectList(db.Currencies, "ID", "FullDescription", conversionrate.CurrencyID);
            return View(conversionrate);
        }

        //
        // GET: /ConversionRate/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ConversionRate conversionrate = db.ConversionRates.Find(id);
            if (conversionrate == null)
            {
                return HttpNotFound();
            }
            return View(conversionrate);
        }

        //
        // POST: /ConversionRate/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ConversionRate conversionrate = db.ConversionRates.Find(id);
            db.ConversionRates.Remove(conversionrate);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}