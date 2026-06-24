using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Models
{
    public class Pet
    {
        [Key]
        public int PetId { get; set; }

        [Required]
        public int CenterId { get; set; }

        [Required]
        [MaxLength(100)]
        public string PetName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Species { get; set; }

        [MaxLength(50)]
        public string? Breed { get; set; }

        public int? Age { get; set; }

        [MaxLength(20)]
        public string? Gender { get; set; }

        [MaxLength(50)]
        public string? HealthStatus { get; set; }

        public string? Description { get; set; }

        [MaxLength(500)]
        public string? ImageURL { get; set; }

        // العلاقات
        [ForeignKey("CenterId")]
        public virtual AdoptionCenter? Center { get; set; }

        public virtual ICollection<Appointment>? Appointments { get; set; }
        public virtual ICollection<Diagnosis>? Diagnoses { get; set; }
    }
}