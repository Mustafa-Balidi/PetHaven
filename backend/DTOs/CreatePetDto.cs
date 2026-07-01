using System.ComponentModel.DataAnnotations;

namespace PetHaven.DTOs
{
    public class CreatePetDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Species { get; set; }

        [MaxLength(50)]
        public string? Breed { get; set; }

        public int? Age { get; set; }

        [MaxLength(20)]
        public string? Gender { get; set; }

        public string? Description { get; set; }

        [MaxLength(50)]
        public string? HealthStatus { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }
    }
}
