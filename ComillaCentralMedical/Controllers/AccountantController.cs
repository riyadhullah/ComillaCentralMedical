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

        public async Task<ActionResult> Pending()
        {
            if (Session["FullName"] == null)
                return RedirectToAction("Login", "User");

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

        [HttpPost]
        public async Task<ActionResult> Confirm(int id)
        {
            if (Session["FullName"] == null)
                return RedirectToAction("Login", "User");

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

            bill.IsConfirmed = true;
            bill.ConfirmedBy = "Accountant";
            bill.ConfirmedAt = DateTime.Now;

            string updatedJson = JsonConvert.SerializeObject(bill);
            var content = new StringContent(updatedJson, Encoding.UTF8, "application/json");

            HttpResponseMessage putResponse = await client.PutAsync($"api/BillApi/{id}", content);
            if (putResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Print", new { id });
            }

            TempData["Error"] = "Failed to confirm bill.";
            return RedirectToAction("Pending");
        }

        public async Task<ActionResult> Report()
        {
            if (Session["FullName"] == null)
                return RedirectToAction("Login", "User");

            List<Bill> bills = new List<Bill>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:9118/");
                HttpResponseMessage response = await client.GetAsync("api/BillApi");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    bills = JsonConvert.DeserializeObject<List<Bill>>(json);
                }
            }

            bills = bills.Where(b => b.IsConfirmed).ToList();

            return View(bills);
        }

        public async Task<ActionResult> Details(int id)
        {
            if (Session["FullName"] == null)
                return RedirectToAction("Login", "User");

            HttpResponseMessage response = await client.GetAsync($"api/BillApi/{id}");

            if (!response.IsSuccessStatusCode)
                return HttpNotFound();

            string json = await response.Content.ReadAsStringAsync();
            Bill bill = JsonConvert.DeserializeObject<Bill>(json);

            return View(bill);
        }

        public async Task<ActionResult> Print(int id)
        {
            if (Session["FullName"] == null)
                return RedirectToAction("Login", "User");

            HttpResponseMessage response = await client.GetAsync($"api/BillApi/{id}");

            if (!response.IsSuccessStatusCode)
                return HttpNotFound();

            string json = await response.Content.ReadAsStringAsync();
            Bill bill = JsonConvert.DeserializeObject<Bill>(json);

            return View("Print", bill);
        }

        public async Task<PartialViewResult> SearchPendingBills(string search)
        {
            if (Session["FullName"] == null)
                RedirectToAction("Login", "User");

            HttpResponseMessage response = await client.GetAsync("api/BillApi");

            List<Bill> bills = new List<Bill>();
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                bills = JsonConvert.DeserializeObject<List<Bill>>(json);
            }

            var filtered = bills
                .Where(b => !b.IsConfirmed && (
                    string.IsNullOrEmpty(search) ||
                    (b.PatientName != null && b.PatientName.ToLower().Contains(search.ToLower())) ||
                    (b.Phone != null && b.Phone.Contains(search))
                ))
                .ToList();

            return PartialView("_PendingBillTable", filtered);
        }

        public async Task<PartialViewResult> SearchConfirmedBills(string search)
        {
            if (Session["FullName"] == null)
                RedirectToAction("Login", "User");

            HttpResponseMessage response = await client.GetAsync("api/BillApi");

            List<Bill> bills = new List<Bill>();
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                bills = JsonConvert.DeserializeObject<List<Bill>>(json);
            }

            var filtered = bills
                .Where(b => b.IsConfirmed && (
                    string.IsNullOrEmpty(search) ||
                    (b.PatientName != null && b.PatientName.ToLower().Contains(search.ToLower())) ||
                    (b.Phone != null && b.Phone.Contains(search))
                ))
                .ToList();

            return PartialView("_ConfirmedBillTable", filtered);
        }
    }
}
