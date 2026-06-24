using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        [Required]
        public int CartId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        // العلاقات
        [ForeignKey("CartId")]
        public virtual Cart? Cart { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}