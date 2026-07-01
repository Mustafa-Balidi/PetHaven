using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Models
{
    public class Adopter
    {
        [Key]
        public int AdopterId { get; set; }

        [Required]
        public int UserId { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        [MaxLength(50)]
        public string? HousingType { get; set; }  // 👈 جديد (نوع السكن)

        public bool HasPetBefore { get; set; }

        [MaxLength(50)]
        public string? ExperienceLevel { get; set; }

        public int MissedReportsCount { get; set; }

        public int FreeHoursPerDay { get; set; }

        public DateTime? LastReportDate { get; set; }

        // 👈 جديد: الرصيد
        [Required]
        public decimal Balance { get; set; }

        // العلاقات
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        // 👈 جديد: Adopter لديه عدة Appointments
        public virtual ICollection<Appointment>? Appointments { get; set; }

        public virtual ICollection<Blacklist>? Blacklists { get; set; }
        public virtual ICollection<PetReport>? PetReports { get; set; }

    }
}