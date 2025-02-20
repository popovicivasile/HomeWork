using HomeWork.Data.Domain;
using HomeWork.Data.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeWork.Data
{
    public class DentalDbContext : IdentityDbContext<UserRegistration>
    {
        public DbSet<UserRegistration> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<RefDentalProcedures> RefDentalProcedures { get; set; }
        public DbSet<IdentityRole> Roles { get; set; }


        public DentalDbContext(DbContextOptions<DentalDbContext> options)
            : base(options)
        {

        }
        public bool AllMigrationsApplied => !Database.GetPendingMigrations().Any();
        public bool AnyMigrations => Database.GetMigrations().Any();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityRole>().HasData(SeedRoles());
            modelBuilder.Entity<UserRegistration>().HasData(SeedSuperAdmin());
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "1", 
                    UserId = "1"
                }
            );
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DentalDbContext).Assembly);
           
        }
        private List<IdentityRole> SeedRoles()
        {
            return new List<IdentityRole>
            {
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "MedicalProfessional", NormalizedName = "MEDICALPROFESSIONAL" },
                new IdentityRole { Id = "3", Name = "Patient", NormalizedName = "PATIENT" }
            };
        }

        private UserRegistration SeedSuperAdmin()
        {
            var hasher = new PasswordHasher<UserRegistration>();
            return new UserRegistration
            {
                Id = "1",
                UserName = "TemporaryUsername",
                NormalizedUserName = "TEMPORARY-USERNAME",
                PhoneNumber  = "1234567890",
                Email = "TemporaryEmail@example.com",
                NormalizedEmail = "TEMPORARYEMAIL@EXAMPLE.COM",
                FirstName = "Temporary first Name",
                LastName = "Temporary last Name",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "TemporaryPassword"),
                SecurityStamp = string.Empty
            };
        }
    }
}
