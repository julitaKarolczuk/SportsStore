using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.Controllers
{
    public class SearchController : Controller
    {
        private Entities db = new Entities();

        // GET: Search
        public ActionResult Index(string search)
        {
            var products = db.Products.Where(p =>
                !p.Hidden && (
                    p.Name.Contains(search) ||
                    p.ShortDescription.Contains(search) ||
                    p.Description.Contains(search) ||
                    p.Category.Name.Contains(search) ||
                    p.Producer.Contains(search)))
                .ToList();

            return View(products);
        }

        public ActionResult Advanced(string name, string shortDescription, string description, string category, string producer)
        {
            var products = db.Products.Where(p => !p.Hidden);

            if (!string.IsNullOrEmpty(name))
                products = products.Where(p => p.Name.Contains(name));

            if (!string.IsNullOrEmpty(shortDescription))
                products = products.Where(p => p.Name.Contains(shortDescription));

            if (!string.IsNullOrEmpty(description))
                products = products.Where(p => p.Name.Contains(description));

            if (!string.IsNullOrEmpty(category))
                products = products.Where(p => p.Name.Contains(category));

            if (!string.IsNullOrEmpty(producer))
                products = products.Where(p => p.Name.Contains(producer));

            return View(products.ToList());
        }
    }
}