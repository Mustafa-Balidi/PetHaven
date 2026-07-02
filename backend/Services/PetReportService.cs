using Microsoft.EntityFrameworkCore;
using PetHaven.Data;
using PetHaven.DTOs;
using PetHaven.Models;

namespace PetHaven.Services
{
    public class PetReportService : IPetReportService
    {
        private readonly ApplicationDbContext _context;

        public PetReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ═══════════════════════════════════════════════════════════════════════
        // POST: تقديم تقرير حيوان من قِبَل المتبني
        // ═══════════════════════════════════════════════════════════════════════
        public async Task<PetReportResponseDto> SubmitReportAsync(CreatePetReportDto dto, string userId)
        {
            // ─── 1. Parse userId ────────────────────────────────────────────────
            if (!int.TryParse(userId, out int parsedUserId))
                throw new Exception("معرّف المستخدم غير صالح.");

            // ─── 2. Find Adopter ────────────────────────────────────────────────
            var adopter = await _context.Adopters
                .FirstOrDefaultAsync(a => a.UserId == parsedUserId);

            if (adopter == null)
                throw new Exception("لم يتم العثور على ملف المتبني.");

            // ─── 3. Find AdoptionRequest (with Pet and Adopter.User) ────────────
            var request = await _context.AdoptionRequests
                .Include(r => r.Pet)
                .Include(r => r.Adopter)
                    .ThenInclude(a => a.User)
                .FirstOrDefaultAsync(r => r.AdoptionRequestId == dto.AdoptionRequestId);

            if (request == null)
                throw new Exception("طلب التبني غير موجود.");

            // ─── 4. Verify ownership ────────────────────────────────────────────
            if (request.AdopterId != adopter.AdopterId)
                throw new UnauthorizedAccessException("ليس لديك صلاحية لتقديم تقرير عن هذا الطلب.");

            // ─── 5. Verify request is Approved ─────────────────────────────────
            if (request.Status != "Approved")
                throw new Exception("لا يمكن تقديم تقرير إلا بعد الموافقة على طلب التبني.");

            // ─── 6. Create PetReport entity ─────────────────────────────────────
            var report = new PetReport
            {
                AdoptionRequestId = dto.AdoptionRequestId,
                ImageURL          = dto.ImageUrl,
                HealthStatus      = dto.HealthStatus,
                Notes             = dto.Notes,
                CreatedAt         = DateTime.UtcNow
            };

            _context.PetReports.Add(report);

            // ─── 7. Update Adopter tracking fields ──────────────────────────────
            adopter.LastReportDate     = DateTime.UtcNow;
            adopter.MissedReportsCount = 0;

            // ─── 8. Save and return DTO ─────────────────────────────────────────
            await _context.SaveChangesAsync();

            return new PetReportResponseDto
            {
                ReportId          = report.ReportId,
                AdoptionRequestId = report.AdoptionRequestId,
                PetName           = request.Pet?.PetName ?? "—",
                AdopterName       = request.Adopter?.User?.FullName ?? "—",
                ImageUrl          = report.ImageURL,
                HealthStatus      = report.HealthStatus,
                Notes             = report.Notes,
                CreatedAt         = report.CreatedAt
            };
        }

        // ═══════════════════════════════════════════════════════════════════════
        // GET: جميع تقارير حيوانات المركز
        // ═══════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<PetReportResponseDto>> GetCenterReportsAsync(string userId)
        {
            // ─── 1. Parse userId ────────────────────────────────────────────────
            if (!int.TryParse(userId, out int parsedUserId))
                throw new Exception("معرّف المستخدم غير صالح.");

            // ─── 2. Find AdoptionCenter ─────────────────────────────────────────
            var center = await _context.AdoptionCenters
                .FirstOrDefaultAsync(c => c.UserId == parsedUserId);

            if (center == null)
                throw new Exception("لم يتم العثور على ملف المركز.");

            // ─── 3. Fetch PetReports for this center ────────────────────────────
            var reports = await _context.PetReports
                .Include(pr => pr.AdoptionRequest)
                    .ThenInclude(ar => ar.Pet)
                .Include(pr => pr.AdoptionRequest)
                    .ThenInclude(ar => ar.Adopter)
                        .ThenInclude(a => a.User)
                .Where(pr => pr.AdoptionRequest.Pet.CenterId == center.CenterId)
                .OrderByDescending(pr => pr.CreatedAt)
                .Select(pr => new PetReportResponseDto
                {
                    ReportId          = pr.ReportId,
                    AdoptionRequestId = pr.AdoptionRequestId,
                    PetName           = pr.AdoptionRequest.Pet.PetName,
                    AdopterName       = pr.AdoptionRequest.Adopter.User != null
                                            ? pr.AdoptionRequest.Adopter.User.FullName
                                            : "—",
                    ImageUrl          = pr.ImageURL,
                    HealthStatus      = pr.HealthStatus,
                    Notes             = pr.Notes,
                    CreatedAt         = pr.CreatedAt
                })
                .ToListAsync();

            return reports;
        }
    }
}
