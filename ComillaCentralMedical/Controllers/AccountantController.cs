using ComillaCentralMedical.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ComillaCentralMedical.Controllers
{
    public class AccountantController : Controller
    {
        private readonly HttpClient client;

        public AccountantController()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:9118/") 
            };
        }

        // GET: Accountant/Pending
        public async Task<ActionResult> Pending()
        {
            var pendingBills = new List<Bill>();

            HttpResponseMessage response = await client.GetAsync("api/BillApi");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var allBills = JsonConvert.DeserializeObject<List<Bill>>(json);
                pendingBills = allBills.Where(b => !b.IsConfirmed && !b.IsReturned).ToList();
            }

            return View(pendingBills);
        }

        // POST: Accountant/Confirm/5
        [HttpPost]
        public async Task<ActionResult> Confirm(int id)
        {
            // Step 1: Get the bill from API
            HttpResponseMessage getResponse = await client.GetAsync($"api/BillApi/{id}");
            if (!getResponse.IsSuccessStatusCode)
            {
                TempData["Error"] = "Bill not found.";
                return RedirectToAction("Pending");
            }

            string json = await getResponse.Content.ReadAsStringAsync();
            Bill bill = JsonConvert.DeserializeObject<Bill>(json);

            if (bill.IsConfirmed || bill.IsReturned)
            {
                TempData["Error"] = "Only pending bills can be confirmed.";
                return RedirectToAction("Pending");
            }

            // Step 2: Update the bill
            bill.IsConfirmed = true;
            bill.ConfirmedBy = "Accountant";
            bill.ConfirmedAt = DateTime.Now;

            string updatedJson = JsonConvert.SerializeObject(bill);
            var content = new StringContent(updatedJson, Encoding.UTF8, "application/json");

            HttpResponseMessage putResponse = await client.PutAsync($"api/BillApi/{id}", content);
            if (putResponse.IsSuccessStatusCode)
            {
                // ✅ Step 3: Redirect to Print view instead of Pending
                return RedirectToAction("Print", new { id });
            }

            TempData["Error"] = "Failed to confirm bill.";
            return RedirectToAction("Pending");
        }


        /* // POST: Accountant/Return/5
         [HttpPost]
         public async Task<ActionResult> Return(int id, string returnReason)
         {
             if (string.IsNullOrWhiteSpace(returnReason))
             {
                 TempData["Error"] = "Return reason is required.";
                 return RedirectToAction("Pending");
             }

             HttpResponseMessage getResponse = await client.GetAsync($"api/BillApi/{id}");
             if (!getResponse.IsSuccessStatusCode)
             {
                 TempData["Error"] = "Bill not found.";
                 return RedirectToAction("Pending");
             }

             string json = await getResponse.Content.ReadAsStringAsync();
             Bill bill = JsonConvert.DeserializeObject<Bill>(json);

             if (bill.IsConfirmed || bill.IsReturned)
             {
                 TempData["Error"] = "Only pending bills can be returned.";
                 return RedirectToAction("Pending");
             }

             // Mark as returned
             bill.IsReturned = true;
             bill.ReturnReason = returnReason;
             bill.ReturnedAt = DateTime.Now;

             string updatedJson = JsonConvert.SerializeObject(bill);
             var content = new StringContent(updatedJson, Encoding.UTF8, "application/json");

             HttpResponseMessage putResponse = await client.PutAsync($"api/BillApi/{id}", content);
             if (putResponse.IsSuccessStatusCode)
             {
                 TempData["Success"] = "Bill returned successfully.";
                 return RedirectToAction("Pending");
             }

             TempData["Error"] = "Failed to return bill.";
             return RedirectToAction("Pending");
         }*/

        // GET: Accountant/Report
        public async Task<ActionResult> Report()
        {
            HttpResponseMessage response = await client.GetAsync("api/BillApi");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Unable to fetch data.";
                return RedirectToAction("Pending");
            }

            string json = await response.Content.ReadAsStringAsync();
            var allBills = JsonConvert.DeserializeObject<List<Bill>>(json);

            // Debug: Ensure bills are fetched
            Console.WriteLine($"Total Bills: {allBills.Count}");

            // Filter bills confirmed today
            var today = DateTime.Today;
            var confirmedToday = allBills
                .Where(b => b.IsConfirmed && b.ConfirmedAt.HasValue && b.ConfirmedAt.Value.Date == today)
                .ToList();

            foreach (var bill in confirmedToday)
            {
                Console.WriteLine($"Bill ID: {bill.BillID}, ConfirmedAt: {bill.ConfirmedAt}, TotalAmount: {bill.TotalAmount}");
            }

            // Filter bills for the current month
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var monthlyBills = allBills
                .Where(b => b.IsConfirmed && b.ConfirmedAt.HasValue && b.ConfirmedAt.Value.Month == currentMonth && b.ConfirmedAt.Value.Year == currentYear)
                .ToList();

            // Prepare the ViewModel
            var reportViewModel = new AccountantSummary
            {
                ConfirmedTodayCount = confirmedToday.Count,
                TotalIncomeToday = confirmedToday.Sum(b => (decimal)(b.TotalAmount ?? 0)),
                TotalIncomeThisMonth = monthlyBills.Sum(b => (decimal)(b.TotalAmount ?? 0)),
                Bills = confirmedToday
            };

            return View(reportViewModel);
        }



        // GET: Accountant/Details/5
        public async Task<ActionResult> Details(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"api/BillApi/{id}");

            if (!response.IsSuccessStatusCode)
                return HttpNotFound();

            string json = await response.Content.ReadAsStringAsync();
            Bill bill = JsonConvert.DeserializeObject<Bill>(json);

            return View(bill);
        }

        // GET: Accountant/Print/5
        public async Task<ActionResult> Print(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"api/BillApi/{id}");

            if (!response.IsSuccessStatusCode)
                return HttpNotFound();

            string json = await response.Content.ReadAsStringAsync();
            Bill bill = JsonConvert.DeserializeObject<Bill>(json);

            return View("Print", bill); // Reuse the same Print.cshtml
        }

    }
}
