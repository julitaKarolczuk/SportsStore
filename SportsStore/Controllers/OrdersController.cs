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

namespace SportsStore.Controllers
{
    public class OrdersController : Controller
    {
        private Entities db = new Entities();

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

                    order.CreationDate = DateTime.Now;
                    order.UserId = User.Identity.GetUserId();
                    order.Status = "Oczekujące na realizację";

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

                    WebMail.SmtpServer = "smtp.gmail.com";
                    //gmail port to send emails  
                    WebMail.SmtpPort = 587;
                    WebMail.SmtpUseDefaultCredentials = true;
                    //sending emails with secure protocol  
                    WebMail.EnableSsl = true;
                    //EmailId used to send emails from application  
                    WebMail.UserName = "";
                    WebMail.Password = "";

                    //Sender email address.  
                    WebMail.From = "krzysztof.kirylowicz@gmail.com";

                    //Send email  
                    WebMail.Send(to: "krzys0149@gmail.com", subject: "dupa1", body: "dupa2");

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
        public ActionResult Edit([Bind(Include = "Id,Status,Payment,Shipment,Address,UserId")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
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
