using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        public int OrderId { get; set; }  // 👈 هذا هو المفتاح الخارجي

        [Required]
        public decimal Amount { get; set; }

        [MaxLength(50)]
        public string? PaymentMethod { get; set; }

        [MaxLength(50)]
        public string? PaymentStatus { get; set; }

        public DateTime PaymentDate { get; set; }

        // العلاقات
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }  // 👈 العلاقة مع Order
    
    }
}