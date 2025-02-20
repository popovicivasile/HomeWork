using HomeWork.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeWork.Data.EntityConfigurations
{
    public class DoctorDentalProcedureConfiguration : IEntityTypeConfiguration<DoctorDentalProcedure>
    {
        public void Configure(EntityTypeBuilder<DoctorDentalProcedure> builder)
        {
            builder.HasKey(i => i.Id);

            builder.HasOne(i => i.User)
                .WithMany(d => d.DoctorDentalProcedure)
                .HasForeignKey(i => i.UserId);

            builder.HasOne(i => i.RefDentalProcedure)
                .WithMany()
                .HasForeignKey(i => i.RefDentalProcedureId);
        }
    }
}
