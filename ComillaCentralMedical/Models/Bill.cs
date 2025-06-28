using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComillaCentralMedical.Models
{
    public class Bill
    {
        [Key]
        public int BillID { get; set; }

        [Required(ErrorMessage = "Patient name is required.")]
        [StringLength(100, ErrorMessage = "Patient name cannot exceed 100 characters.")]
        [Display(Name = "Patient Name")]
        public string PatientName { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^01\d{9}$", ErrorMessage = "Phone number must start with '01' and be exactly 11 digits.")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Created By (Receptionist)")]
        [StringLength(100)]
        public string CreatedBy { get; set; }

        [Display(Name = "Confirmed By (Accountant)")]
        [StringLength(100)]
        public string ConfirmedBy { get; set; }

        [Display(Name = "Is Confirmed")]
        public bool IsConfirmed { get; set; } = false;
        [Display(Name = "Confirm At")]
        public DateTime? ConfirmedAt { get; set; }

        [Display(Name = "Is Returned")]
        public bool IsReturned { get; set; } = false;

        [StringLength(255)]
        [Display(Name = "Return Reason")]
        public string ReturnReason { get; set; }

        [Display(Name = "Returned At")]
        public DateTime? ReturnedAt { get; set; }

        [Display(Name = "Overall Discount (%)")]
        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
        public double? OverallDiscountRate { get; set; }

        [Display(Name = "Total Amount (after discount)")]
        [Range(0, double.MaxValue, ErrorMessage = "Total amount must be positive.")]
        public double? TotalAmount { get; set; }

        public virtual ICollection<BillItem> BillItems { get; set; }
    }
}
