using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComillaCentralMedical.Models
{
    public class Service
    {
        [Key]
        public int ServiceID { get; set; }
        [Required(ErrorMessage = "Service name is required.")]
        [MaxLength(100, ErrorMessage = "Service name cannot exceed 100 characters.")]
        [Display(Name = "Service Name")]
        public string ServiceName { get; set; }
        [Required(ErrorMessage = "Unit cost is required.")]
        [Range(0.01, 9999999.99, ErrorMessage = "Unit cost must be a positive number.")]
        [Display(Name = "Unit Cost")]
        public double UnitCost { get; set; }
        [Required(ErrorMessage = "Unit type is required.")]
        [MaxLength(50, ErrorMessage = "Unit type cannot exceed 50 characters.")]
        [Display(Name = "Unit Type")]
        public string UnitType { get; set; }

    }
}