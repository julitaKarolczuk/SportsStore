using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.Controllers
{
    public class CartController : Controller
    {
        // GET: Card
        public ActionResult Index()
        {
            return View();
        }
    }
}