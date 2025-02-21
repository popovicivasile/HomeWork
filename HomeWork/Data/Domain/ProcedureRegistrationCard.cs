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
        public virtual UserRegistration Patient { get; set; }

        [Required]
        public string DoctorId { get; set; }
        public virtual UserRegistration Doctor { get; set; }

        [Required]
        public Guid ProcedureId { get; set; }
        public virtual RefDentalProcedures Procedure { get; set; }

        [Required]
        public DateTime AppointmentTime { get; set; }

        public Guid? StatusId { get; set; }
        public virtual RefStatusType Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}