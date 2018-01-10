using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class Order_ProductController : Controller
    {
        private Entities db = new Entities();

        // GET: Order_Product
        public ActionResult Index()
        {
            var order_Product = db.Order_Product.Include(o => o.Order).Include(o => o.Product);
            return View(order_Product.ToList());
        }

        // GET: Order_Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order_Product order_Product = db.Order_Product.Find(id);
            if (order_Product == null)
            {
                return HttpNotFound();
            }
            return View(order_Product);
        }

        // GET: Order_Product/Create
        public ActionResult Create()
        {
            ViewBag.OrderId = new SelectList(db.Orders, "Id", "Status");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            return View();
        }

        // POST: Order_Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,ProductId,Amount")] Order_Product order_Product)
        {
            if (ModelState.IsValid)
            {
                db.Order_Product.Add(order_Product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderId = new SelectList(db.Orders, "Id", "Status", order_Product.OrderId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", order_Product.ProductId);
            return View(order_Product);
        }

        // GET: Order_Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order_Product order_Product = db.Order_Product.Find(id);
            if (order_Product == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderId = new SelectList(db.Orders, "Id", "Status", order_Product.OrderId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", order_Product.ProductId);
            return View(order_Product);
        }

        // POST: Order_Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,ProductId,Amount")] Order_Product order_Product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order_Product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderId = new SelectList(db.Orders, "Id", "Status", order_Product.OrderId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", order_Product.ProductId);
            return View(order_Product);
        }

        // GET: Order_Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order_Product order_Product = db.Order_Product.Find(id);
            if (order_Product == null)
            {
                return HttpNotFound();
            }
            return View(order_Product);
        }

        // POST: Order_Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order_Product order_Product = db.Order_Product.Find(id);
            db.Order_Product.Remove(order_Product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
