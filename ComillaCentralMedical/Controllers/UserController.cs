using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComillaCentralMedical.Context;
using ComillaCentralMedical.Models;

namespace ComillaCentralMedical.Controllers
{
    public class UserController : Controller
    {
        public MedicalDbContext db;
        public UserController()
        {
            this.db = new MedicalDbContext();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string emailOrPhone, string password)
        {
            if (string.IsNullOrWhiteSpace(emailOrPhone) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Invalid = "Please enter Email/Phone and Password.";
                return View();
            }

            var user = db.Users.FirstOrDefault(u =>
                (u.Email == emailOrPhone || u.Phone == emailOrPhone) &&
                u.Password == password);

            if (user != null)
            {
                Session["UserId"] = user.ID;
                Session["FullName"] = user.FullName; 
                Session["Role"] = user.Role;

                switch (user.Role.ToLower())
                {
                    case "admin":
                        return RedirectToAction("ManageUsers", "Admin");
                    case "receptionist":
                        return RedirectToAction("Dashboard", "Receptionist");
                    case "accountant":
                        return RedirectToAction("Dashboard", "Accountant");
                    default:
                        ViewBag.Invalid = "User role is not recognized.";
                        return View();
                }
            }

            ViewBag.Invalid = "Invalid Email/Phone or Password.";
            return View();
        }
        public ActionResult Logout()
        {
            if (Session["FullName"] == null)
                return RedirectToAction("Login", "User");

            Session.Clear();
            Session.Abandon();
            TempData["Logout"] = "Logged out from System.";
            return View("Login");
        }

    }
}