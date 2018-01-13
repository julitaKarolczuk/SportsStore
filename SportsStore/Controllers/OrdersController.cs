using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SportsStore.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using SportsStore.ViewModels;
using System.Web.Helpers;
using SportsStore.Helpers;

namespace SportsStore.Controllers
{
    public class OrdersController : Controller
    {
        private Entities db = new Entities();

        // GET: OrdersForAdmin
        public ActionResult IndexAdmin()
        {
            var orders = db.Orders;
            return View(orders.ToList());
        }

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.AspNetUser);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        [ValidateInput(false)]
        public ActionResult Create()
        {
            if (Request.Cookies.AllKeys.Contains("Cart"))
            {
                var cookie = Request.Cookies["Cart"];
                var items = JsonConvert.DeserializeObject<List<CartItem>>(cookie.Value);

                var currentUserAddress = db.AspNetUsers.Find(User.Identity.GetUserId()).Address;
                var model = new OrderViewModel
                {
                    UserAddress = currentUserAddress,
                    Card = items
                };

                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,Payment,Shipment,Address")] Order order)
        {
            if (ModelState.IsValid)
            {
                if (Request.Cookies.AllKeys.Contains("Cart"))
                {
                    var cookie = Request.Cookies["Cart"];
                    var items = JsonConvert.DeserializeObject<List<CartItem>>(cookie.Value);

                    var userId = User.Identity.GetUserId();

                    order.CreationDate = DateTime.Now;
                    order.UserId = userId;
                    order.Status = "Nowe";

                    order.Order_Product = items.Select(i => new Order_Product
                    {
                        ProductId = i.Product.Id,
                        Amount = i.Count
                    }).ToList();

                    db.Orders.Add(order);
                    db.SaveChanges();

                    Response.Cookies.Remove("Cart");
                    cookie.Expires = DateTime.Now.AddDays(-10);
                    cookie.Value = null;
                    Response.SetCookie(cookie);

                    var receiverEmail = db.AspNetUsers.Find(userId)?.Email;

                    var message = string.Empty;
                    double sum = 0;

                    foreach (var product in items)
                    {
                        sum += (double)(product.Count * product.Product.Price);
                        message += $"Nazwa produktu: {product.Product.Name}, Cena: {product.Product.Price}, Ilosc: {product.Count}, Suma: {((double)(product.Count * product.Product.Price)).ToString()}";
                    }

                    message += $"Suma zamówienia: {sum.ToString()}, Sposób dostawy: {order.Shipment}, Metoda płatności: {order.Payment}";

                    EmailsHelper.SendEmail(receiverEmail, "Potwierdzenie zamówienia", message);

                    return RedirectToAction("Index");
                }
            }

            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                var currentOrder = db.Orders.Find(order.Id);
                var oldStatus = currentOrder.Status;
                currentOrder.Status = order.Status;
                db.SaveChanges();

                var receiverEmail = db.AspNetUsers.Find(order.UserId)?.Address;
                if (!string.IsNullOrEmpty(receiverEmail))
                    EmailsHelper.SendEmail(receiverEmail, "Zmiana statusu zamówienia", $"Status Twojego zamówienia został zmieniony z '{oldStatus}' na '{order.Status}'");

                return RedirectToAction("Index");

            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
