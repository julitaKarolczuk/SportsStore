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
        public ArrayList cart = new ArrayList();
        // GET: Cart
        public ActionResult Index()
        {
            cart.Add(new CartItem { Product = db.Products.Find(7), Quantity = 2, TotalPrice = 85 });
            return View(cart);
        }

        public ActionResult AddToCart(int productId)
        {
            var product = db.Order_Product.FirstOrDefault(p => p.ProductId == productId);
            cart.Add(new CartItem { Product = db.Products.Find(productId), Quantity = 1, TotalPrice = product.Product.Price* product.Amount});
            return View(cart);
        }
    }
}