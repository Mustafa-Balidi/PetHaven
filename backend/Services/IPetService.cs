using PetHaven.DTOs;

namespace PetHaven.Services
{
    public interface IPetService
    {
        /// <summary>
        /// Adds a new pet linked to the AdoptionCenter that belongs to the given userId.
        /// </summary>
        Task<PetResponseDto> AddPetAsync(CreatePetDto dto, string userId);

        /// <summary>
        /// Returns all pets that are available for adoption (no active appointment / not yet adopted).
        /// Currently returns every pet in the system; extend the filter as your business rules evolve.
        /// </summary>
        Task<IEnumerable<PetResponseDto>> GetAllAvailablePetsAsync();

        /// <summary>
        /// Returns all pets belonging to the AdoptionCenter linked to the given userId.
        /// </summary>
        Task<IEnumerable<PetResponseDto>> GetPetsByCenterAsync(string userId);

        /// <summary>
        /// Updates the editable fields of a pet. Verifies that the pet belongs
        /// to the AdoptionCenter of the logged-in user before applying changes.
        /// </summary>
        Task<PetResponseDto> UpdatePetAsync(int petId, UpdatePetDto dto, string userId);

        /// <summary>
        /// Deletes a pet permanently. Verifies ownership before removal.
        /// </summary>
        Task DeletePetAsync(int petId, string userId);
    }
}
