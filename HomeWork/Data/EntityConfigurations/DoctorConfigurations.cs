using HomeWork.Data.Domain;
using HomeWork.Data.EntityConfigurations.BaseEntityConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeWork.Data.EntityConfigurations
{
    public class DoctorConfigurations : BaseEntityConfiguration<Doctor>,
        IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(d => d.LastName)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
