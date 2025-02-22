using HomeWork.Data.Domain;
using HomeWork.Data.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWork.Models.Booking;
using HomeWork.Data.Repository.Abstract;
using HomeWork.Core.RefStaticList;

namespace HomeWork.Data.Repository.Real
{
    public class BookingRepository : IBookingRepository
    {
        private readonly DentalDbContext _dbContext;
        private readonly UserManager<UserRegistration> _userManager;
        private readonly IConfiguration _config;
        private readonly IMailService _mailService;

        public BookingRepository(
            DentalDbContext dbContext,
            UserManager<UserRegistration> userManager,
            IConfiguration config,
            IMailService mailService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _config = config;
            _mailService = mailService;
        }

        public async Task<List<RefDentalProcedures>> GetAllProceduresAsync()
        {
            return await _dbContext.RefDentalProcedures.ToListAsync();
        }

        public async Task<List<UserRegistration>> GetAvailableDoctorsAsync(Guid procedureId, DateTime appointmentTime)
        {
            var doctorIds = await _dbContext.DoctorDentalProcedures
                .Where(ddp => ddp.RefDentalProcedureId == procedureId)
                .Select(ddp => ddp.UserId)
                .ToListAsync();

            var bookedDoctorIds = await _dbContext.ProcedureRegistrationCards
                .Where(prc => prc.AppointmentTime == appointmentTime && prc.StatusId != Guid.Parse(RefStatusTypeList.Cancelled))
                .Select(prc => prc.DoctorId)
                .ToListAsync();

            return await _userManager.Users
                .Where(u => doctorIds.Contains(u.Id) && !bookedDoctorIds.Contains(u.Id))
                .ToListAsync();
        }

        public async Task<string> BookAppointmentAsync(BookingDto bookingDto, string patientId)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var availableDoctors = await GetAvailableDoctorsAsync(bookingDto.ProcedureId, bookingDto.AppointmentTime);
                if (!availableDoctors.Any(d => d.Id == bookingDto.DoctorId))
                {
                    throw new Exception("Selected doctor is not available at this time.");
                }

                var appointment = new ProcedureRegistrationCard
                {
                    Id = Guid.NewGuid(),
                    PatientId = patientId,
                    DoctorId = bookingDto.DoctorId,
                    ProcedureId = bookingDto.ProcedureId,
                    AppointmentTime = bookingDto.AppointmentTime,
                    StatusId = Guid.Parse(RefStatusTypeList.Confirmed)
                };

                _dbContext.ProcedureRegistrationCards.Add(appointment);
                await _dbContext.SaveChangesAsync();

                var patient = await _userManager.FindByIdAsync(patientId);
                var doctor = await _userManager.FindByIdAsync(bookingDto.DoctorId);
                var procedure = await _dbContext.RefDentalProcedures
                    .FirstOrDefaultAsync(p => p.Id == bookingDto.ProcedureId);

                await _mailService.SendAsync(patient.Email, "Appointment Confirmed",
                    $"Your appointment with Dr. {doctor.FirstName} {doctor.LastName} " +
                    $"for {procedure.Name} on {bookingDto.AppointmentTime} has been confirmed.");

                await transaction.CommitAsync();
                return "Appointment booked successfully.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Failed to book appointment: {ex.Message}");
            }
        }

        public async Task<List<ProcedureRegistrationCard>> GetAllBookingsAsync(string searchTerm = null, string sortBy = "AppointmentTime")
        {
            var query = _dbContext.ProcedureRegistrationCards

                .Include(prc => prc.Patient)
                .Include(prc => prc.Doctor)
                .Include(prc => prc.Procedure)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(prc =>
                    prc.Patient.FirstName.Contains(searchTerm) ||
                    prc.Doctor.FirstName.Contains(searchTerm) ||
                    prc.Procedure.Name.Contains(searchTerm));
            }

            query = sortBy switch
            {
                "Doctor" => query.OrderBy(prc => prc.Doctor.LastName),
                "Patient" => query.OrderBy(prc => prc.Patient.LastName),
                "Procedure" => query.OrderBy(prc => prc.Procedure.Name),
                _ => query.OrderBy(prc => prc.AppointmentTime)
            };

            return await query.ToListAsync();
        }

        public async Task<List<BookingStatsDto>> GetBookingStatsAsync(string doctorId, DateTimeOffset startDate, DateTimeOffset endDate, Guid procedureId)
        {
            List<BookingStatsDto> result = new List<BookingStatsDto>();
            try
            {
                var query = _dbContext.ProcedureRegistrationCards
                    .Include(prc => prc.Doctor)
                    .Include(prc => prc.Procedure)
                    .Where(s => s.DoctorId == doctorId && s.ProcedureId == procedureId &&
                                s.AppointmentTime >= startDate && s.AppointmentTime <= endDate)
                    .ToListAsync();

                var grouped = (await query)
                    .GroupBy(s => new { s.DoctorId, s.ProcedureId })
                    .Select(g => new BookingStatsDto
                    {
                        DoctorName = g.First().Doctor.FirstName + " " + g.First().Doctor.LastName,
                        ProcedureName = g.First().Procedure.Name,
                        AppointmentCount = g.Count(t => t.StatusId == Guid.Parse(RefStatusTypeList.Confirmed)),
                        TotalDuration = g.Sum(t => t.StatusId == Guid.Parse(RefStatusTypeList.Confirmed) ? t.Procedure.DurationInMinutes : 0),
                        CancelledCount = g.Count(t => t.StatusId == Guid.Parse(RefStatusTypeList.Cancelled)),
                        PeriodStart = startDate,
                        PeriodEnd = endDate
                    }).ToList();

                result = grouped.Any() ? grouped : result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get booking stats: {ex.Message}");
            }
            return result;
        }

        public async Task SendAppointmentRemindersAsync()
        {
            var tomorrow = DateTime.UtcNow.AddDays(1).Date;
            var appointments = await _dbContext.ProcedureRegistrationCards
                .Where(prc => prc.AppointmentTime.Date == tomorrow && prc.StatusId == Guid.Parse(RefStatusTypeList.Confirmed))
                .Include(prc => prc.Patient)
                .Include(prc => prc.Doctor)
                .Include(prc => prc.Procedure)
                .ToListAsync();

            foreach (var appointment in appointments)
            {
                await _mailService.SendAsync(appointment.Patient.Email, "Appointment Reminder",
                    $"Reminder: You have an appointment with Dr. {appointment.Doctor.FirstName} {appointment.Doctor.LastName} " +
                    $"for {appointment.Procedure.Name} tomorrow at {appointment.AppointmentTime}.");
            }
        }
    }
}