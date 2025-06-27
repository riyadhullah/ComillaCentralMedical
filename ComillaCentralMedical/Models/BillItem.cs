using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComillaCentralMedical.Models
{
    public class BillItem
    {
        [Key]
        public int BillItemID { get; set; }

        [Required]
        [ForeignKey("Bill")]
        public int BillID { get; set; }
        public Bill Bill { get; set; }

        [Required]
        [ForeignKey("Service")]
        public int ServiceID { get; set; }
        public Service Service { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000.")]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Unit Price")]
        [Range(0.01, 999999.99, ErrorMessage = "Unit price must be a positive number.")]
        public double UnitPrice { get; set; }

        [Display(Name = "Discount Rate (%)")]
        [Range(0, 100)]
        public double DiscountRate { get; set; }

        [Display(Name = "Total Price")]
        [Range(0, double.MaxValue, ErrorMessage = "Total price must be positive.")]
        public double TotalPrice { get; set; }
    }
}
