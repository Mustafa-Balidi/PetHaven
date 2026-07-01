using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHaven.DTOs;
using PetHaven.Services;

namespace PetHaven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly IPetService _petService;

        public PetsController(IPetService petService)
        {
            _petService = petService;
        }

        // =============================================
        // POST: api/pets/CreateCenterPet
        // إضافة حيوان أليف جديد (مراكز التبني فقط)
        // =============================================
        [HttpPost("CreateCenterPet")]
        [Authorize(Roles = "AdoptionCenter")]
        public async Task<IActionResult> AddPet([FromBody] CreatePetDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { Success = false, Message = "لم يتم التعرف على المستخدم." });

                var result = await _petService.AddPetAsync(dto, userId);

                return Ok(new
                {
                    Success = true,
                    Message = "تم إضافة الحيوان الأليف بنجاح!",
                    Data    = result
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
        // GET: api/pets/AllPets
        // جلب جميع الحيوانات المتاحة للتبني (عام)
        // =============================================
        [HttpGet("AllPets")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAvailablePets()
        {
            try
            {
                var pets = await _petService.GetAllAvailablePetsAsync();

                return Ok(new
                {
                    Success = true,
                    Data    = pets
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
        // GET: api/pets/CenterPets
        // جلب حيوانات المركز المسجّل دخوله (مراكز التبني فقط)
        // =============================================
        [HttpGet("CenterPets")]
        [Authorize(Roles = "AdoptionCenter")]
        public async Task<IActionResult> GetMyPets()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { Success = false, Message = "لم يتم التعرف على المستخدم." });

                var pets = await _petService.GetPetsByCenterAsync(userId);

                return Ok(new
                {
                    Success = true,
                    Data    = pets
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
        // PUT: api/pets/UpdateCenterPet/{id}
        // تعديل بيانات حيوان أليف (مراكز التبني فقط)
        // =============================================
        [HttpPut("UpdateCenterPet/{id}")]
        [Authorize(Roles = "AdoptionCenter")]
        public async Task<IActionResult> UpdatePet(int id, [FromBody] UpdatePetDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { Success = false, Message = "لم يتم التعرف على المستخدم." });

                var result = await _petService.UpdatePetAsync(id, dto, userId);

                return Ok(new
                {
                    Success = true,
                    Message = "تم تعديل بيانات الحيوان الأليف بنجاح!",
                    Data    = result
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new
                {
                    Success = false,
                    Message = ex.Message
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
        // DELETE: api/pets/DeleteCenterPet/{id}
        // حذف حيوان أليف (مراكز التبني فقط)
        // =============================================
        [HttpDelete("DeleteCenterPet/{id}")]
        [Authorize(Roles = "AdoptionCenter")]
        public async Task<IActionResult> DeletePet(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { Success = false, Message = "لم يتم التعرف على المستخدم." });

                await _petService.DeletePetAsync(id, userId);

                return Ok(new
                {
                    Success = true,
                    Message = "تم حذف الحيوان الأليف بنجاح!"
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new
                {
                    Success = false,
                    Message = ex.Message
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
