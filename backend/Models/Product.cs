using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public int CenterId { get; set; }  // 👈 تغير من CategoryId إلى CenterId

        [Required]
        public int CategoryId { get; set; }  // 👈 أضفنا هذا (التصنيف)

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public decimal ProductPrice { get; set; }

        public decimal DiscountRate { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        [MaxLength(500)]
        public string? ImageURL { get; set; }

        // العلاقات
        [ForeignKey("CenterId")]
        public virtual AdoptionCenter? Center { get; set; }  // 👈 العلاقة مع المركز

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        public virtual ICollection<CartItem>? CartItems { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
        public virtual ICollection<Wishlist>? Wishlists { get; set; }
    }
} 