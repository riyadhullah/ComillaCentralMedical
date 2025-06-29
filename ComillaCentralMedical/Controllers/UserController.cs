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
                user.IsActive = true;
                db.SaveChanges();

                switch (user.Role.ToLower())
                {
                    case "admin":
                        return RedirectToAction("Dashboard", "AdminDashboard");
                    case "receptionist":
                        return RedirectToAction("Index", "Receptionist");
                    case "accountant":
                        return RedirectToAction("Report", "Accountant");
                    default:
                        ViewBag.Invalid = "User role is not recognized.";
                        return View();
                }
            }

            ViewBag.Invalid = "Invalid Email/Phone or Password.";
            return View();
        }

        [HttpGet]
        public ActionResult ClearLogin()
        {
            ModelState.Clear();
            return RedirectToAction("Login", "User");
        }
        public ActionResult Logout()
        {
            if (Session["UserId"] != null)
            {
                int userId = (int)Session["UserId"];
                var user = db.Users.FirstOrDefault(u => u.ID == userId);
                if (user != null)
                {
                    user.IsActive = false;
                    db.SaveChanges(); 
                }
            }

            Session.Clear();
            Session.Abandon();
            TempData["Logout"] = "Logged out from System.";
            return RedirectToAction("Login", "User");
        }


        public ActionResult Profile(int id)
        {
            if (Session["FullName"] == null)
                RedirectToAction("Login", "User");

            var user = db.Users.FirstOrDefault(u => u.ID == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View("Profile", user); 
        }


    }
}