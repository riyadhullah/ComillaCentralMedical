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
                    // Get the original file extension (.jpg, .png, etc.)
                    string extension = Path.GetExtension(ImageFile.FileName);

                    // Set file name as the user's phone number + extension
                    string fileName = user.Phone + extension;

                    // Combine with server path
                    string path = Path.Combine(Server.MapPath("~/Uploads/"), fileName);

                    // Save file to the folder
                    ImageFile.SaveAs(path);

                    // Save path in database
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