using HomeWork.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeWork.Data.EntityConfigurations
{
    public class ProcedureRegistrationCardConfiguration : IEntityTypeConfiguration<ProcedureRegistrationCard>
    {
        public void Configure(EntityTypeBuilder<ProcedureRegistrationCard> builder)
        {
            builder.HasKey(prc => prc.Id);

            builder.HasOne(prc => prc.Patient)
                .WithMany(u => u.PatientAppointments)
                .HasForeignKey(prc => prc.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(prc => prc.Doctor)
                .WithMany(u => u.DoctorAppointments)
                .HasForeignKey(prc => prc.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(prc => prc.Procedure)
                .WithMany(p => p.ProcedureRegistrations)
                .HasForeignKey(prc => prc.ProcedureId);

            builder.HasOne(prc => prc.Status)
                .WithMany()
                .HasForeignKey(prc => prc.StatusId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(prc => prc.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
