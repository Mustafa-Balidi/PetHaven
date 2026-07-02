using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHaven.DTOs;
using PetHaven.Services;
using System.Security.Claims;

namespace PetHaven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlacklistController : ControllerBase
    {
        private readonly IBlacklistService _blacklistService;

        public BlacklistController(IBlacklistService blacklistService)
        {
            _blacklistService = blacklistService;
        }

        // =============================================
        // حظر متبنٍ (للمركز فقط)
        // POST: api/Blacklist/BanAdopter
        // =============================================
        [HttpPost("BanAdopter")]
        [Authorize(Roles = "AdoptionCenter")]
        public async Task<IActionResult> BanAdopter([FromBody] BanAdopterDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { Success = false, Message = "لم يتم التعرف على المستخدم." });

                await _blacklistService.BanAdopterAsync(dto, userId);

                return Ok(new
                {
                    Success = true,
                    Message = "تم حظر المتبني بنجاح."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        // =============================================
        // عرض قائمة المحظورين النشطة للمركز
        // GET: api/Blacklist/CenterBlacklist
        // =============================================
        [HttpGet("CenterBlacklist")]
        [Authorize(Roles = "AdoptionCenter")]
        public async Task<IActionResult> GetCenterBlacklist()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { Success = false, Message = "لم يتم التعرف على المستخدم." });

                var blacklist = await _blacklistService.GetCenterBlacklistAsync(userId);

                return Ok(new
                {
                    Success = true,
                    Data    = blacklist
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        // =============================================
        // رفع الحظر عن متبنٍ (للمركز فقط)
        // PUT: api/Blacklist/UnbanAdopter/{adopterId}
        // =============================================
        [HttpPut("UnbanAdopter/{adopterId}")]
        [Authorize(Roles = "AdoptionCenter")]
        public async Task<IActionResult> UnbanAdopter(int adopterId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { Success = false, Message = "لم يتم التعرف على المستخدم." });

                await _blacklistService.UnbanAdopterAsync(adopterId, userId);

                return Ok(new
                {
                    Success = true,
                    Message = "تم رفع الحظر عن المتبني بنجاح."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}
