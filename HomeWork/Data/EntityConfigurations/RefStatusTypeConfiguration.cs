using HomeWork.Data.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeWork.Data.EntityConfigurations
{

    public class RefStatusTypeConfiguration : IEntityTypeConfiguration<RefStatusType>

    {
        public void Configure(EntityTypeBuilder<RefStatusType> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }

}
