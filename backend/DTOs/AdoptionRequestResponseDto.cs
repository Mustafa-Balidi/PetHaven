namespace PetHaven.DTOs
{
    public class AdoptionRequestResponseDto
    {
        public int RequestId { get; set; }
        public string PetName { get; set; } = string.Empty;
        public string AdopterName { get; set; } = string.Empty;
        public int Score { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
    }
}
