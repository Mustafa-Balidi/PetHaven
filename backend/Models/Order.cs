using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int UserId { get; set; }

        public DateTime OrderDate { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }  // أو TotalAmount

        [MaxLength(50)]
        public string? Status { get; set; }

        // العلاقات
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public virtual ICollection<OrderItem>? OrderItems { get; set; }

        public virtual Payment? Payment { get; set; }  // 👈 إضافة العلاقة مع Payment
    }
}