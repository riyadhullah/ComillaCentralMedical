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
            string confirmPassword = Request["ConfirmPassword"];

            if (user.Password != confirmPassword)
            {
                ViewBag.PasswordMismatch = "Password and Confirm Password do not match.";
                return View(user); // return with error and user input
            }

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
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        public ActionResult Delete(int id)
        {
            var user = db.Users.Find(id);
            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
            }

            return RedirectToAction("ManageUsers");
        }

        public ActionResult EditUser(int id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        
        [HttpPost]
        public ActionResult EditUser(FormCollection form, HttpPostedFileBase ImageFile)
        {
            int id = int.Parse(form["ID"]);
            var user = db.Users.Find(id);
            if (user == null) return HttpNotFound();

            string password = form["Password"];
            string confirmPassword = form["ConfirmPassword"];

            if (password != confirmPassword)
            {
                TempData["PasswordError"] = "Password and Confirm Password do not match.";
                return RedirectToAction("EditUser", new { id = user.ID });

            }

            user.FullName = form["FullName"];
            user.Phone = form["Phone"];
            user.Password = password;
            user.Email = form["Email"];
            user.Role = form["Role"];
            user.Gender = form["Gender"];
            user.Address = form["Address"];
            user.IsActive = form["IsActive"] == "true" || form["IsActive"] == "on";

            if (DateTime.TryParse(form["DOB"], out DateTime dob)) user.DOB = dob;
            if (DateTime.TryParse(form["JoinDate"], out DateTime joinDate)) user.JoinDate = joinDate;

            // Image upload
            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                string extension = Path.GetExtension(ImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Only JPG, JPEG, or PNG files are allowed.");
                    return View(user);
                }

                string fileName = user.Phone + extension;
                string path = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                ImageFile.SaveAs(path);

                user.ImagePath = "/Uploads/" + fileName;
            }

            db.SaveChanges();
            return RedirectToAction("ManageUsers");
        }


    }
}