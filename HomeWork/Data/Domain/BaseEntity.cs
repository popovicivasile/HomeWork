using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HomeWork.Data.Domain
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? LastModifiedDate { get; set; }
        public string? CreatedByUserId { get; set; }
    }
}
