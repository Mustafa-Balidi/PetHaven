using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHaven.DTOs;
using PetHaven.Services;
using System.Security.Claims;

namespace PetHaven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdoptionController : ControllerBase
    {
        private readonly IAdoptionService _adoptionService;

        public AdoptionController(IAdoptionService adoptionService)
        {
            _adoptionService = adoptionService;
        }

        // =============================================
        // تقديم طلب تبني جديد
        // POST: api/Adoption/SubmitRequest
        // =============================================
        [HttpPost("SubmitRequest")]
        [Authorize(Roles = "Adopter")]
        public async Task<IActionResult> SubmitRequest([FromBody] SubmitAdoptionRequestDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { Success = false, Message = "لم يتم التعرف على المستخدم." });

                await _adoptionService.SubmitRequestAsync(dto, userId);

                return Ok(new
                {
                    Success = true,
                    Message = "تم إرسال طلب التبني بنجاح!"
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
        // عرض طلبات التبني للمركز (مرتبة حسب الأفضلية)
        // GET: api/Adoption/CenterRequests
        // =============================================
        [HttpGet("CenterRequests")]
        [Authorize(Roles = "AdoptionCenter")]
        public async Task<IActionResult> GetCenterRequests()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { Success = false, Message = "لم يتم التعرف على المستخدم." });

                var requests = await _adoptionService.GetCenterRequestsAsync(userId);

                return Ok(new
                {
                    Success = true,
                    Data    = requests
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
        // الرد على طلب تبني (قبول أو رفض)
        // PUT: api/Adoption/Respond/{id}
        // =============================================
        [HttpPut("Respond/{id}")]
        [Authorize(Roles = "AdoptionCenter")]
        public async Task<IActionResult> RespondToRequest(int id, [FromBody] RespondToRequestDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { Success = false, Message = "لم يتم التعرف على المستخدم." });

                await _adoptionService.RespondToRequestAsync(id, dto, userId);

                var message = dto.Status == "Approved"
                    ? "تمت الموافقة على طلب التبني بنجاح!"
                    : "تم رفض طلب التبني.";

                return Ok(new
                {
                    Success = true,
                    Message = message
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
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
