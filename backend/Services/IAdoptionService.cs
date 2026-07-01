using PetHaven.DTOs;

namespace PetHaven.Services
{
    public interface IAdoptionService
    {
        Task<bool> SubmitRequestAsync(SubmitAdoptionRequestDto dto, string userId);

        Task<IEnumerable<AdoptionRequestResponseDto>> GetCenterRequestsAsync(string userId);

        Task<bool> RespondToRequestAsync(int requestId, RespondToRequestDto dto, string userId);
    }
}
