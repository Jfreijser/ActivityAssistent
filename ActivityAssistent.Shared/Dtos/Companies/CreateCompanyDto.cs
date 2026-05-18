using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ActivityAssistent.Shared.Dtos.Companies
{
    public class CreateCompanyDto
    {
        [Required(ErrorMessage = "Company Name is required.")]
        [StringLength(100, ErrorMessage = "Company Name cannot exceed 100 characters.")]
        public string CompanyName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string EmailAddress { get; set; } = string.Empty;


        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression(@"^[0-9+\s-]{7,20}$", ErrorMessage = "Please enter a valid phone number.")]
        public string PhoneNumber { get; set; } = string.Empty;


        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; } = string.Empty;


        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; } = string.Empty;


    }
}
