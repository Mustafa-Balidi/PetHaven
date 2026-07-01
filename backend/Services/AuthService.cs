using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PetHaven.Data;
using PetHaven.DTOs;
using PetHaven.Helpers;
using PetHaven.Models;

namespace PetHaven.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtHelper _jwtHelper;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, JwtHelper jwtHelper, IConfiguration configuration)
        {
            _context = context;
            _jwtHelper = jwtHelper;
            _configuration = configuration;
        }

        // =============================================
        // تسجيل مستخدم جديد (جميع الأدوار)
        // =============================================
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            // 1. التحقق من وجود البريد الإلكتروني
            var emailExists = await _context.Users
                .AnyAsync(u => u.Email == dto.Email);

            if (emailExists)
                throw new Exception("البريد الإلكتروني مستخدم بالفعل!");

            // 2. التحقق من وجود اسم المستخدم
            var userNameExists = await _context.Users
                .AnyAsync(u => u.UserName == dto.UserName);

            if (userNameExists)
                throw new Exception("اسم المستخدم مستخدم بالفعل!");

            // 3. تعيين دور واجهة المستخدم إلى دور قاعدة البيانات
            var mappedRole = dto.Role.Trim() switch
            {
                "Pet Owner"       => "Adopter",
                "Veterinarian"    => "Vet",
                "Adoption Center" => "AdoptionCenter",
                _                 => throw new Exception($"الدور '{dto.Role}' غير مدعوم. الأدوار المقبولة: Pet Owner, Veterinarian, Adoption Center.")
            };

            // 4. جلب الدور من قاعدة البيانات
            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleName == mappedRole);

            if (role == null)
                throw new Exception($"الدور '{mappedRole}' غير موجود في قاعدة البيانات! أضف الأدوار أولاً.");

            // 5. تشفير كلمة المرور باستخدام BCrypt
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // 6. إنشاء المستخدم الأساسي
            var user = new User
            {
                FullName    = dto.FullName,
                UserName    = dto.UserName,
                PhoneNumber = dto.PhoneNumber,
                Email       = dto.Email,
                Password    = hashedPassword,
                RoleId      = role.RoleId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // 7. إنشاء الكيان الفرعي بناءً على الدور
            switch (mappedRole)
            {
                case "Adopter":
                    var adopter = new Adopter
                    {
                        UserId             = user.UserId,
                        Address            = null,
                        HousingType        = null,
                        HasPetBefore       = false,
                        ExperienceLevel    = null,
                        MissedReportsCount = 0,
                        LastReportDate     = null,
                        Balance            = 0
                    };
                    _context.Adopters.Add(adopter);

                    var cart = new Cart
                    {
                        UserId    = user.UserId,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.Carts.Add(cart);
                    break;

                case "AdoptionCenter":
                    var center = new AdoptionCenter
                    {
                        UserId     = user.UserId,
                        CenterName = dto.FullName,
                        Address    = null,
                        ContactInfo = null
                    };
                    _context.AdoptionCenters.Add(center);
                    break;

                case "Vet":
                    var vet = new Vet
                    {
                        UserId     = user.UserId,
                        FullName   = dto.FullName,
                        IsVerified = false,
                        CreatedAt  = DateTime.UtcNow
                    };
                    _context.Vets.Add(vet);
                    break;
            }

            await _context.SaveChangesAsync();

            // 8. قراءة مدة انتهاء الصلاحية من appsettings.json وإنشاء JWT
            var expiryInMinutes = _configuration.GetValue<int>("Jwt:ExpiryInMinutes", 20);
            var token = _jwtHelper.GenerateToken(user, role.RoleName);

            // 9. إرجاع الرد
            return new AuthResponseDto
            {
                Token        = token,
                RefreshToken = _jwtHelper.GenerateRefreshToken(),
                ExpiresAt    = DateTime.UtcNow.AddMinutes(expiryInMinutes),
                User = new UserDto
                {
                    UserId      = user.UserId,
                    UserName    = user.UserName,
                    FullName    = user.FullName,
                    Email       = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role        = role.RoleName
                }
            };
        }

        // =============================================
        // تسجيل الدخول (لجميع الأدوار)
        // =============================================
        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            // 1. البحث عن المستخدم بالبريد الإلكتروني
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
                throw new Exception("البريد الإلكتروني أو كلمة المرور غير صحيحة!");

            // 2. التحقق من كلمة المرور باستخدام BCrypt
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);

            if (!isPasswordValid)
                throw new Exception("البريد الإلكتروني أو كلمة المرور غير صحيحة!");

            // 3. قراءة مدة انتهاء الصلاحية من appsettings.json وإنشاء JWT
            var expiryInMinutes = _configuration.GetValue<int>("Jwt:ExpiryInMinutes", 20);
            var token = _jwtHelper.GenerateToken(user, user.Role?.RoleName ?? "User");

            // 4. إرجاع الرد
            return new AuthResponseDto
            {
                Token        = token,
                RefreshToken = _jwtHelper.GenerateRefreshToken(),
                ExpiresAt    = DateTime.UtcNow.AddMinutes(expiryInMinutes),
                User = new UserDto
                {
                    UserId      = user.UserId,
                    UserName    = user.UserName,
                    FullName    = user.FullName,
                    Email       = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role        = user.Role?.RoleName ?? "User"
                }
            };
        }
    }
}