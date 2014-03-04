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
    public class DistributorController : Controller
    {
        private TCRMDBContext db = new TCRMDBContext();

        //
        // GET: /Distributor/

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //Sorting
            ViewBag.IDSortParam = String.IsNullOrEmpty(sortOrder) ? "ID desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "Name desc" : "Name";
            var distributors = db.Distributors.Include(d => d.Country).Include(d => d.Currency);

            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else page = 1;

            ViewBag.CurrentFilter = searchString;
            //searching 
            if (!String.IsNullOrEmpty(searchString))
            {
                distributors = distributors.Where(c => c.Name.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "ID desc":
                    distributors = distributors.OrderByDescending(c => c.ID);
                    break;
                case "Name":
                    distributors = distributors.OrderBy(c => c.Name);
                    break;
                case "Name desc":
                    distributors = distributors.OrderByDescending(c => c.Name);
                    break;
                default:
                    distributors = distributors.OrderBy(c => c.ID);
                    break;
            }

            //Paging
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(distributors.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Distributor/Details/5

        public ActionResult Details(int id = 0)
        {
            Distributor distributor = db.Distributors.Find(id);
            if (distributor == null)
            {
                return HttpNotFound();
            }
            return View(distributor);
        }

        //
        // GET: /Distributor/Create

        public ActionResult Create()
        {
            ViewBag.CountryID = new SelectList(db.Countries, "ID", "Description");
            ViewBag.CurrencyID = new SelectList(db.Currencies, "ID", "FullDescription");
            return View();
        }

        //
        // POST: /Distributor/Create

        [HttpPost]
        public ActionResult Create(Distributor distributor)
        {
            if (ModelState.IsValid)
            {
                db.Distributors.Add(distributor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CountryID = new SelectList(db.Countries, "ID", "Description", distributor.CountryID);
            ViewBag.CurrencyID = new SelectList(db.Currencies, "ID", "FullDescription", distributor.CurrencyID);
            return View(distributor);
        }

        //
        // GET: /Distributor/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Distributor distributor = db.Distributors.Find(id);
            if (distributor == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryID = new SelectList(db.Countries, "ID", "Description", distributor.CountryID);
            ViewBag.CurrencyID = new SelectList(db.Currencies, "ID", "FullDescription", distributor.CurrencyID);
            return View(distributor);
        }

        //
        // POST: /Distributor/Edit/5

        [HttpPost]
        public ActionResult Edit(Distributor distributor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(distributor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryID = new SelectList(db.Countries, "ID", "Description", distributor.CountryID);
            ViewBag.CurrencyID = new SelectList(db.Currencies, "ID", "FullDescription", distributor.CurrencyID);
            return View(distributor);
        }

        //
        // GET: /Distributor/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Distributor distributor = db.Distributors.Find(id);
            if (distributor == null)
            {
                return HttpNotFound();
            }
            return View(distributor);
        }

        //
        // POST: /Distributor/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Distributor distributor = db.Distributors.Find(id);
            db.Distributors.Remove(distributor);
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