using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HomeWork.Data.Domain
{
    public class UserRegistration : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(50)]
        public string PhoneNumber { get; set; }
        public ICollection<DoctorDentalProcedure> DoctorDentalProcedure { get; set; } = new List<DoctorDentalProcedure>();
        public ICollection<ProcedureRegistrationCard> PatientAppointments { get; set; } = new List<ProcedureRegistrationCard>();
        public ICollection<ProcedureRegistrationCard> DoctorAppointments { get; set; } = new List<ProcedureRegistrationCard>();
    }
}
