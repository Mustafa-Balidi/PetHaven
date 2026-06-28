using Microsoft.EntityFrameworkCore;
using PetHaven.Models;

namespace PetHaven.Data
{
    public class DatabaseSeeder
    {
        private readonly ApplicationDbContext _context;

        public DatabaseSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Guard: if Pets already exist, the database has been seeded before.
            if (await _context.Pets.AnyAsync()) return;

            // =========================================================
            // 1. Roles
            // =========================================================
            var roleAdopter        = await EnsureRoleAsync("Adopter");
            var roleCenter         = await EnsureRoleAsync("AdoptionCenter");
            var roleVet            = await EnsureRoleAsync("Vet");
            await _context.SaveChangesAsync();

            // =========================================================
            // 2. Users  (passwords hashed with BCrypt)
            // =========================================================

            // --- Adopter user ---
            var adopterUser = new User
            {
                RoleId      = roleAdopter.RoleId,
                UserName    = "john_adopter",
                FullName    = "John Smith",
                Email       = "john.adopter@pethaven.com",
                Password    = BCrypt.Net.BCrypt.HashPassword("Adopter@123"),
                PhoneNumber = "+1-555-0101"
            };

            // --- Adoption Center user ---
            var centerUser = new User
            {
                RoleId      = roleCenter.RoleId,
                UserName    = "happy_paws_center",
                FullName    = "Happy Paws Adoption Center",
                Email       = "contact@happypaws.com",
                Password    = BCrypt.Net.BCrypt.HashPassword("Center@123"),
                PhoneNumber = "+1-555-0202"
            };

            // --- Vet user ---
            var vetUser = new User
            {
                RoleId      = roleVet.RoleId,
                UserName    = "dr_sarah_vet",
                FullName    = "Dr. Sarah Johnson",
                Email       = "sarah.johnson@pethaven.com",
                Password    = BCrypt.Net.BCrypt.HashPassword("Vet@123"),
                PhoneNumber = "+1-555-0303"
            };

            await _context.Users.AddRangeAsync(adopterUser, centerUser, vetUser);
            await _context.SaveChangesAsync();

            // =========================================================
            // 3. Profile records (use generated UserIds)
            // =========================================================

            // Adopter profile
            var adopterProfile = new Adopter
            {
                UserId           = adopterUser.UserId,
                Address          = "123 Maple Street, Springfield, IL 62701",
                HousingType      = "House",
                HasPetBefore     = true,
                ExperienceLevel  = "Intermediate",
                MissedReportsCount = 0,
                Balance          = 500.00m
            };

            // Cart for the adopter
            var adopterCart = new Cart
            {
                UserId    = adopterUser.UserId,
                CreatedAt = DateTime.UtcNow
            };

            // Adoption Center profile
            var centerProfile = new AdoptionCenter
            {
                UserId      = centerUser.UserId,
                CenterName  = "Happy Paws Adoption Center",
                Address     = "456 Oak Avenue, Chicago, IL 60601",
                ContactInfo = "contact@happypaws.com | +1-555-0202"
            };

            // Vet profile
            var vetProfile = new Vet
            {
                UserId          = vetUser.UserId,
                FullName        = "Dr. Sarah Johnson",
                Specialization  = "Small Animal Medicine",
                ClinicName      = "PetCare Veterinary Clinic",
                ClinicAddress   = "789 Elm Road, Chicago, IL 60602",
                PhoneNumber     = "+1-555-0303",
                Email           = "sarah.johnson@pethaven.com",
                ExperienceYears = 8,
                LicenseNumber   = "VET-IL-2024-00123",
                Location_Lat    = 41.8781m,
                Location_Lng    = -87.6298m,
                IsVerified      = true,
                CreatedAt       = DateTime.UtcNow
            };

            await _context.Adopters.AddAsync(adopterProfile);
            await _context.Carts.AddAsync(adopterCart);
            await _context.AdoptionCenters.AddAsync(centerProfile);
            await _context.Vets.AddAsync(vetProfile);
            await _context.SaveChangesAsync();

            // =========================================================
            // 4. Categories
            // =========================================================
            var catFood = new Category
            {
                CategoryName = "Food",
                Description  = "Nutritious meals and treats for all pet types.",
                ImageURL     = "https://placehold.co/200x200?text=Food"
            };
            var catToys = new Category
            {
                CategoryName = "Toys",
                Description  = "Fun and engaging toys to keep pets active.",
                ImageURL     = "https://placehold.co/200x200?text=Toys"
            };
            var catMedicine = new Category
            {
                CategoryName = "Medicine",
                Description  = "Health supplements, vitamins, and medication.",
                ImageURL     = "https://placehold.co/200x200?text=Medicine"
            };
            var catAccessories = new Category
            {
                CategoryName = "Accessories",
                Description  = "Collars, leashes, beds, and grooming supplies.",
                ImageURL     = "https://placehold.co/200x200?text=Accessories"
            };

            await _context.Categories.AddRangeAsync(catFood, catToys, catMedicine, catAccessories);
            await _context.SaveChangesAsync();

