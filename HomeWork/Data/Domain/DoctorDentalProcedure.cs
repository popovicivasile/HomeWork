using HomeWork.Data.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace HomeWork.Data.Domain
{
    public class DoctorDentalProcedure
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public UserRegistration User { get; set; }
        [Required]
        public Guid RefDentalProcedureId { get; set; }
        public RefDentalProcedures RefDentalProcedure { get; set; }
    }

}
