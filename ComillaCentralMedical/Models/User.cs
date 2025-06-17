using System;
using System.ComponentModel.DataAnnotations;

namespace ComillaCentralMedical.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(50, ErrorMessage = "Full Name cannot exceed 50 characters")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(11, ErrorMessage = "Phone number cannot exceed 11 digits")]
        [RegularExpression(@"^01\d{9}$", ErrorMessage = "Phone number must start with '01' and be exactly 11 digits.")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$", ErrorMessage = "Password must be at least 6 characters long and contain an uppercase letter, a lowercase letter, a digit, and a special character.")]
        public string Password { get; set; }

        [Display(Name = "User Role")]
        [Required(ErrorMessage = "Role is required")]
        [RegularExpression(@"^(Admin|Receptionist|Accountant)$", ErrorMessage = "Role must be 'Admin', 'Receptionist', or 'Accountant'.")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [MinimumAge(18, ErrorMessage = "User must be an adult (at least 18 years old).")]
        public DateTime DOB { get; set; }

        
        [DataType(DataType.Date)]
        [Display(Name = "Joining Date")]
        public DateTime JoinDate { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [RegularExpression(@"^(Male|Female)$", ErrorMessage = "Gender must be Male, Female")]
        public string Gender { get; set; }

        
        [StringLength(255)]
        public string ImagePath { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(255)]
        public string Address { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;
    }
}
