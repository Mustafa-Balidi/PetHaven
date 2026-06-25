using System.ComponentModel.DataAnnotations;

namespace PetHaven.Models
{
    public class AppointmentRequest
    {
        [Key]
        public int AppointmentRequestId { get; set; }

        [MaxLength(500)]
        public string? ImageURL { get; set; }

        [MaxLength(50)]
        public string? HealthStatus { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}