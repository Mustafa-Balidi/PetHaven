using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Models
{
    public class Blacklist
    {
        [Key]
        public int BlacklistId { get; set; }

        [Required]
        public int AdopterId { get; set; }

        [Required]
        public int CenterId { get; set; }

        [MaxLength(500)]
        public string? Reason { get; set; }

        public DateTime BlockedAt { get; set; }

        public bool IsActive { get; set; }

        // العلاقات
        [ForeignKey("AdopterId")]
        public virtual Adopter? Adopter { get; set; }

        [ForeignKey("CenterId")]
        public virtual AdoptionCenter? Center { get; set; }
    }
}