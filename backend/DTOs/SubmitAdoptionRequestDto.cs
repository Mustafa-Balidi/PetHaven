namespace PetHaven.DTOs
{
    public class SubmitAdoptionRequestDto
    {
        public int PetId { get; set; }
        public string HousingType { get; set; } = string.Empty;
        public bool HasPetBefore { get; set; }
        public string ExperienceLevel { get; set; } = string.Empty;
        public int FreeHoursPerDay { get; set; }
    }
}
