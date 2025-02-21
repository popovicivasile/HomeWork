using HomeWork.Data.Domain.ValueObjects;
using System;
using System.ComponentModel.DataAnnotations;

namespace HomeWork.Data.Domain
{
    public class ProcedureRegistrationCard
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string PatientId { get; set; }
        public UserRegistration Patient { get; set; }

        [Required]
        public string DoctorId { get; set; }
        public UserRegistration Doctor { get; set; }

        [Required]
        public Guid ProcedureId { get; set; }
        public RefDentalProcedures Procedure { get; set; }

        [Required]
        public DateTime AppointmentTime { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}