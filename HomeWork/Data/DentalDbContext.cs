using HomeWork.Data.Domain;
using HomeWork.Data.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HomeWork.Data
{
    public class DentalDbContext : IdentityDbContext<UserRegistration>
    {
        public DbSet<UserRegistration> Users { get; set; }
        public DbSet<RefDentalProcedures> RefDentalProcedures { get; set; }
        public DbSet<DoctorDentalProcedure> DoctorDentalProcedures { get; set; }
        public DbSet<ProcedureRegistrationCard> ProcedureRegistrationCards { get; set; }

        public DentalDbContext(DbContextOptions<DentalDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DentalDbContext).Assembly);

            modelBuilder.Entity<IdentityRole>().HasData(SeedRoles());
            modelBuilder.Entity<UserRegistration>().HasData(SeedSuperAdmin());
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = "1", UserId = "1" }
            );

            modelBuilder.Entity<RefStatusType>().HasData(
                new RefStatusType { Id = Guid.NewGuid(), Name = "Confirmed" },
                new RefStatusType { Id = Guid.NewGuid(), Name = "Pending" },
                new RefStatusType { Id = Guid.NewGuid(), Name = "Cancelled" }
                );


            modelBuilder.Entity<RefDentalProcedures>().HasData(
                new RefDentalProcedures { Id = Guid.NewGuid(), Name = "A", DurationInMinutes = 30 },
                new RefDentalProcedures { Id = Guid.NewGuid(), Name = "B", DurationInMinutes = 45 },
                new RefDentalProcedures { Id = Guid.NewGuid(), Name = "C", DurationInMinutes = 60 },
                new RefDentalProcedures { Id = Guid.NewGuid(), Name = "D", DurationInMinutes = 20 },
                new RefDentalProcedures { Id = Guid.NewGuid(), Name = "E", DurationInMinutes = 40 }
            );
        }

        private List<IdentityRole> SeedRoles() => new List<IdentityRole>
        {
            new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Id = "2", Name = "Doctor", NormalizedName = "DOCTOR" },
            new IdentityRole { Id = "3", Name = "Patient", NormalizedName = "PATIENT" }
        };

        private UserRegistration SeedSuperAdmin()
        {
            var hasher = new PasswordHasher<UserRegistration>();
            return new UserRegistration
            {
                Id = "1",
                UserName = "SuperUser",
                NormalizedUserName = "SUPERUSER",
                PhoneNumber = "1234567890",
                Email = "superuser@gmail.com",
                NormalizedEmail = "SUPERUSER@GMAIL.COM",
                FirstName = "Super",
                LastName = "User",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "password"),
                SecurityStamp = string.Empty
            };
        }
    }
}