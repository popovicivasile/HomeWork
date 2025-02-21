using HomeWork.Data.Domain;
using HomeWork.Data.Domain.ValueObjects;
using HomeWork.Models.Booking;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeWork.Data.Repository.Abstract
{
    public interface IBookingRepository
    {
        Task<List<RefDentalProcedures>> GetAllProceduresAsync();
        Task<List<UserRegistration>> GetAvailableDoctorsAsync(Guid procedureId, DateTime appointmentTime);
        Task<string> BookAppointmentAsync(BookingDto bookingDto, string patientId);
        Task<List<ProcedureRegistrationCard>> GetAllBookingsAsync(string searchTerm = null, string sortBy = "AppointmentTime");
        Task<List<BookingStatsDto>> GetBookingStatsAsync(
            string doctorId = null, Guid? procedureId = null, DateTime? startDate = null, DateTime? endDate = null);
        Task SendAppointmentRemindersAsync();
    }
}