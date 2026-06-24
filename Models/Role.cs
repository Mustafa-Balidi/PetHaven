using System.ComponentModel.DataAnnotations;

namespace PetHaven.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(100)]
        public string RoleName { get; set; } = string.Empty;

        // العلاقة: Role لديه عدة Users
        public virtual ICollection<User>? Users { get; set; }
    }
}