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

            db.Bills.Add(bill);
            db.SaveChanges();

            return Ok(bill);
        }

        // PUT: api/BillApi/5
        [HttpPut]
        public IHttpActionResult UpdateBill(int id, Bill updated)
        {
            var bill = db.Bills.Find(id);
            if (bill == null || bill.IsConfirmed)
                return BadRequest("Cannot edit a confirmed bill.");

            bill.PatientName = updated.PatientName;
            bill.Phone = updated.Phone;
            bill.OverallDiscountRate = updated.OverallDiscountRate;
            bill.TotalAmount = updated.TotalAmount;
            bill.IsReturned = false;
            bill.ReturnReason = null;

            db.SaveChanges();
            return Ok("Bill updated.");
        }

        // DELETE: api/BillApi/5
        [HttpDelete]
        public IHttpActionResult DeleteBill(int id)
        {
            var bill = db.Bills.Find(id);
            if (bill == null || bill.IsConfirmed)
                return BadRequest("Cannot delete confirmed bill.");

            db.Bills.Remove(bill);
            db.SaveChanges();
            return Ok("Bill deleted.");
        }
    }
}
