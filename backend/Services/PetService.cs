using Microsoft.EntityFrameworkCore;
using PetHaven.Data;
using PetHaven.DTOs;
using PetHaven.Models;

namespace PetHaven.Services
{
    public class PetService : IPetService
    {
        private readonly ApplicationDbContext _context;

        public PetService(ApplicationDbContext context)
        {
            _context = context;
        }

        // =============================================
        // Add a pet linked to the logged-in center
        // =============================================
        public async Task<PetResponseDto> AddPetAsync(CreatePetDto dto, string userId)
        {
            // 1. Parse the userId claim (stored as int in the database)
            if (!int.TryParse(userId, out var parsedUserId))
                throw new Exception("معرّف المستخدم غير صالح.");

            // 2. Find the AdoptionCenter that belongs to this user
            var center = await _context.AdoptionCenters
                .FirstOrDefaultAsync(c => c.UserId == parsedUserId);

            if (center == null)
                throw new Exception("لم يتم العثور على مركز تبني مرتبط بهذا الحساب.");

            // 3. Create and persist the Pet entity
            var pet = new Pet
            {
                CenterId     = center.CenterId,
                PetName      = dto.Name,
                Species      = dto.Species,
                Breed        = dto.Breed,
                Age          = dto.Age,
                Gender       = dto.Gender,
                Description  = dto.Description,
                HealthStatus = dto.HealthStatus,
                ImageURL     = dto.ImageUrl
            };

            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();

            // 4. Return response DTO
            return new PetResponseDto
            {
                PetId        = pet.PetId,
                Name         = pet.PetName,
                Species      = pet.Species,
                Breed        = pet.Breed,
                Age          = pet.Age,
                Gender       = pet.Gender,
                Description  = pet.Description,
                HealthStatus = pet.HealthStatus,
                ImageUrl     = pet.ImageURL,
                CenterName   = center.CenterName
            };
        }

        // =============================================
        // Get all available pets (public endpoint)
        // =============================================
        public async Task<IEnumerable<PetResponseDto>> GetAllAvailablePetsAsync()
        {
            var pets = await _context.Pets
                .Include(p => p.Center)
                .ToListAsync();

            return pets.Select(p => new PetResponseDto
            {
                PetId        = p.PetId,
                Name         = p.PetName,
                Species      = p.Species,
                Breed        = p.Breed,
                Age          = p.Age,
                Gender       = p.Gender,
                Description  = p.Description,
                HealthStatus = p.HealthStatus,
                ImageUrl     = p.ImageURL,
                CenterName   = p.Center?.CenterName ?? string.Empty
            });
        }

        // =============================================
        // Get pets belonging to the logged-in center
        // =============================================
        public async Task<IEnumerable<PetResponseDto>> GetPetsByCenterAsync(string userId)
        {
            // 1. Parse the userId claim
            if (!int.TryParse(userId, out var parsedUserId))
                throw new Exception("معرّف المستخدم غير صالح.");

            // 2. Find the AdoptionCenter linked to this user
            var center = await _context.AdoptionCenters
                .FirstOrDefaultAsync(c => c.UserId == parsedUserId);

            if (center == null)
                throw new Exception("لم يتم العثور على مركز تبني مرتبط بهذا الحساب.");

            // 3. Query only pets that belong to this center
            var pets = await _context.Pets
                .Where(p => p.CenterId == center.CenterId)
                .ToListAsync();

            return pets.Select(p => new PetResponseDto
            {
                PetId        = p.PetId,
                Name         = p.PetName,
                Species      = p.Species,
                Breed        = p.Breed,
                Age          = p.Age,
                Gender       = p.Gender,
                Description  = p.Description,
                HealthStatus = p.HealthStatus,
                ImageUrl     = p.ImageURL,
                CenterName   = center.CenterName
            });
        }
        // =============================================
        // Update a pet (ownership-verified)
        // =============================================
        public async Task<PetResponseDto> UpdatePetAsync(int petId, UpdatePetDto dto, string userId)
        {
            // 1. Parse the userId claim
            if (!int.TryParse(userId, out var parsedUserId))
                throw new Exception("معرّف المستخدم غير صالح.");

            // 2. Load the pet together with its center (single DB round-trip)
            var pet = await _context.Pets
                .Include(p => p.Center)
                .FirstOrDefaultAsync(p => p.PetId == petId);

            if (pet == null)
                throw new Exception("الحيوان الأليف المطلوب غير موجود.");

            // 3. Verify ownership — the center linked to this pet must belong to the logged-in user
            if (pet.Center == null || pet.Center.UserId != parsedUserId)
                throw new UnauthorizedAccessException("ليس لديك صلاحية لتعديل هذا الحيوان الأليف.");

            // 4. Apply only the fields that were provided (null = keep existing value)
            if (dto.Name        != null) pet.PetName      = dto.Name;
            if (dto.Age         != null) pet.Age          = dto.Age;
            if (dto.HealthStatus!= null) pet.HealthStatus = dto.HealthStatus;
            if (dto.Description != null) pet.Description  = dto.Description;
            if (dto.ImageUrl    != null) pet.ImageURL     = dto.ImageUrl;

            await _context.SaveChangesAsync();

            // 5. Return the updated response DTO
            return new PetResponseDto
            {
                PetId        = pet.PetId,
                Name         = pet.PetName,
                Species      = pet.Species,
                Breed        = pet.Breed,
                Age          = pet.Age,
                Gender       = pet.Gender,
                Description  = pet.Description,
                HealthStatus = pet.HealthStatus,
                ImageUrl     = pet.ImageURL,
                CenterName   = pet.Center.CenterName
            };
        }

        // =============================================
        // Delete a pet (ownership-verified)
        // =============================================
        public async Task DeletePetAsync(int petId, string userId)
        {
            // 1. Parse the userId claim
            if (!int.TryParse(userId, out var parsedUserId))
                throw new Exception("معرّف المستخدم غير صالح.");

            // 2. Load the pet together with its center
            var pet = await _context.Pets
                .Include(p => p.Center)
                .FirstOrDefaultAsync(p => p.PetId == petId);

            if (pet == null)
                throw new Exception("الحيوان الأليف المطلوب غير موجود.");

            // 3. Verify ownership
            if (pet.Center == null || pet.Center.UserId != parsedUserId)
                throw new UnauthorizedAccessException("ليس لديك صلاحية لحذف هذا الحيوان الأليف.");

            // 4. Remove and persist
            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
        }
    }
}
