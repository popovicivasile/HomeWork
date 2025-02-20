using HomeWork.Data.Repository.Abstract;
using HomeWork.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeWork.Controllers
{
    [Route("api/core/[action]")]
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
            if (ModelState.IsValid)
            {
                var result = await _securityRepository.SignInAsync(model);

                if (result.Succeeded)
                {
                    return Ok(new { message = "Registration successful, now you can login" });
                }
                return BadRequest(new { errors = string.Join(", ", result.Errors.Select(e => e.Description)) });
            }
            return BadRequest(ModelState);
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

        [HttpPost("register/medicalprofessional")]
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
