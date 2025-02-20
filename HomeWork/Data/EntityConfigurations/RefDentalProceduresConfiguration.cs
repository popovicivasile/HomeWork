using HomeWork.Data.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeWork.Data.EntityConfigurations
{
    public class RefDentalProceduresConfigurations : IEntityTypeConfiguration<RefDentalProcedures>

    {
        public void Configure(EntityTypeBuilder<RefDentalProcedures> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
