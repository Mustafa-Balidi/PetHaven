using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Models
{
    public class Vet
    {
        [Key]
        public int VetId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Specialization { get; set; }

        [MaxLength(500)]
        public string? ClinicName { get; set; }  

        [MaxLength(500)]
        public string? ClinicAddress { get; set; }

        [MaxLength(50)]
        public string? PhoneNumber { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        public int? ExperienceYears { get; set; }  // 👈 تغير من YearsOfExperience

        [MaxLength(50)]
        public string? LicenseNumber { get; set; }

        public decimal? Location_Lat { get; set; }  // 👈 تغير من Latitude
        public decimal? Location_Lng { get; set; }  // 👈 تغير من Longitude

        public bool IsVerified { get; set; }

        public DateTime CreatedAt { get; set; }

        // العلاقات
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}