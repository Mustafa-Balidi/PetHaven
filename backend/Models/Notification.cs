using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Type { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }

        // العلاقات
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}