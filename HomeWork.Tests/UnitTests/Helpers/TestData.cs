using HomeWork.Core.RefStaticList;
using HomeWork.Data.Domain;
using HomeWork.Data.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace HomeWork.Tests.UnitTests.TestHelpers
{
    public static class TestData
    {
       
        public static UserRegistration GetPatient()
        {
            return new UserRegistration
            {
                Id = "patient1",
                UserName = "patientuser",
                Email = "patient@example.com", 
                FirstName = "Jane",
                LastName = "Doe",
                PhoneNumber = "1234567890" 
            };
        }

        public static UserRegistration GetDoctor()
        {
            return new UserRegistration
            {
                Id = "doctor1",
                UserName = "doctoruser", 
                Email = "doctor@example.com", 
                FirstName = "John",
                LastName = "Smith",
                PhoneNumber = "0987654321" 
            };
        }

        public static UserRegistration GetAdmin()
        {
            return new UserRegistration
            {
                Id = "admin1",
                UserName = "adminuser", 
                Email = "admin@example.com", 
                FirstName = "Admin",
                LastName = "User",
                PhoneNumber = "1112223333" 
            };
        }

        public static RefDentalProcedures GetProcedure()
        {
            return new RefDentalProcedures
            {
                Id = Guid.NewGuid(),
                Name = "Cleaning",
                DurationInMinutes = 30
            };
        }
        public static DoctorDentalProcedure GetDoctorProcedure(string doctorId, Guid procédureId)
        {
            return new DoctorDentalProcedure
            {
                Id = Guid.NewGuid(),
                UserId = doctorId,
                RefDentalProcedureId = procédureId
            };
        }
        public static ProcedureRegistrationCard GetConfirmedBooking(string patientId, string doctorId, Guid procedureId)
        {
            return new ProcedureRegistrationCard
            {
                Id = Guid.NewGuid(),
                PatientId = patientId,
                DoctorId = doctorId,
                ProcedureId = procedureId,
                AppointmentTime = DateTime.UtcNow.AddDays(1),
                StatusId = Guid.Parse(RefStatusTypeList.Confirmed),
                CreatedAt = DateTime.UtcNow
            };
        }

        public static ProcedureRegistrationCard GetCancelledBooking(string patientId, string doctorId, Guid procedureId)
        {
            return new ProcedureRegistrationCard
            {
                Id = Guid.NewGuid(),
                PatientId = patientId,
                DoctorId = doctorId,
                ProcedureId = procedureId,
                AppointmentTime = DateTime.UtcNow.AddDays(1),
                StatusId = Guid.Parse(RefStatusTypeList.Cancelled),
                CreatedAt = DateTime.UtcNow
            };
        }

        public static ProcedureRegistrationCard GetPendingBooking(string patientId, string doctorId, Guid procedureId)
        {
            return new ProcedureRegistrationCard
            {
                Id = Guid.NewGuid(),
                PatientId = patientId,
                DoctorId = doctorId,
                ProcedureId = procedureId,
                AppointmentTime = DateTime.UtcNow.AddDays(1),
                StatusId = Guid.Parse(RefStatusTypeList.Pending),
                CreatedAt = DateTime.UtcNow
            };
        }
        public static List<UserRegistration> GetDoctorList()
        {
            return new List<UserRegistration>
            {
                new UserRegistration
                {
                    Id = "doctor1",
                    UserName = "johnsmith",
                    Email = "john@example.com",
                    FirstName = "John",
                    LastName = "Smith",
                    PhoneNumber = "1112223333"
                },
                new UserRegistration
                {
                    Id = "doctor2",
                    UserName = "alicejones",
                    Email = "alice@example.com",
                    FirstName = "Alice",
                    LastName = "Jones",
                    PhoneNumber = "4445556666"
                }
            };
        }

       

        public static List<ProcedureRegistrationCard> GetBookingList(string patientId, string doctorId)
        {
            var procedure1 = GetProcedure();
            var procedure2 = GetProcedure();
            return new List<ProcedureRegistrationCard>
            {
                new ProcedureRegistrationCard
                {
                    Id = Guid.NewGuid(),
                    PatientId = patientId,
                    DoctorId = doctorId,
                    ProcedureId = procedure1.Id,
                    AppointmentTime = DateTime.UtcNow.AddDays(1),
                    StatusId = Guid.Parse(RefStatusTypeList.Confirmed),
                    CreatedAt = DateTime.UtcNow
                },
                new ProcedureRegistrationCard
                {
                    Id = Guid.NewGuid(),
                    PatientId = patientId,
                    DoctorId = doctorId,
                    ProcedureId = procedure2.Id,
                    AppointmentTime = DateTime.UtcNow.AddDays(2),
                    StatusId = Guid.Parse(RefStatusTypeList.Confirmed),
                    CreatedAt = DateTime.UtcNow
                }
            };
        }
    }
}