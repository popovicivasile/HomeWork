using HomeWork.Data.Repository.Abstract;
using HomeWork.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeWork.Controllers
{
    [Route("api/core/account/[action]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly ISecurityRepository _securityRepository;
        public AccountController(ISecurityRepository securityRepository)
        {
            _securityRepository = securityRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> SignIn(RegistrationDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _securityRepository.SignInAsync(model);
                return result.Succeeded ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred during registration", Error = ex.Message });
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _securityRepository.ConfirmEmailAsync(userId, token);
            if (result.Succeeded)
            {
                return Ok("Email confirmed successfully.");
            }

            return BadRequest(result.Errors);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loginInformation = await _securityRepository.LogInAsync(model);
            if (loginInformation != null)
            {
                return Ok(loginInformation);
            }

            return Unauthorized("Invalid login attempt.");
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            string message = await _securityRepository.LogOutAsync();
            return Ok(message);
        }

        [HttpPost("register/doctor")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterDoctor(DoctorDto model)
        {
            if (model == null)
            {
                return BadRequest("Invalid request data.");
            }

            var result = await _securityRepository.RegisterDoctorAsync(model);

            if (result == null)
            {
                return BadRequest("Doctor registration failed.");
            }

            return Ok(result);
        }

    }
}
