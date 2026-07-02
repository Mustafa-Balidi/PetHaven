namespace PetHaven.DTOs
{
    public class BlacklistResponseDto
    {
        public int BlacklistId { get; set; }
        public string AdopterName { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public DateTime BanDate { get; set; }
        public bool IsActive { get; set; }
    }
}
