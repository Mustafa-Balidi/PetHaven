namespace PetHaven.DTOs
{
    public class PetReportResponseDto
    {
        public int ReportId { get; set; }
        public int AdoptionRequestId { get; set; }
        public string PetName { get; set; } = string.Empty;
        public string AdopterName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? HealthStatus { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
