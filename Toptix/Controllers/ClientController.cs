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

namespace Toptix.Controllers
{
    public class ClientController : Controller
    {
        private TCRMDBContext db = new TCRMDBContext();

        //
        // GET: /Client/

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //Sorting
            ViewBag.IDSortParam = String.IsNullOrEmpty(sortOrder) ? "ID desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "Name desc" : "Name";
            ViewBag.DistributorSortParm = sortOrder == "Distributor" ? "Distributor desc" : "Distributor";
            var clients = db.Clients.Include(c => c.Distributor).Include(c => c.ActivityType).Include(c => c.Country).Include(c => c.PayType).Include(c => c.Currency);

            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else page = 1;

            ViewBag.CurrentFilter = searchString;
            //searching 
            if (!String.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(c => c.Name.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "ID desc":
                    clients = clients.OrderByDescending(c => c.ID);
                    break;
                case "Name":
                    clients = clients.OrderBy(c => c.Name);
                    break;
                case "Name desc":
                    clients = clients.OrderByDescending(c => c.Name);
                    break;
                case "Distributor":
                    clients = clients.OrderBy(c => c.Distributor.Name);
                    break;
                case "Distributor desc":
                    clients = clients.OrderByDescending(c => c.Distributor.Name);
                    break;
                default:
                    clients = clients.OrderBy(c => c.ID);
                    break;
            }

            //Paging
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(clients.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Client/Details/5

        public ActionResult Details(int id = 0)
        {
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        //
        // GET: /Client/Create

        public ActionResult Create()
        {
            ViewBag.DistributorID = new SelectList(db.Distributors, "ID", "Name");
            ViewBag.ActivityTypeID = new SelectList(db.ActivityTypes, "ID", "Description");
            ViewBag.CountryID = new SelectList(db.Countries, "ID", "Description");
            ViewBag.PayTypeID = new SelectList(db.PayTypes, "ID", "Description");
            ViewBag.CurrencyID = new SelectList(db.Currencies, "ID", "FullDescription");
            return View();
        }

        //
        // POST: /Client/Create

        [HttpPost]
        public ActionResult Create(Client client)
        {
            if (ModelState.IsValid)
            {
                db.Clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DistributorID = new SelectList(db.Distributors, "ID", "Name", client.DistributorID);
            ViewBag.ActivityTypeID = new SelectList(db.ActivityTypes, "ID", "Description", client.ActivityTypeID);
            ViewBag.CountryID = new SelectList(db.Countries, "ID", "Description", client.CountryID);
            ViewBag.PayTypeID = new SelectList(db.PayTypes, "ID", "Description", client.PayTypeID);
            ViewBag.CurrencyID = new SelectList(db.Currencies, "ID", "FullDescription", client.CurrencyID);
            return View(client);
        }

        //
        // GET: /Client/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            TempData["ClientStatus"] = client.Status;
            ViewBag.DistributorID = new SelectList(db.Distributors, "ID", "Name", client.DistributorID);
            ViewBag.ActivityTypeID = new SelectList(db.ActivityTypes, "ID", "Description", client.ActivityTypeID);
            ViewBag.CountryID = new SelectList(db.Countries, "ID", "Description", client.CountryID);
            ViewBag.PayTypeID = new SelectList(db.PayTypes, "ID", "Description", client.PayTypeID);
            ViewBag.CurrencyID = new SelectList(db.Currencies, "ID", "FullDescription", client.CurrencyID);
            return View(client);
        }

        //
        // POST: /Client/Edit/5

        [HttpPost]
        public ActionResult Edit(Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                //check if client status has changed to not active
                bool clientStatus = (bool)TempData["ClientStatus"];
                if ((client.Status != clientStatus) && (client.Status == false)) OrderUtilities.UpdateOrderOnClientEdit(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DistributorID = new SelectList(db.Distributors, "ID", "Name", client.DistributorID);
            ViewBag.ActivityTypeID = new SelectList(db.ActivityTypes, "ID", "Description", client.ActivityTypeID);
            ViewBag.CountryID = new SelectList(db.Countries, "ID", "Description", client.CountryID);
            ViewBag.PayTypeID = new SelectList(db.PayTypes, "ID", "Description", client.PayTypeID);
            ViewBag.CurrencyID = new SelectList(db.Currencies, "ID", "FullDescription", client.CurrencyID);
            return View(client);
        }

        //
        // GET: /Client/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        //
        // POST: /Client/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
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