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
            var category = db.Categories.FirstOrDefault();
            return RedirectToAction("List", "Store", new { categoryName = category != null ? category.Name : string.Empty });
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