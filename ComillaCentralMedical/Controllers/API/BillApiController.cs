using ComillaCentralMedical.Models;
using ComillaCentralMedical.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Data.Entity;

namespace ComillaCentralMedical.Controllers.API
{
    public class BillApiController : ApiController
    {
        private MedicalDbContext db = new MedicalDbContext();

        // GET: api/BillApi
        public IEnumerable<Bill> GetAllBills()
        {
            return db.Bills
                     .Include("BillItems.Service")
                     .ToList();
        }



        // GET: api/BillApi/5
        [HttpGet]
        public IHttpActionResult GetBill(int id)
        {
            var bill = db.Bills
                         .Include("BillItems.Service")
                         .FirstOrDefault(b => b.BillID == id);

            if (bill == null)
                return NotFound();

            return Ok(bill);
        }


        // POST: api/BillApi
        [HttpPost]
        public IHttpActionResult CreateBill(Bill bill)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bill.CreatedAt = DateTime.Now;
            bill.IsConfirmed = false;
            bill.IsReturned = false;

            db.Bills.Add(bill);
            db.SaveChanges();

            return Ok(bill);
        }

        // PUT: api/BillApi/5
        [HttpPut]
        public IHttpActionResult UpdateBill(int id, Bill updated)
        {
            var bill = db.Bills.Include("BillItems").FirstOrDefault(b => b.BillID == id);
            if (bill == null)
                return NotFound();

            // Update only confirmation and return properties if applicable
            if (updated.IsConfirmed && !bill.IsConfirmed)
            {
                bill.IsConfirmed = true;
                bill.ConfirmedBy = updated.ConfirmedBy ?? bill.ConfirmedBy;
                bill.ConfirmedAt = DateTime.Now;
            }

            if (updated.IsReturned && !bill.IsReturned)
            {
                bill.IsReturned = true;
                bill.ReturnReason = updated.ReturnReason;
                bill.ReturnedAt = DateTime.Now;
            }

            // Only allow editing if bill is not confirmed
            if (!bill.IsConfirmed)
            {
                // Basic info
                bill.PatientName = updated.PatientName;
                bill.Phone = updated.Phone;
                bill.OverallDiscountRate = updated.OverallDiscountRate;
                bill.TotalAmount = updated.TotalAmount;

                // Clear return info
                bill.IsReturned = false;
                bill.ReturnReason = null;
                bill.ReturnedAt = null;

                // 🔥 Remove old BillItems
                db.BillItems.RemoveRange(bill.BillItems);

                // 🔁 Add updated BillItems
                foreach (var item in updated.BillItems)
                {
                    item.BillID = bill.BillID; // Important to set FK
                    db.BillItems.Add(item);
                }
            }

            db.SaveChanges();
            return Ok("Bill updated.");
        }


        // DELETE: api/BillApi/5
        [HttpDelete]
        public IHttpActionResult DeleteBill(int id)
        {
            var bill = db.Bills.Find(id);
            if (bill == null)
                return NotFound();

            if (bill.IsConfirmed)
                return BadRequest("Cannot delete confirmed bill.");

            db.Bills.Remove(bill);
            db.SaveChanges();
            return Ok("Bill deleted.");
        }
    }
}
