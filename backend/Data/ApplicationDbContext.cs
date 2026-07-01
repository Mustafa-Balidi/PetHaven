using Microsoft.EntityFrameworkCore;
using PetHaven.Models;

namespace PetHaven.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // =============================================
        // DbSets (جميع الجداول)
        // =============================================

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Adopter> Adopters { get; set; }
        public DbSet<AdoptionCenter> AdoptionCenters { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Blacklist> Blacklists { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AdoptionRequest> AdoptionRequests { get; set; }
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<PetReport> PetReports { get; set; }
        public DbSet<Vet> Vets { get; set; }

        // =============================================
        // OnModelCreating
        // =============================================

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =============================================
            // 1. Precision للأعداد العشرية (Decimal)
            // =============================================

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.PriceAtPurchase)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.ProductPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.DiscountRate)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Adopter>()
                .Property(a => a.Balance)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Vet>()
                .Property(v => v.Location_Lat)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Vet>()
                .Property(v => v.Location_Lng)
                .HasPrecision(18, 8);

            // =============================================
            // 2. العلاقات (Relationships)
            // =============================================

            // ---------------------------------------------
            // 2.1 User → Adopter (One-to-One)
            // ---------------------------------------------
            modelBuilder.Entity<Adopter>()
                .HasOne(a => a.User)
                .WithOne(u => u.Adopter)
                .HasForeignKey<Adopter>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------------------------
            // 2.2 User → AdoptionCenter (One-to-One)
            // ---------------------------------------------
            modelBuilder.Entity<AdoptionCenter>()
                .HasOne(ac => ac.User)
                .WithOne(u => u.AdoptionCenter)
                .HasForeignKey<AdoptionCenter>(ac => ac.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------------------------
            // 2.3 User → Vet (One-to-One)
            // ---------------------------------------------
            modelBuilder.Entity<Vet>()
                .HasOne(v => v.User)
                .WithOne(u => u.Vet)
                .HasForeignKey<Vet>(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------------------------
            // 2.4 User → Notifications (One-to-Many) ✅ Restrict
            // ---------------------------------------------
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------------------------
            // 2.5 User → Ratings (One-to-Many) ✅ Restrict
            // ---------------------------------------------
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------------------------
            // 2.6 User → Cart (One-to-Many) ✅ Restrict
            // ---------------------------------------------
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------------------------
            // 2.7 User → Orders (One-to-Many) ✅ Restrict
            // ---------------------------------------------
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------------------------
            // 2.8 User → Wishlists (One-to-Many) ✅ Restrict
            // ---------------------------------------------
            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.User)
                .WithMany(u => u.Wishlists)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------------------------
            // 2.9 User → Diagnoses (One-to-Many) ✅ Restrict
            // ---------------------------------------------
            modelBuilder.Entity<Diagnosis>()
                .HasOne(d => d.User)
                .WithMany(u => u.Diagnoses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------------------------
            // 2.10 Adopter → Appointments (One-to-Many)
            // ---------------------------------------------
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Adopter)
                .WithMany(ad => ad.Appointments)
                .HasForeignKey(a => a.AdopterId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------------------------
            // 2.11 Appointment → Pet (Many-to-One) ✅ Restrict
            // ---------------------------------------------
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Pet)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PetId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------------------------
            // 2.12 Pet → AdoptionCenter (Many-to-One)
            // ---------------------------------------------
            modelBuilder.Entity<Pet>()
                .HasOne(p => p.Center)
                .WithMany(c => c.Pets)
                .HasForeignKey(p => p.CenterId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------------------------
            // 2.13 Diagnosis → Pet (Many-to-One)
            // ---------------------------------------------
            modelBuilder.Entity<Diagnosis>()
                .HasOne(d => d.Pet)
                .WithMany(p => p.Diagnoses)
                .HasForeignKey(d => d.PetId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------------------------
            // 2.14 AdoptionCenter → Products (One-to-Many)
            // ---------------------------------------------
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Center)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CenterId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------------------------
            // 2.15 Product → Category (Many-to-One)
            // ---------------------------------------------
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------------------------
            // 2.16 Order → Payment (One-to-One)
            // ---------------------------------------------
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------------------------
            // 2.17 Order → OrderItems (One-to-Many)
            // ---------------------------------------------
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------------------------
            // 2.18 OrderItem → Product (Many-to-One)
            // ---------------------------------------------
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------------------------
            // 2.19 Cart → CartItems (One-to-Many)
            // ---------------------------------------------
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------------------------
            // 2.20 CartItem → Product (Many-to-One)
            // ---------------------------------------------
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------------------------
            // 2.21 Wishlist → Product (Many-to-One)
            // ---------------------------------------------
            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.Product)
                .WithMany(p => p.Wishlists)
                .HasForeignKey(w => w.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------------------------
            // 2.22 Blacklist (Many-to-One with Adopter)
            // ---------------------------------------------
            modelBuilder.Entity<Blacklist>()
                .HasOne(b => b.Adopter)
                .WithMany(a => a.Blacklists)
                .HasForeignKey(b => b.AdopterId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------------------------
            // 2.23 Blacklist (Many-to-One with Center)
            // ---------------------------------------------
            modelBuilder.Entity<Blacklist>()
                .HasOne(b => b.Center)
                .WithMany(c => c.Blacklists)
                .HasForeignKey(b => b.CenterId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------------------------
            // 2.24 PetReport (Many-to-One with AdoptionRequest)
            // ---------------------------------------------
            modelBuilder.Entity<PetReport>()
                .HasOne(pr => pr.AdoptionRequest)
                .WithMany(ar => ar.PetReports)
                .HasForeignKey(pr => pr.AdoptionRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------------------------------------------
            // 2.25 AdoptionRequest → Adopter (Many-to-One)
            // ---------------------------------------------
            modelBuilder.Entity<AdoptionRequest>()
                .HasOne(ar => ar.Adopter)
                .WithMany()
                .HasForeignKey(ar => ar.AdopterId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------------------------------------
            // 2.26 AdoptionRequest → Pet (Many-to-One)
            // ---------------------------------------------
            modelBuilder.Entity<AdoptionRequest>()
                .HasOne(ar => ar.Pet)
                .WithMany()
                .HasForeignKey(ar => ar.PetId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}