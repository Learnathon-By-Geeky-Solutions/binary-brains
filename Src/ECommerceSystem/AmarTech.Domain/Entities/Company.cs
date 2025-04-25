using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmarTech.Domain.Entities
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Company name is required.")]
        [DisplayName("Company Name*")]
        public required string Name { get; set; }

        [DisplayName("Street Address*")]
        [Required(ErrorMessage = "Street Address is required.")]
        public string? StreetAddress { get; set; }
        

        [Required(ErrorMessage = "City is required.")]
        [DisplayName("City*")]
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9\s]*$", ErrorMessage = "Must start with a letter and contain only letters, numbers, and spaces.")]


        public string? City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [DisplayName("State*")]
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9\s]*$", ErrorMessage = "Must start with a letter and contain only letters, numbers, and spaces.")]

        public string? State { get; set; }

        [Required(ErrorMessage = "Postal Code is required.")]
        [DisplayName("Postal Code*")]
        [RegularExpression(@"^\d{4,10}$", ErrorMessage = "Postal Code must be 4-10 digits.")]
        public string? PostalCode { get; set; }

        [DisplayName("Phone No.*")]
        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression(@"^(013|014|015|016|017|018|019)\d{8}$", ErrorMessage = "Enter a valid Bangladeshi phone number.")]
        public string? PhoneNumber { get; set; }
    }
}
