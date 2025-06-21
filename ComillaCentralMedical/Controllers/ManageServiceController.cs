using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComillaCentralMedical.Context;
using ComillaCentralMedical.Models;

namespace ComillaCentralMedical.Controllers
{
    public class ManageServiceController : Controller
    {
        public MedicalDbContext db;
        public ManageServiceController()
        {
            this.db = new MedicalDbContext();
        }

        // GET: ManageServices
        public ActionResult ManageServices()
        {
            var services = db.Services.ToList();
            return View(services);
        }

        // GET: CreateService
        public ActionResult CreateService()
        {
            return View();
        }

        // POST: CreateService
        [HttpPost]
        public ActionResult CreateService(Service service)
        {
            if (ModelState.IsValid)
            {
                db.Services.Add(service);
                db.SaveChanges();
                TempData["Message"] = "Service added successfully.";
                return RedirectToAction("ManageServices");
            }

            return View(service);
        }

        // GET: EditService
        public ActionResult EditService(int id)
        {
            var service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }

            return View(service);
        }

        // POST: EditService
        [HttpPost]
        public ActionResult EditService(Service service)
        {
            if (ModelState.IsValid)
            {
                db.Entry(service).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "Service updated successfully.";
                return RedirectToAction("ManageServices");
            }

            return View(service);
        }


        public ActionResult DeleteService(int id)
        {
            var user = db.Services.Find(id);
            if (user != null)
            {
                db.Services.Remove(user);
                db.SaveChanges();
            }

            return RedirectToAction("ManageServices");
        }
    }


}