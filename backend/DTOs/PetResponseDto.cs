namespace PetHaven.DTOs
{
    public class PetResponseDto
    {
        public int PetId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Species { get; set; }
        public string? Breed { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? Description { get; set; }
        public string? HealthStatus { get; set; }
        public string? ImageUrl { get; set; }
        public string CenterName { get; set; } = string.Empty;
    }
}
