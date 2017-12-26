using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.Controllers
{
    public class ContactFormController : Controller
    {
        // GET: ContactForm
        public ActionResult Index(ContactForm contactForm)
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
                return View();
            }
        }
    }
}