using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? PhoneNumber { get; set; }

        // العلاقات
        [ForeignKey("RoleId")]
        public virtual Role? Role { get; set; }

        public virtual ICollection<Notification>? Notifications { get; set; }
        public virtual ICollection<Rating>? Ratings { get; set; }
        public virtual ICollection<Cart>? Carts { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<Wishlist>? Wishlists { get; set; }
        public virtual ICollection<Diagnosis>? Diagnoses { get; set; }
      //  public virtual ICollection<Payment>? Payments { get; set; }
      //  public virtual ICollection<Appointment>? Appointments { get; set; }
        public virtual Adopter? Adopter { get; set; }
        public virtual AdoptionCenter? AdoptionCenter { get; set; }
        public virtual Vet? Vet { get; set; }
    }
}