using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComillaCentralMedical.Models
{
    public class AccountantSummary
    {
        [Key]
        public int ID { get; set; }

        public int ConfirmedTodayCount { get; set; }

        public decimal TotalIncomeToday { get; set; }

        public decimal TotalIncomeThisMonth { get; set; }

        public List<Bill> Bills { get; set; }
    }
}
