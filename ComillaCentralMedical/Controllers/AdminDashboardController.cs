using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ComillaCentralMedical.Context;

namespace ComillaCentralMedical.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly MedicalDbContext db;

        public AdminDashboardController()
        {
            db = new MedicalDbContext();
        }

        public ActionResult Dashboard()
        {
            if (Session["FullName"] == null)
                return RedirectToAction("Login", "User");

            var today = DateTime.Today;

            var todayBills = db.Bills
                .Where(b => DbFunctions.TruncateTime(b.CreatedAt) == today)
                .ToList();

            var totalInvoices = db.Bills.Count();

            var totalIncome = db.Bills
                .Where(b => b.IsConfirmed && b.TotalAmount.HasValue)
                .Sum(b => b.TotalAmount.Value);

            var recentActivities = db.Bills
                .Where(b => DbFunctions.TruncateTime(b.CreatedAt) == today)
                .ToList();

            ViewBag.TodayInvoices = todayBills;
            ViewBag.TotalInvoices = totalInvoices;
            ViewBag.TotalIncome = totalIncome;
            ViewBag.RecentActivities = recentActivities;

            return View();
        }
    }
}
