using System.ComponentModel.DataAnnotations;

namespace PetHaven.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        public string? Description { get; set; }

        [MaxLength(500)]
        public string? ImageURL { get; set; }


        // العلاقة: Category لديه عدة Products
        public virtual ICollection<Product>? Products { get; set; }
    }
}