            // =========================================================
            // 5. Products  (linked to center + categories)
            // =========================================================
            var products = new List<Product>
            {
                new Product
                {
                    CenterId      = centerProfile.CenterId,
                    CategoryId    = catFood.CategoryId,
                    Name          = "Premium Dog Kibble (5 kg)",
                    Description   = "High-protein dry food formulated for adult dogs of all breeds.",
                    ProductPrice  = 34.99m,
                    DiscountRate  = 0.05m,
                    StockQuantity = 80,
                    ImageURL      = "https://placehold.co/300x300?text=DogKibble"
                },
                new Product
                {
                    CenterId      = centerProfile.CenterId,
                    CategoryId    = catFood.CategoryId,
                    Name          = "Gourmet Wet Cat Food (24-pack)",
                    Description   = "Grain-free pâté with real tuna for adult cats.",
                    ProductPrice  = 27.49m,
                    DiscountRate  = 0.00m,
                    StockQuantity = 60,
                    ImageURL      = "https://placehold.co/300x300?text=CatFood"
                },
                new Product
                {
                    CenterId      = centerProfile.CenterId,
                    CategoryId    = catToys.CategoryId,
                    Name          = "Interactive Rope Tug Toy",
                    Description   = "Durable braided rope toy ideal for fetch and tug-of-war.",
                    ProductPrice  = 9.99m,
                    DiscountRate  = 0.10m,
                    StockQuantity = 150,
                    ImageURL      = "https://placehold.co/300x300?text=RopeToy"
                },
                new Product
                {
                    CenterId      = centerProfile.CenterId,
                    CategoryId    = catToys.CategoryId,
                    Name          = "Feather Wand Cat Teaser",
                    Description   = "Retractable wand with colourful feather attachment for cats.",
                    ProductPrice  = 7.49m,
                    DiscountRate  = 0.00m,
                    StockQuantity = 120,
                    ImageURL      = "https://placehold.co/300x300?text=Feather"
                },
                new Product
                {
                    CenterId      = centerProfile.CenterId,
                    CategoryId    = catMedicine.CategoryId,
                    Name          = "Omega-3 Fish Oil Supplements (90 caps)",
                    Description   = "Supports coat health, joint function, and immune system in dogs and cats.",
                    ProductPrice  = 19.99m,
                    DiscountRate  = 0.00m,
                    StockQuantity = 200,
                    ImageURL      = "https://placehold.co/300x300?text=Omega3"
                },
                new Product
                {
                    CenterId      = centerProfile.CenterId,
                    CategoryId    = catAccessories.CategoryId,
                    Name          = "Adjustable Nylon Dog Collar (Medium)",
                    Description   = "Lightweight, waterproof collar with quick-release buckle.",
                    ProductPrice  = 12.99m,
                    DiscountRate  = 0.15m,
                    StockQuantity = 95,
                    ImageURL      = "https://placehold.co/300x300?text=Collar"
                }
            };

            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();

            // =========================================================
            // 6. Pets  (linked to center)
            // =========================================================
            var pets = new List<Pet>
            {
                new Pet
                {
                    CenterId     = centerProfile.CenterId,
                    PetName      = "Buddy",
                    Species      = "Dog",
                    Breed        = "Golden Retriever",
                    Age          = 2,
                    Gender       = "Male",
                    HealthStatus = "Healthy",
                    Description  = "Friendly and energetic golden retriever who loves to play fetch.",
                    ImageURL     = "https://placehold.co/400x400?text=Buddy"
                },
                new Pet
                {
                    CenterId     = centerProfile.CenterId,
                    PetName      = "Luna",
                    Species      = "Cat",
                    Breed        = "Siamese",
                    Age          = 3,
                    Gender       = "Female",
                    HealthStatus = "Healthy",
                    Description  = "Elegant Siamese cat with striking blue eyes. Very affectionate.",
                    ImageURL     = "https://placehold.co/400x400?text=Luna"
                },
                new Pet
                {
                    CenterId     = centerProfile.CenterId,
                    PetName      = "Max",
                    Species      = "Dog",
                    Breed        = "German Shepherd",
                    Age          = 4,
                    Gender       = "Male",
                    HealthStatus = "Healthy",
                    Description  = "Intelligent and loyal German Shepherd, well-trained and great with kids.",
                    ImageURL     = "https://placehold.co/400x400?text=Max"
                },
                new Pet
                {
                    CenterId     = centerProfile.CenterId,
                    PetName      = "Bella",
                    Species      = "Cat",
                    Breed        = "Persian",
                    Age          = 1,
                    Gender       = "Female",
                    HealthStatus = "Healthy",
                    Description  = "Fluffy Persian kitten with a playful personality.",
                    ImageURL     = "https://placehold.co/400x400?text=Bella"
                },
                new Pet
                {
                    CenterId     = centerProfile.CenterId,
                    PetName      = "Charlie",
                    Species      = "Dog",
                    Breed        = "Beagle",
                    Age          = 5,
                    Gender       = "Male",
                    HealthStatus = "Healthy",
                    Description  = "Curious and gentle Beagle, great with families and other dogs.",
                    ImageURL     = "https://placehold.co/400x400?text=Charlie"
                },
                new Pet
                {
                    CenterId     = centerProfile.CenterId,
                    PetName      = "Mango",
                    Species      = "Rabbit",
                    Breed        = "Holland Lop",
                    Age          = 1,
                    Gender       = "Male",
                    HealthStatus = "Healthy",
                    Description  = "Adorable Holland Lop rabbit with floppy ears and a calm temperament.",
                    ImageURL     = "https://placehold.co/400x400?text=Mango"
                }
            };

            await _context.Pets.AddRangeAsync(pets);
            await _context.SaveChangesAsync();
        }

        // =========================================================
        // Helper: ensure a Role exists, return it (create if missing)
        // =========================================================
        private async Task<Role> EnsureRoleAsync(string roleName)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (role == null)
            {
                role = new Role { RoleName = roleName };
                await _context.Roles.AddAsync(role);
            }
            return role;
        }
    }
}
