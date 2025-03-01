﻿using System.ComponentModel.DataAnnotations;

namespace HomeWork.Data.Domain.ValueObjects
{

    public class RefDentalProcedures
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public int DurationInMinutes { get; set; }
        public ICollection<DoctorDentalProcedure> DoctorDentalProcedures { get; set; } = new List<DoctorDentalProcedure>();
        public ICollection<ProcedureRegistrationCard> ProcedureRegistrations { get; set; } = new List<ProcedureRegistrationCard>();
    }
}
