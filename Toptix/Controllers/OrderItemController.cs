using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Toptix.Models;
using PagedList;

namespace Toptix.Controllers
{
    public class OrderItemController : Controller
    {
        private TCRMDBContext db = new TCRMDBContext();

        //
        // GET: /OrderItem/

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //Sorting
            ViewBag.IDSortParam = String.IsNullOrEmpty(sortOrder) ? "ID desc" : "";
            ViewBag.OrderSortParm = sortOrder == "Order" ? "Order desc" : "Order";
            var orderitemsQueryable = db.OrderItems.Include(o => o.Product).Include(o => o.EnumCalculationType).Include(o => o.Order).Include(o => o.Currency);
            var orderitems = db.OrderItems.Include(o => o.Product).Include(o => o.EnumCalculationType).Include(o => o.Order).Include(o => o.Currency).Take(50);
            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else page = 1;

            //Paging
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            ViewBag.CurrentFilter = searchString;
            //searching 
            if (!String.IsNullOrEmpty(searchString))
            {
                int orderID;
                try
                {
                    orderID = Convert.ToInt32(searchString);
                }
                catch (Exception e) {
                    //add exception details
                    return RedirectToAction("Index");
                }
                orderitems = orderitemsQueryable.Where(o => o.OrderID == orderID);
            }

            switch (sortOrder)
            {
                case "ID desc":
                    orderitems = orderitems.OrderByDescending(o => o.ID);
                    break;
                case "Order desc":
                    orderitems = orderitems.OrderByDescending(o => o.OrderID);
                    break;
                case "Order":
                    orderitems = orderitems.OrderBy(o => o.OrderID);
                    break;
                default:
                    orderitems = orderitems.OrderBy(o => o.ID);
                    break;
            }


            return View(orderitems.ToPagedList(pageNumber, pageSize));
        }
        
         //GET: /OrderItem/Edit/5

        public ActionResult Edit(int id = 0)
        {
            OrderItem orderitem = db.OrderItems.Find(id);
            if (orderitem == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", orderitem.ProductID);
            return View(orderitem);
        }

        //
        // POST: /OrderItem/Edit/5

        [HttpPost]
        public ActionResult Edit(OrderItem orderitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderitem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", orderitem.ProductID);
            return View(orderitem);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}