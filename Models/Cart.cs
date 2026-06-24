using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        [Required]
        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        // العلاقات
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public virtual ICollection<CartItem>? CartItems { get; set; }
    }
}