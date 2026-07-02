using PetHaven.DTOs;

namespace PetHaven.Services
{
    public interface IBlacklistService
    {
        Task<bool> BanAdopterAsync(BanAdopterDto dto, string centerUserId);
        Task<IEnumerable<BlacklistResponseDto>> GetCenterBlacklistAsync(string centerUserId);
        Task<bool> UnbanAdopterAsync(int adopterId, string centerUserId);
    }
}
