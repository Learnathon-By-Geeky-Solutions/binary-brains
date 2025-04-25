using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmarTech.Domain.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; } = null!;

        [ValidateNever]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; } = null!;

        [Required(ErrorMessage = "Stock quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Stock quantity must be greater than zero.")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "Discount amount is required.")]
        [Range(1, 100000, ErrorMessage = "Discount must be a positive number and less than 100,000 and more than zero")]
        public decimal DiscountAmount { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
