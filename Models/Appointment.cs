using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        public int AdopterId { get; set; }  // 👈 تغير من UserId إلى AdopterId

        [Required]
        public int PetId { get; set; }

        public DateTime AppointmentDate { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }

        public string? Reason { get; set; }

        // العلاقات
        [ForeignKey("AdopterId")]
        public virtual Adopter? Adopter { get; set; }  // 👈 العلاقة مع Adopter

        [ForeignKey("PetId")]
        public virtual Pet? Pet { get; set; }

    }
}