using HomeWork.Data.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeWork.Data.EntityConfigurations
{
    public class DoctorDentalProcedureConfiguration
    {

        public void Configure(EntityTypeBuilder<DoctorDentalProcedure> builder)
        {
            builder.HasKey(i => i.Id);

            builder.HasOne(i => i.Doctor)
                .WithMany(d => d.DoctorDentalProcedures)
                .HasForeignKey(i => i.DoctorId);

            builder.HasOne(i => i.RefDentalProcedure)
                .WithMany(r => r.DoctorDentalProcedures)
                .HasForeignKey(i => i.RefDentalProcedureId);
        }
    }
}
