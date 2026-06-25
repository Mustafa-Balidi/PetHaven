using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Models
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string TargetType { get; set; } = string.Empty; // "Product" or "Vet"

        [Required]
        public int TargetId { get; set; }

        [Required]
        [Range(1, 5)]
        public int StarsCount { get; set; }

        public string? ReviewText { get; set; }

        public DateTime CreatedAt { get; set; }

        // العلاقات
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}