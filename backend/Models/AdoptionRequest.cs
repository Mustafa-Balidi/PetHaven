using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Models
{
    public class AdoptionRequest
    {
        [Key]
        public int AdoptionRequestId { get; set; }

        [Required]
        public int AdopterId { get; set; }

        [Required]
        public int PetId { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public int Score { get; set; }  // لحساب الاولوية 

        public string? CenterNote { get; set; }  // عندما يرفض المركز طلباً، يمكنه كتابة سبب الرفض هنا (مثلاً: "نعتذر، ساعات تفرغك لا تناسب هذا النوع من الكلاب")

        // Navigation properties
        [ForeignKey("AdopterId")]
        public virtual Adopter Adopter { get; set; } = null!;

        [ForeignKey("PetId")]
        public virtual Pet Pet { get; set; } = null!;

        public virtual ICollection<PetReport> PetReports { get; set; } = new List<PetReport>();
    }
}
