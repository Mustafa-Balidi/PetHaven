namespace PetHaven.DTOs
{
    public class CreatePetReportDto
    {
        public int AdoptionRequestId { get; set; }
        public string? ImageUrl { get; set; }
        public string? HealthStatus { get; set; }
        public string? Notes { get; set; }
    }
}
