using SportsStore.Common;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        Entities db = new Entities();
        // GET: Admin
        public ActionResult Index()
        {
            var counter = db.Settings.FirstOrDefault(s => s.Key.Equals(Constant.Counter, StringComparison.InvariantCultureIgnoreCase));
            ViewBag.Counter = counter.Value;
            return View();
        }
    }
}