using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using SportsStore.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.Controllers
{
    public class CartController : Controller
    {
        Entities db = new Entities();

        public ActionResult Index()
        {
            var user = db.AspNetUsers.Find(User.Identity.GetUserId());
            var items = new List<CartItem>();
            ViewBag.Discount = user != null ? user.Discount : 0;
            if (Request.Cookies.AllKeys.Contains("Cart"))
            {
                var cookie = Request.Cookies["Cart"];
                items = JsonConvert.DeserializeObject<List<CartItem>>(cookie.Value);
            }

            return View(items);
        }

        public ActionResult AddToCart(int id)
        {
            var product = db.Products.Find(id);
            HttpCookie cookie;
            if (Request.Cookies.AllKeys.Contains("Cart"))
            {
                cookie = Request.Cookies["Cart"];
                var items = JsonConvert.DeserializeObject<List<CartItem>>(cookie.Value);
                var item = items.FirstOrDefault(i => i.Product.Id == id);
                if (item != null)
                    item.Count = item.Count + 1;
                else
                    items.Add(new CartItem(product));

                cookie.Value = JsonConvert.SerializeObject(items);
                Response.SetCookie(cookie);
            }
            else
            {
                cookie = new HttpCookie("Cart");
                var items = new List<CartItem> {
                    new CartItem(product)
                };
                cookie.Value = JsonConvert.SerializeObject(items);
                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(int id)
        {
            var product = db.Products.Find(id);
            HttpCookie cookie;
            if (Request.Cookies.AllKeys.Contains("Cart"))
            {
                cookie = Request.Cookies["Cart"];
                var items = JsonConvert.DeserializeObject<List<CartItem>>(cookie.Value);
                var item = items.FirstOrDefault(i => i.Product.Id == id);
                if (item != null)
                {
                    items.Remove(item);
                    cookie.Value = JsonConvert.SerializeObject(items);
                    Response.SetCookie(cookie);
                }
            }

            return RedirectToAction("Index");
        }
    }
}