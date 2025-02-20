using HomeWork.Data.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeWork.Controllers
{
    [Route("api/core/[action]")]
    [ApiController]
    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllProcedures()
        {
            try
            {
                var procedures = await _bookingRepository.GetAllProceduresAsync();
                if (procedures == null)
                {
                    return NotFound();
                }
                return Ok(procedures);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching procedures.");
            }
        }
    }
}
