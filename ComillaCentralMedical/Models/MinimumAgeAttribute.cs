using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComillaCentralMedical.Models
{
    public class MinimumAgeAttribute : ValidationAttribute
    {
        private readonly int minimunAge;

        public MinimumAgeAttribute(int minimunAge)
        {
            this.minimunAge = minimunAge;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;

            DateTime dob = Convert.ToDateTime(value);
            DateTime today = DateTime.Today;
            int age = (int)((today - dob).TotalDays / 365.25);

            return age >= minimunAge;

 
        }
    }
}