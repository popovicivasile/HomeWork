using HomeWork.Data.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HomeWork.Data.EntityConfigurations.BaseEntityConfiguration
{
    public class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(entity => entity.Id);
            builder.Property(entity => entity.CreatedByUserId)
                .IsRequired(false)
                .HasMaxLength(36);
            builder.Property(entity => entity.CreatedDate)
                .IsRequired();
            builder.Property(entity => entity.LastModifiedDate)
                .IsRequired(false);
        }
    }
}
