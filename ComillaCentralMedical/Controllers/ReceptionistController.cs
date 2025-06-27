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
            

            if (!ModelState.IsValid)
                return View(bill);

            bill.CreatedAt = DateTime.Now;
            bill.IsConfirmed = false;
            bill.CreatedBy = "Receptionist";

            string json = JsonConvert.SerializeObject(bill);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("api/BillApi", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(bill);
        }

        // GET: Receptionist/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"api/BillApi/{id}");

            if (!response.IsSuccessStatusCode)
                return HttpNotFound();

            string json = await response.Content.ReadAsStringAsync();
            Bill bill = JsonConvert.DeserializeObject<Bill>(json);

            if (bill.IsConfirmed)
                return new HttpStatusCodeResult(403, "Cannot edit confirmed bill.");

            return View(bill);
        }

        // POST: Receptionist/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Bill bill)
        {
            if (!ModelState.IsValid)
                return View(bill);

            string json = JsonConvert.SerializeObject(bill);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync($"api/BillApi/{id}", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

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
    }
}
