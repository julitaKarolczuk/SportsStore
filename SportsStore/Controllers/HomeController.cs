using SportsStore.Models;
using SportsStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        
        private Entities db = new Entities();

        
        public ActionResult Statute()
        {
            return View();
        }

        public ActionResult Index()
        {
            var categories = db.Categories.ToList();

            var products = db.Products.Where(p => !p.Hidden).Take(3).ToList();

            var vm = new HomeViewModel()
            {
                Categories = categories,
                Products = products
            };

            return View(vm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}