using Microsoft.AspNetCore.Mvc;
using PetHaven.DTOs;
using PetHaven.Services;

namespace PetHaven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // =============================================
        // تسجيل مستخدم جديد (جميع الأدوار)
        // POST: api/auth/register
        // =============================================
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var result = await _authService.RegisterAsync(registerDto);
                return Ok(new
                {
                    Success = true,
                    Message = "تم تسجيل المستخدم بنجاح!",
                    Data = result
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
        // تسجيل الدخول (لجميع الأدوار)
        // POST: api/auth/login
        // =============================================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var result = await _authService.LoginAsync(loginDto);
                return Ok(new
                {
                    Success = true,
                    Message = "تم تسجيل الدخول بنجاح!",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}