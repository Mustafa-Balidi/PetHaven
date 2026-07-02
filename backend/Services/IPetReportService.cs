using PetHaven.DTOs;

namespace PetHaven.Services
{
    public interface IPetReportService
    {
        Task<PetReportResponseDto> SubmitReportAsync(CreatePetReportDto dto, string userId);
        Task<IEnumerable<PetReportResponseDto>> GetCenterReportsAsync(string userId);
    }
}
