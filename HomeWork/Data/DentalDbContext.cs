using HomeWork.Data.Domain;
using HomeWork.Data.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace HomeWork.Data
{
    public class DentalDbContext : DbContext
    {
        public DbSet<UserRegistration> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<RefDentalProcedures> RefDentalProcedures { get; set; }


        public DentalDbContext(DbContextOptions<DentalDbContext> options)
            : base(options)
        {

        }
        public bool AllMigrationsApplied => !Database.GetPendingMigrations().Any();
        public bool AnyMigrations => Database.GetMigrations().Any();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DentalDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
