using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toptix.Models;

namespace Toptix.Controllers
{
    public class PayConditionController : Controller
    {
        private TCRMDBContext db = new TCRMDBContext();

        //
        // GET: /PayConditions/

        public ActionResult Index()
        {
            return View(db.PayConditions.ToList());
        }

        //
        // GET: /PayConditions/Details/5

        public ActionResult Details(int id = 0)
        {
            PayCondition payconditions = db.PayConditions.Find(id);
            if (payconditions == null)
            {
                return HttpNotFound();
            }
            return View(payconditions);
        }

        //
        // GET: /PayConditions/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PayConditions/Create

        [HttpPost]
        public ActionResult Create(PayCondition payconditions)
        {
            if (ModelState.IsValid)
            {
                db.PayConditions.Add(payconditions);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(payconditions);
        }

        //
        // GET: /PayConditions/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PayCondition payconditions = db.PayConditions.Find(id);
            if (payconditions == null)
            {
                return HttpNotFound();
            }
            return View(payconditions);
        }

        //
        // POST: /PayConditions/Edit/5

        [HttpPost]
        public ActionResult Edit(PayCondition payconditions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payconditions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(payconditions);
        }

        //
        // GET: /PayConditions/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PayCondition payconditions = db.PayConditions.Find(id);
            if (payconditions == null)
            {
                return HttpNotFound();
            }
            return View(payconditions);
        }

        //
        // POST: /PayConditions/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            PayCondition payconditions = db.PayConditions.Find(id);
            db.PayConditions.Remove(payconditions);
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