using Microsoft.EntityFrameworkCore;
using PetHaven.Data;
using PetHaven.DTOs;
using PetHaven.Models;

namespace PetHaven.Services
{
    public class AdoptionService : IAdoptionService
    {
        private readonly ApplicationDbContext _context;

        public AdoptionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SubmitRequestAsync(SubmitAdoptionRequestDto dto, string userId)
        {
            // ─── a. Resolve Adopter ────────────────────────────────────────────────
            if (!int.TryParse(userId, out int parsedUserId))
                throw new Exception("معرّف المستخدم غير صالح.");

            var adopter = await _context.Adopters
                .FirstOrDefaultAsync(a => a.UserId == parsedUserId);

            if (adopter == null)
                throw new Exception("لم يتم العثور على ملف المتبني. يرجى التأكد من تسجيل الحساب كمتبنٍ.");



            // ─── b. Update Adopter profile ─────────────────────────────────────────
            adopter.HousingType     = dto.HousingType;
            adopter.HasPetBefore    = dto.HasPetBefore;
            adopter.ExperienceLevel = dto.ExperienceLevel;
            adopter.FreeHoursPerDay = dto.FreeHoursPerDay;

            // ─── c. Calculate Score ────────────────────────────────────────────────
            int score = 0;

            if (dto.HasPetBefore)
                score += 25;

            if (dto.HousingType == "House")
                score += 25;
            else if (dto.HousingType == "Apartment")
                score += 10;

            if (dto.FreeHoursPerDay >= 4)
                score += 20;
            else if (dto.FreeHoursPerDay >= 2)
                score += 15;

            if (dto.ExperienceLevel == "Expert")
                score += 30;
            else if (dto.ExperienceLevel == "Intermediate")
                score += 20;
            else if (dto.ExperienceLevel == "Beginner")
                score += 10;

            // ─── d. Verify Pet exists ──────────────────────────────────────────────

            //var petExists = await _context.Pets.AnyAsync(p => p.PetId == dto.PetId);
            //if (!petExists)
            //    throw new Exception("الحيوان المطلوب غير موجود أو تم اعتماده بالفعل.");
            // --- d. Verify Pet exists ---
            var pet = await _context.Pets.FindAsync(dto.PetId);
            if (pet == null)
                throw new Exception("الحيوان المطلوب غير موجود أو تم اعتماده بالفعل.");
            // 🛑 فحص القائمة السوداء: هل هذا المتبني محظور في مركز هذا الحيوان؟
            var isBlacklisted = await _context.Blacklists
                .AnyAsync(b => b.AdopterId == adopter.AdopterId
                            && b.CenterId == pet.CenterId
                            && b.IsActive);

            if (isBlacklisted)
                throw new UnauthorizedAccessException("عذراً، لا يمكنك إرسال طلب تبني. لقد تم حظرك من قبل هذا المركز.");

            // ─── e. Create AdoptionRequest ─────────────────────────────────────────
            var request = new AdoptionRequest
            {
                AdopterId = adopter.AdopterId,
                PetId     = dto.PetId,
                Status    = "Pending",
                Score     = score,
                CreatedAt = DateTime.UtcNow
            };

            // ─── f. Persist ────────────────────────────────────────────────────────
            _context.AdoptionRequests.Add(request);
            await _context.SaveChangesAsync();

            return true;
        }

        // ═══════════════════════════════════════════════════════════════════════
        // GET: طلبات التبني الخاصة بالمركز (مرتبة تنازلياً حسب Score)
        // ═══════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<AdoptionRequestResponseDto>> GetCenterRequestsAsync(string userId)
        {
            if (!int.TryParse(userId, out int parsedUserId))
                throw new Exception("معرّف المستخدم غير صالح.");

            var center = await _context.AdoptionCenters
                .FirstOrDefaultAsync(c => c.UserId == parsedUserId);

            if (center == null)
                throw new Exception("لم يتم العثور على ملف المركز.");

            var requests = await _context.AdoptionRequests
                .Include(r => r.Pet)
                .Include(r => r.Adopter)
                    .ThenInclude(a => a.User)
                .Where(r => r.Pet.CenterId == center.CenterId)
                .OrderByDescending(r => r.Score)
                .Select(r => new AdoptionRequestResponseDto
                {
                    RequestId   = r.AdoptionRequestId,
                    PetName     = r.Pet.PetName,
                    AdopterName = r.Adopter.User != null ? r.Adopter.User.FullName : "—",
                    Score       = r.Score,
                    Status      = r.Status ?? "Pending",
                    RequestDate = r.CreatedAt
                })
                .ToListAsync();

            return requests;
        }

        // ═══════════════════════════════════════════════════════════════════════
        // PUT: استجابة المركز على طلب تبني (قبول / رفض)
        // ═══════════════════════════════════════════════════════════════════════
        public async Task<bool> RespondToRequestAsync(int requestId, RespondToRequestDto dto, string userId)
        {
            if (!int.TryParse(userId, out int parsedUserId))
                throw new Exception("معرّف المستخدم غير صالح.");

            // تحميل الطلب مع العلاقات المطلوبة للتحقق من الصلاحية
            var request = await _context.AdoptionRequests
                .Include(r => r.Pet)
                    .ThenInclude(p => p.Center)
                .Include(r => r.Adopter)
                .FirstOrDefaultAsync(r => r.AdoptionRequestId == requestId);

            if (request == null)
                throw new Exception("الطلب غير موجود.");

            // تأكد أن هذا الطلب يخص حيواناً تابعاً لمركز المستخدم الحالي
            if (request.Pet?.Center?.UserId != parsedUserId)
                throw new UnauthorizedAccessException("ليس لديك صلاحية للرد على هذا الطلب.");

            // تحديث الحالة والملاحظة
            request.Status     = dto.Status;
            request.CenterNote = dto.CenterNote;

            // إذا تمت الموافقة → تحديث بيانات المتبني
            if (dto.Status == "Approved" && request.Adopter != null)
            {
                request.Adopter.LastReportDate     = DateTime.UtcNow;
                request.Adopter.MissedReportsCount = 0;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
