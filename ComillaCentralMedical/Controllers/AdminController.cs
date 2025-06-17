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

        public ActionResult CreateUsers()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUsers(User user, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    // Get extension and convert to lowercase
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    string extension = Path.GetExtension(ImageFile.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("ImageFile", "Only JPG, JPEG, or PNG files are allowed.");
                        return View();
                    }

                    // Save file
                    string fileName = user.Phone + extension;
                    string path = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                    ImageFile.SaveAs(path);
                    user.ImagePath = "/Uploads/" + fileName;
                }

                user.JoinDate = DateTime.Today;
                user.IsActive = false;

                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("ManageUsers");
            }

            return View();
        }



        public ActionResult Details(int id)
        {
            var user = db.Users.Find(id);
            if(user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }


    }
}