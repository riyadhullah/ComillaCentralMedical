using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComillaCentralMedical.Context;
using ComillaCentralMedical.Models;

namespace ComillaCentralMedical.Controllers
{
    public class AdminController : Controller
    {
        public MedicalDbContext db;
        public AdminController() 
        {
            this.db = new MedicalDbContext();
        }

        public ActionResult ManageUsers()
        {
            var users = db.Users.ToList();
            return View(users);
        }

        public ActionResult CreateUsers(User user)
        {
            return View(user);
        }

        [HttpPost]
        public ActionResult CreateUsers(User user, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    // Get file name and path
                    string fileName = Path.GetFileName(ImageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Uploads/"), fileName);

                    // Save image to folder
                    ImageFile.SaveAs(path);

                    // Save filename to database
                    user.ImagePath = "/Uploads/" + fileName;
                    
                }

                DateTime JoinDate = DateTime.Today;
                user.JoinDate = JoinDate;
                user.IsActive = false;

                // Save user to database
                db.Users.Add(user);
                db.SaveChanges();

                return RedirectToAction("ManageUsers");
            }

            return View(user);
        }



    }
}