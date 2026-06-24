using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Models
{
    public class Diagnosis
    {
        [Key]
        public int DiagnosisId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int PetId { get; set; }

        public string? Symptoms { get; set; }

        public string? Result { get; set; }

        public DateTime CreatedAt { get; set; }

        // العلاقات
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [ForeignKey("PetId")]
        public virtual Pet? Pet { get; set; }
    }
}