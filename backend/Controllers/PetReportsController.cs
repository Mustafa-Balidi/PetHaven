using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHaven.DTOs;
using PetHaven.Services;
using System.Security.Claims;

namespace PetHaven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetReportsController : ControllerBase
    {
        private readonly IPetReportService _petReportService;

        public PetReportsController(IPetReportService petReportService)
        {
            _petReportService = petReportService;
        }

        // =============================================
        // تقديم تقرير حيوان (للمتبني فقط)
        // POST: api/PetReports/SubmitReport
        // =============================================
        [HttpPost("SubmitReport")]
        [Authorize(Roles = "Adopter")]
        public async Task<IActionResult> SubmitReport([FromBody] CreatePetReportDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { Success = false, Message = "لم يتم التعرف على المستخدم." });

                var report = await _petReportService.SubmitReportAsync(dto, userId);

                return Ok(new
                {
                    Success = true,
                    Message = "تم تقديم التقرير بنجاح!",
                    Data    = report
                });
            }
            catch (UnauthorizedAccessException)
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

        // =============================================
        // عرض تقارير الحيوانات الخاصة بالمركز
        // GET: api/PetReports/CenterReports
        // =============================================
        [HttpGet("CenterReports")]
        [Authorize(Roles = "AdoptionCenter")]
        public async Task<IActionResult> GetCenterReports()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { Success = false, Message = "لم يتم التعرف على المستخدم." });

                var reports = await _petReportService.GetCenterReportsAsync(userId);

                return Ok(new
                {
                    Success = true,
                    Data    = reports
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
