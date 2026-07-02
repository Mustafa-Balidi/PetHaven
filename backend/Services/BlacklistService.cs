using Microsoft.EntityFrameworkCore;
using PetHaven.Data;
using PetHaven.DTOs;
using PetHaven.Models;

namespace PetHaven.Services
{
    public class BlacklistService : IBlacklistService
    {
        private readonly ApplicationDbContext _context;

        public BlacklistService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ═══════════════════════════════════════════════════════════════════════
        // POST: حظر متبنٍ من قِبَل مركز التبني
        // ═══════════════════════════════════════════════════════════════════════
        public async Task<bool> BanAdopterAsync(BanAdopterDto dto, string centerUserId)
        {
            // ─── 1. Parse centerUserId ──────────────────────────────────────────
            if (!int.TryParse(centerUserId, out int parsedUserId))
                throw new Exception("معرّف المستخدم غير صالح.");

            // ─── 2. Find AdoptionCenter ─────────────────────────────────────────
            var center = await _context.AdoptionCenters
                .FirstOrDefaultAsync(c => c.UserId == parsedUserId);

            if (center == null)
                throw new Exception("لم يتم العثور على ملف المركز.");

            // ─── 3. Check for existing active ban ───────────────────────────────
            var existingBan = await _context.Blacklists
                .AnyAsync(b => b.CenterId  == center.CenterId
                            && b.AdopterId == dto.AdopterId
                            && b.IsActive  == true);

            if (existingBan)
                throw new Exception("هذا المتبني محظور بالفعل لدى مركزكم.");

            // ─── 4. Create Blacklist entry ──────────────────────────────────────
            var ban = new Blacklist
            {
                CenterId   = center.CenterId,
                AdopterId  = dto.AdopterId,
                Reason     = dto.Reason,
                BlockedAt  = DateTime.UtcNow,
                IsActive   = true
            };

            _context.Blacklists.Add(ban);
            await _context.SaveChangesAsync();

            return true;
        }

        // ═══════════════════════════════════════════════════════════════════════
        // GET: قائمة المحظورين النشطة لمركز التبني
        // ═══════════════════════════════════════════════════════════════════════
        public async Task<IEnumerable<BlacklistResponseDto>> GetCenterBlacklistAsync(string centerUserId)
        {
            // ─── 1. Parse centerUserId ──────────────────────────────────────────
            if (!int.TryParse(centerUserId, out int parsedUserId))
                throw new Exception("معرّف المستخدم غير صالح.");

            // ─── 2. Find AdoptionCenter ─────────────────────────────────────────
            var center = await _context.AdoptionCenters
                .FirstOrDefaultAsync(c => c.UserId == parsedUserId);

            if (center == null)
                throw new Exception("لم يتم العثور على ملف المركز.");

            // ─── 3. Fetch active blacklist entries with Adopter.User ────────────
            var blacklist = await _context.Blacklists
                .Include(b => b.Adopter)
                    .ThenInclude(a => a!.User)
                .Where(b => b.CenterId == center.CenterId && b.IsActive == true)
                .OrderByDescending(b => b.BlockedAt)
                .Select(b => new BlacklistResponseDto
                {
                    BlacklistId = b.BlacklistId,
                    AdopterName = b.Adopter != null && b.Adopter.User != null
                                    ? b.Adopter.User.FullName
                                    : "—",
                    Reason      = b.Reason ?? string.Empty,
                    BanDate     = b.BlockedAt,
                    IsActive    = b.IsActive
                })
                .ToListAsync();

            return blacklist;
        }

        // ═══════════════════════════════════════════════════════════════════════
        // PUT: رفع الحظر عن متبنٍ
        // ═══════════════════════════════════════════════════════════════════════
        public async Task<bool> UnbanAdopterAsync(int adopterId, string centerUserId)
        {
            // ─── 1. Parse centerUserId ──────────────────────────────────────────
            if (!int.TryParse(centerUserId, out int parsedUserId))
                throw new Exception("معرّف المستخدم غير صالح.");

            // ─── 2. Find AdoptionCenter ─────────────────────────────────────────
            var center = await _context.AdoptionCenters
                .FirstOrDefaultAsync(c => c.UserId == parsedUserId);

            if (center == null)
                throw new Exception("لم يتم العثور على ملف المركز.");

            // ─── 3. Find the active ban record ──────────────────────────────────
            var ban = await _context.Blacklists
                .FirstOrDefaultAsync(b => b.CenterId  == center.CenterId
                                       && b.AdopterId == adopterId
                                       && b.IsActive  == true);

            if (ban == null)
                throw new Exception("لا يوجد حظر نشط لهذا المتبني في مركزكم.");

            // ─── 4. Deactivate the ban ──────────────────────────────────────────
            ban.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
