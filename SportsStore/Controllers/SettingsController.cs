using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SportsStore.Models;
using SportsStore.ViewModels;
using SportsStore.Common;

namespace SportsStore.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private Entities db = new Entities();


        // GET: Settings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setting setting = db.Settings.Find(id);
            if (setting == null)
            {
                return HttpNotFound();
            }
            return View(setting);
        }

        // GET: Settings/Create
        public ActionResult Index()
        {
            var model = new SettingsViewModel
            {
                ContactEmail = db.Settings.FirstOrDefault(s => s.Key.Equals(Constant.ContactEmail, StringComparison.InvariantCultureIgnoreCase)) ?? new Setting { Key = Constant.ContactEmail, Value = string.Empty },
                ApplicationEmail = db.Settings.FirstOrDefault(s => s.Key.Equals(Constant.ApplicationEmail, StringComparison.InvariantCultureIgnoreCase)) ?? new Setting { Key = Constant.ApplicationEmail, Value = string.Empty },
                ApplicationEmailPassword = db.Settings.FirstOrDefault(s => s.Key.Equals(Constant.ApplicationEmailPassword, StringComparison.InvariantCultureIgnoreCase)) ?? new Setting { Key = Constant.ApplicationEmailPassword, Value = string.Empty }
            };

            return View(model);
        }

        // POST: Settings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string Email, string ApplicationEmail, string ApplicationEmailPassword)
        {
            var contactEmail = db.Settings.FirstOrDefault(s => s.Key.Equals(Constant.ContactEmail, StringComparison.InvariantCultureIgnoreCase));
            var applicationEmail = db.Settings.FirstOrDefault(s => s.Key.Equals(Constant.ApplicationEmail, StringComparison.InvariantCultureIgnoreCase));
            var applicationEmailPassword = db.Settings.FirstOrDefault(s => s.Key.Equals(Constant.ApplicationEmailPassword, StringComparison.InvariantCultureIgnoreCase));

            if (contactEmail != null)
            {
                contactEmail.Value = Email;
            }
            else
            {
                db.Settings.Add(new Setting
                {
                    Key = Constant.ContactEmail,
                    Value = Email
                });
            }

            if (applicationEmail != null)
            {
                applicationEmail.Value = ApplicationEmail;
            }
            else
            {
                db.Settings.Add(new Setting
                {
                    Key = Constant.ApplicationEmail,
                    Value = ApplicationEmail
                });
            }
            if (applicationEmailPassword != null)
            {
                applicationEmailPassword.Value = ApplicationEmailPassword;
            }
            else
            {
                db.Settings.Add(new Setting
                {
                    Key = Constant.ApplicationEmailPassword,
                    Value = ApplicationEmailPassword
                });
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: Settings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setting setting = db.Settings.Find(id);
            if (setting == null)
            {
                return HttpNotFound();
            }
            return View(setting);
        }

        // POST: Settings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Key,Value")] Setting setting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(setting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(setting);
        }

        // GET: Settings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setting setting = db.Settings.Find(id);
            if (setting == null)
            {
                return HttpNotFound();
            }
            return View(setting);
        }

        // POST: Settings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Setting setting = db.Settings.Find(id);
            db.Settings.Remove(setting);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
