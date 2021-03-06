﻿using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Helpers;
using SportsStore.Common;

namespace SportsStore.Controllers
{
    public class ContactFormController : Controller
    {
        Entities db = new Entities();
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
                //send  email
                EmailsHelper.SendEmail(db.Settings.FirstOrDefault(s => s.Key.Equals(Constant.ApplicationEmail, StringComparison.InvariantCultureIgnoreCase)).Value,"Formularz kontaktowy",($"Zgłoszenie od {contactForm.FirstName} {contactForm.LastName} nr.tel: {contactForm.TextArea} o treści {contactForm.TextArea}"));
                return RedirectToAction("Index", "Home");
            }
        }

    }
}