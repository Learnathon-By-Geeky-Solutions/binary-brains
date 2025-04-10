﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSystem.Models
{
   public class Company
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }

        [DisplayName("Street Address")]
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        [DisplayName("Phone No.")]
        public string? PhoneNumber { get; set; }
    }
}
