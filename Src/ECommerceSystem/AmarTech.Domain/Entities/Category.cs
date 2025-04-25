using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AmarTech.Domain.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Category Name Cann't be empty")]
        [DisplayName("Category Name*")]
        [MaxLength(30)]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Display Order Must Be Required.")]
        [DisplayName("Display Order*")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1-100")]
        public int DsiplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
        [Required(ErrorMessage = "Created Name Must Be Required")]
        [DisplayName("Created By*")]
        public required string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [DisplayName("Updated By")]
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
