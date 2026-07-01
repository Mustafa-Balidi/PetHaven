namespace PetHaven.DTOs
{
    public class RespondToRequestDto
    {
        /// <summary>Expected values: "Approved" | "Rejected"</summary>
        public string Status { get; set; } = string.Empty;

        public string? CenterNote { get; set; }
    }
}
