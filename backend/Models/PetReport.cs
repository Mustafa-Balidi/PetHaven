using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Models
{
    public class PetReport
    {
        [Key]
        public int ReportId { get; set; }

        [Required]
        public int AdoptionRequestId { get; set; }

        [MaxLength(500)]
        public string? ImageURL { get; set; }

        [MaxLength(50)]
        public string? HealthStatus { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        // العلاقات
        [ForeignKey("AdoptionRequestId")]
        public virtual Adopter? Adopter { get; set; }
    }
}