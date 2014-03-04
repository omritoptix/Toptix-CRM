using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toptix.Models;

namespace Toptix.Controllers.Admin
{
    public class GlobalParametersController : Controller
    {
        private TCRMDBContext db = new TCRMDBContext();

        //
        // GET: /GlobalParameters/

        public ActionResult Index()
        {
            return View(db.GlobalParameteres.ToList());
        }

        // GET: /GlobalParameters/Edit/5

        public ActionResult Edit(int id = 0)
        {
            GlobalParameters globalparameters = db.GlobalParameteres.Find(id);
            if (globalparameters == null)
            {
                return HttpNotFound();
            }
            ViewBag.CurrencyID = new SelectList(db.Currencies, "ID", "FullDescription", globalparameters.CurrencyID);
            return View(globalparameters);
        }

        //
        // POST: /GlobalParameters/Edit/5

        [HttpPost]
        public ActionResult Edit(GlobalParameters globalparameters)
        {
            if (ModelState.IsValid)
            {
                db.Entry(globalparameters).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.CurrencyID = new SelectList(db.Currencies, "ID", "FullDescription", globalparameters.CurrencyID);
            return View(globalparameters);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}