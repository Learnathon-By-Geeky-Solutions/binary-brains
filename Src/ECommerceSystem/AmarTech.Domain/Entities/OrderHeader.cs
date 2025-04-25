using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AmarTech.Domain.Entities
{
    public class OrderHeader
    {
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = null!;

        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser? ApplicationUser { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public double OrderTotal { get; set; }

        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }

        public DateTime PaymentDate { get; set; }
        public DateOnly PaymentDueDate { get; set; }

        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }

        [Required]

        [DisplayName("Phone No*")]

        public string PhoneNumber { get; set; } = null!;

        [Required]
        [DisplayName("Street Address*")]
        public string StreetAddress { get; set; } = null!;

        [Required]
        [DisplayName("City*")]
        public string City { get; set; } = null!;

        [Required]
        [DisplayName("State*")]
        public string State { get; set; } = null!;

        [Required]
        [DisplayName("Postal Code*")]
        public string PostalCode { get; set; } = null!;

        [Required]
        [DisplayName("Name*")]

        public string Name { get; set; } = null!;
    }
}
