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
    public class ReceptionistController : Controller
    {
        private readonly HttpClient client;

        public ReceptionistController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:9118/"); // API port
        }

        // GET: Receptionist
        public async Task<ActionResult> Index()
        {
            List<Bill> bills = new List<Bill>();
            HttpResponseMessage response = await client.GetAsync("api/BillApi");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                bills = JsonConvert.DeserializeObject<List<Bill>>(json);
            }

            return View(bills);
        }

        // GET: Receptionist/Details/5
        public async Task<ActionResult> Details(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"api/BillApi/{id}");

            if (!response.IsSuccessStatusCode)
                return HttpNotFound();

            string json = await response.Content.ReadAsStringAsync();
            Bill bill = JsonConvert.DeserializeObject<Bill>(json);

            return View(bill);
        }

        // GET: Receptionist/Create
        public async Task<ActionResult> Create()
        {
            HttpResponseMessage response = await client.GetAsync("api/ServiceApi");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                ViewBag.Services = JsonConvert.DeserializeObject<List<Service>>(json);
            }
            else
            {
                ViewBag.Services = new List<Service>();
            }

            return View();
        }

        // POST: Receptionist/Create
        [HttpPost]
        public async Task<ActionResult> Create(Bill bill)
        {
            // Model-level validation (like Required, Regex, etc.)
            if (!ModelState.IsValid)
            {
                // Re-fetch services if returning to view
                HttpResponseMessage serviceResponse = await client.GetAsync("api/ServiceApi");
                if (serviceResponse.IsSuccessStatusCode)
                {
                    string serviceJson = await serviceResponse.Content.ReadAsStringAsync();
                    ViewBag.Services = JsonConvert.DeserializeObject<List<Service>>(serviceJson);
                }
                else
                {
                    ViewBag.Services = new List<Service>();
                }
                return View(bill);
            }

            // ✅ Custom Validation #1: At least one service
            if (bill.BillItems == null || !bill.BillItems.Any())
            {
                ModelState.AddModelError("", "At least one service must be added.");
            }

            // ✅ Custom Validation #2: Overall Discount cannot be negative
            if (bill.OverallDiscountRate < 0)
            {
                ModelState.AddModelError("OverallDiscountRate", "Overall discount cannot be negative.");
            }

            // Return view with errors if any
            if (!ModelState.IsValid)
            {
                HttpResponseMessage serviceResponse = await client.GetAsync("api/ServiceApi");
                if (serviceResponse.IsSuccessStatusCode)
                {
                    string serviceJson = await serviceResponse.Content.ReadAsStringAsync();
                    ViewBag.Services = JsonConvert.DeserializeObject<List<Service>>(serviceJson);
                }
                else
                {
                    ViewBag.Services = new List<Service>();
                }
                return View(bill);
            }

            // ✅ Set required fields before saving
            bill.CreatedAt = DateTime.Now;
            bill.IsConfirmed = false;
            bill.CreatedBy = "Receptionist";

            // ✅ Send data to API
            string json = JsonConvert.SerializeObject(bill);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("api/BillApi", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            // Fallback in case API fails
            ModelState.AddModelError("", "Failed to create bill. Please try again.");

            // Reload services before returning
            HttpResponseMessage serviceReload = await client.GetAsync("api/ServiceApi");
            if (serviceReload.IsSuccessStatusCode)
            {
                string jsonReload = await serviceReload.Content.ReadAsStringAsync();
                ViewBag.Services = JsonConvert.DeserializeObject<List<Service>>(jsonReload);
            }
            else
            {
                ViewBag.Services = new List<Service>();
            }

            return View(bill);
        }


        // GET: Receptionist/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            // Get bill
            HttpResponseMessage response = await client.GetAsync($"api/BillApi/{id}");

            if (!response.IsSuccessStatusCode)
                return HttpNotFound();

            string json = await response.Content.ReadAsStringAsync();
            Bill bill = JsonConvert.DeserializeObject<Bill>(json);

            if (bill.IsConfirmed)
                return new HttpStatusCodeResult(403, "Cannot edit confirmed bill.");

            // 🔥 Load services for dropdown
            HttpResponseMessage serviceResponse = await client.GetAsync("api/ServiceApi");
            if (serviceResponse.IsSuccessStatusCode)
            {
                string serviceJson = await serviceResponse.Content.ReadAsStringAsync();
                ViewBag.Services = JsonConvert.DeserializeObject<List<Service>>(serviceJson);
            }
            else
            {
                ViewBag.Services = new List<Service>();
            }

            return View(bill);
        }


        // POST: Receptionist/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Bill bill)
        {
            // Validate
            if (!ModelState.IsValid)
            {
                // Refetch services if needed
                HttpResponseMessage serviceResponse = await client.GetAsync("api/ServiceApi");
                if (serviceResponse.IsSuccessStatusCode)
                {
                    string serviceJson = await serviceResponse.Content.ReadAsStringAsync();
                    ViewBag.Services = JsonConvert.DeserializeObject<List<Service>>(serviceJson);
                }
                else
                {
                    ViewBag.Services = new List<Service>();
                }
                return View(bill);
            }

            // ✅ Ensure at least one service
            if (bill.BillItems == null || !bill.BillItems.Any())
            {
                ModelState.AddModelError("", "Please add at least one service.");
                return View(bill);
            }

            // ✅ Fetch unit price and discount again from service DB to avoid tampering
            HttpResponseMessage serviceFetch = await client.GetAsync("api/ServiceApi");
            List<Service> allServices = new List<Service>();

            if (serviceFetch.IsSuccessStatusCode)
            {
                string json = await serviceFetch.Content.ReadAsStringAsync();
                allServices = JsonConvert.DeserializeObject<List<Service>>(json);
            }

            // ✅ Update UnitPrice, DiscountRate for each item and calculate subtotal
            double total = 0;
            foreach (var item in bill.BillItems)
            {
                var service = allServices.FirstOrDefault(s => s.ServiceID == item.ServiceID);
                if (service != null)
                {
                    item.UnitPrice = service.UnitCost;
                    double discount = service.DiscountRate ?? 0;
                    total += item.UnitPrice * item.Quantity * (1 - discount / 100);
                }
            }

            // ✅ Apply overall discount
            double overallDiscount = bill.OverallDiscountRate ?? 0;
            bill.TotalAmount = total * (1 - overallDiscount / 100);

            // ✅ Keep original metadata intact
            bill.IsConfirmed = false;

            // Update via API
            string updatedJson = JsonConvert.SerializeObject(bill);
            var content = new StringContent(updatedJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync($"api/BillApi/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Bill updated successfully.";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Failed to update bill.";
            return View(bill);
        }


        // GET: Receptionist/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"api/BillApi/{id}");

            if (!response.IsSuccessStatusCode)
                return HttpNotFound();

            string json = await response.Content.ReadAsStringAsync();
            Bill bill = JsonConvert.DeserializeObject<Bill>(json);

            return View(bill);
        }

        // POST: Receptionist/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync($"api/BillApi/{id}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return new HttpStatusCodeResult(400, "Delete failed.");
        }

        public async Task<ActionResult> Print(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"api/BillApi/{id}");

            if (!response.IsSuccessStatusCode)
                return HttpNotFound();

            string json = await response.Content.ReadAsStringAsync();
            Bill bill = JsonConvert.DeserializeObject<Bill>(json);

            return View("Print", bill);
        }


        public async Task<PartialViewResult> SearchBills(string search, bool confirmed)
        {
            HttpResponseMessage response = await client.GetAsync("api/BillApi");

            List<Bill> bills = new List<Bill>();
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                bills = JsonConvert.DeserializeObject<List<Bill>>(json);
            }

            var filtered = bills.Where(b => b.IsConfirmed == confirmed).ToList();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                filtered = filtered.Where(b =>
                    (b.PatientName != null && b.PatientName.ToLower().Contains(search)) ||
                    (b.Phone != null && b.Phone.Contains(search))
                ).ToList();
            }

            return PartialView("_BillTable", filtered);
        }


    }
}
