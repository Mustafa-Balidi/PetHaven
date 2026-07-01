using System.ComponentModel.DataAnnotations;

namespace PetHaven.DTOs
{
    public class UpdatePetDto
    {
        [MaxLength(100)]
        public string? Name { get; set; }

        public int? Age { get; set; }

        [MaxLength(50)]
        public string? HealthStatus { get; set; }

        public string? Description { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }
    }
}
