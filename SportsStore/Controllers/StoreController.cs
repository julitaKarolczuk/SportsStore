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
    public class StoreController : Controller
    {
        private Entities db = new Entities();
        // GET: Store
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            Product product = db.Products.Find(id);
            
            return View(product);
        }

        public ActionResult List(string categoryName)
        {
            var category = db.Categories.Include("Products").FirstOrDefault(c => c.Name.Equals(categoryName, StringComparison.InvariantCultureIgnoreCase));
            if (category != null)
            {
                var products = category.Products.Where(p => !p.Hidden);

                return View(products);
            }

            return View();
        }

        [ChildActionOnly]
        //bezposrednio nie mozna sie do niej dostac
        public ActionResult CategoriesMenu()
        {
            var categories = db.Categories.ToList();
            return PartialView("_CategoriesMenu",categories);
        }
    }
}