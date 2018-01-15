using SportsStore.Common;
using SportsStore.Helpers;
using SportsStore.Models;
using SportsStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.Controllers
{
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

        public ActionResult Contact(ContactForm contactForm)
        {
            return View(contactForm);
        }

        [HttpPost]
        public ActionResult Send(ContactForm contactForm)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", contactForm);
            }
            else
            {
                //send  email
                EmailsHelper.SendEmail(db.Settings.FirstOrDefault(s => s.Key.Equals(Constant.ApplicationEmail, StringComparison.InvariantCultureIgnoreCase)).Value, "Formularz kontaktowy", ($"Zgłoszenie od {contactForm.FirstName} {contactForm.LastName} nr.tel: {contactForm.TextArea} o treści {contactForm.TextArea}"));
                return RedirectToAction("Index", "Home");
            }
        }
    }
}