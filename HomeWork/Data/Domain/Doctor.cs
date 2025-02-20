using HomeWork.Data.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace HomeWork.Data.Domain
{
    public class Doctor : BaseEntity
    {

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        public ICollection<DoctorDentalProcedure> DoctorDentalProcedures { get; set; } = new List<DoctorDentalProcedure>();

    }
}
