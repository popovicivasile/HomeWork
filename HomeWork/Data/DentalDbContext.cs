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
                new RefStatusType { Id = Guid.Parse("B87BDB63-8EFA-410D-9750-F3A61BF98796"), Name = "Confirmed" },
                new RefStatusType { Id = Guid.Parse("64F98627-5932-4C00-B365-D36579AF5B9F"), Name = "Pending" },
                new RefStatusType { Id = Guid.Parse("4023DB95-1762-4B3B-A9BA-8484586A42F2"), Name = "Cancelled" }
                );


            modelBuilder.Entity<RefDentalProcedures>().HasData(
                new RefDentalProcedures { Id = Guid.Parse("D30941AE-B428-4C42-BA55-079678747851"), Name = "A", DurationInMinutes = 30 },
                new RefDentalProcedures { Id = Guid.Parse("832081D0-5A02-492F-8AAB-785D9D4B36FC"), Name = "B", DurationInMinutes = 45 },
                new RefDentalProcedures { Id = Guid.Parse("9B64EC7F-9B58-40E9-B910-6CD44953CEA4"), Name = "C", DurationInMinutes = 60 },
                new RefDentalProcedures { Id = Guid.Parse("4E4DFF86-51DE-432D-822F-1845AD52772E"), Name = "D", DurationInMinutes = 20 },
                new RefDentalProcedures { Id = Guid.Parse("80874D68-1511-4465-BD9F-CA08AA179933"), Name = "E", DurationInMinutes = 40 }
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