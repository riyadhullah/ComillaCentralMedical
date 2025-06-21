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
    public class AdminDashboardController : Controller
    {
        public MedicalDbContext db;
        public AdminDashboardController()
        {
            this.db = new MedicalDbContext();
        }

        // GET: Admin/Dashboard
        public ActionResult Dashboard()
        {
            var totalPatients = 20; //db.Patients.Count();
            var totalInvoices = 25; //db.Invoices.Count();
            var totalIncome = 35000; //db.Invoices.Sum(i => (decimal?)i.Total) ?? 0;
            /*var recentActivities = db.ActivityLogs
                .OrderByDescending(a => a.Timestamp)
                .Take(5)
                .Select(a => new { a.Action, a.Timestamp, a.User.FullName })
                .ToList();*/

            ViewBag.TotalPatients = totalPatients;
            ViewBag.TotalInvoices = totalInvoices;
            ViewBag.TotalIncome = totalIncome;
           // ViewBag.RecentActivities = recentActivities;

            return View();
        }
    }
}