using HomeWork.Data.Repository.Abstract;
using HomeWork.Models.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HomeWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
        }

 
        [HttpGet("procedures")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetAllProcedures()
        {
            try
            {
                var procedures = await _bookingRepository.GetAllProceduresAsync();
                return Ok(procedures);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving procedures: {ex.Message}");
            }
        }

  
        [HttpGet("doctors")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetAvailableDoctors([FromQuery] Guid procedureId, [FromQuery] DateTime appointmentTime)
        {
            if (procedureId == Guid.Empty || appointmentTime == default)
            {
                return BadRequest("Procedure ID and appointment time are required.");
            }

            try
            {
                var doctors = await _bookingRepository.GetAvailableDoctorsAsync(procedureId, appointmentTime);
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving available doctors: {ex.Message}");
            }
        }

        [HttpPost("book")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> BookAppointment([FromBody] BookingDto bookingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var patientId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(patientId))
                {
                    return Unauthorized("User not authenticated.");
                }

                var result = await _bookingRepository.BookAppointmentAsync(bookingDto, patientId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error booking appointment: {ex.Message}");
            }
        }

      
        [HttpGet("admin/bookings")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllBookings(
            [FromQuery] string searchTerm = null,
            [FromQuery] string sortBy = "AppointmentTime")
        {
            try
            {
                var bookings = await _bookingRepository.GetAllBookingsAsync(searchTerm, sortBy);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving bookings: {ex.Message}");
            }
        }

     
        [HttpGet("admin/stats")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBookingStats(
            [FromQuery] string doctorId,
            [FromQuery] Guid procedureId,
            [FromQuery] DateTimeOffset startDate,
            [FromQuery] DateTimeOffset endDate)
        {
            try
            {
                var stats = await _bookingRepository.GetBookingStatsAsync(doctorId, startDate, endDate, procedureId);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving statistics: {ex.Message}");
            }
        }

        [HttpPost("admin/send-reminders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SendAppointmentReminders()
        {
            try
            {
                await _bookingRepository.SendAppointmentRemindersAsync();
                return Ok("Reminders sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error sending reminders: {ex.Message}");
            }
        }
    }
}