using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Models
{
    public class AdoptionCenter
    {
        [Key]
        public int CenterId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(200)]
        public string CenterName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Address { get; set; }

        [MaxLength(200)]
        public string? ContactInfo { get; set; }

        // العلاقات
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        //    المركز لديه عدة منتجات   
        public virtual ICollection<Product>? Products { get; set; }

        public virtual ICollection<Pet>? Pets { get; set; }
        public virtual ICollection<Blacklist>? Blacklists { get; set; }
    }
}