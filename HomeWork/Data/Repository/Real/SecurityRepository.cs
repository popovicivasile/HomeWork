using HomeWork.Data.Domain;
using HomeWork.Data.Repository.Abstract;
using HomeWork.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HomeWork.Data.Repository.Real
{
    public class SecurityRepository : ISecurityRepository
    {
        private readonly DentalDbContext _dbContext;
        private readonly UserManager<UserRegistration> _userManager;
        private readonly SignInManager<UserRegistration> _signInManager;
        private readonly IConfiguration config;
        public SecurityRepository(DentalDbContext dbContext, IConfiguration config, UserManager<UserRegistration> userManager, SignInManager<UserRegistration> signInManager)
        {
            _dbContext = dbContext;
            _signInManager = signInManager;
            _userManager = userManager;
            this.config = config;
        }


        public async Task<IdentityResult> SignInAsync(RegistrationDto registrationData)
        {
            await using var transactionAo = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                
                var user = new UserRegistration
                {
                    UserName = registrationData.UserName,
                    Email = registrationData.Email,
                    FirstName = registrationData.FirstName,
                    LastName = registrationData.LastName,
                    PhoneNumber = registrationData.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, registrationData.Password);
                if (!result.Succeeded)
                {
                    return result;
                }

                await _userManager.AddToRoleAsync(user, "Patient");
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = $"https://yourwebsite.com/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";
                MailService _emailService = new MailService(config);
                await _emailService.SendAsync(user.Email, "Confirm Your Email",
                    $"Please confirm your email by clicking {confirmationLink} ");
                await transactionAo.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await transactionAo.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return IdentityResult.Failed(new IdentityError { Description = "Invalid confirmation request." });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            return await _userManager.ConfirmEmailAsync(user, token);
        }


        public async Task<LoginInformationDto> LogInAsync(LoginDTO loginData)
        {
            LoginInformationDto loginInformation = new LoginInformationDto();
            try
            {
                var user = await _userManager.FindByEmailAsync(loginData.Email);

                if (user != null && await _userManager.CheckPasswordAsync(user, loginData.Password))
                {
                    var token = await GenerateJwtTokenAsync(user);

                    loginInformation.Token = token;
                    loginInformation.ExpirationTime = DateTime.Now.AddHours(2);
                }
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException("Invalid login attempt.");
            }
            return loginInformation;
        }


        public async Task<string> LogOutAsync()
        {
            await _signInManager.SignOutAsync();
            return "Logout successful";
        }


        public async Task<string> RegisterDoctorAsync(DoctorDto model)
        {
            try
            {
                var user = new UserRegistration
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, "Doctor");
                var userId = user.Id;

                List<Guid> RefProcedure = model.RefDentalProcedureId;

                foreach (var refProcedureId in RefProcedure)
                {
                    DoctorDentalProcedure doctorDentalProcedure = new DoctorDentalProcedure
                    {
                        UserId = userId,
                        RefDentalProcedureId = refProcedureId
                    };
                    _dbContext.DoctorDentalProcedures.Add(doctorDentalProcedure);
                }
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return "Doctor registered successfully";
        }


     
        public async Task<string> GenerateJwtTokenAsync(UserRegistration user)
        {
            var claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(30);

            var token = new JwtSecurityToken(
                config["Jwt:Issuer"],
                config["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
