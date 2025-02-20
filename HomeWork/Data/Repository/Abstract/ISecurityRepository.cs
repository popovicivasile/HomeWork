using HomeWork.Data.Domain;
using HomeWork.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace HomeWork.Data.Repository.Abstract
{
    public interface ISecurityRepository
    {
        Task<IdentityResult> SignInAsync(RegistrationDto registrationData);
        Task<LoginInformationDto> LogInAsync(LoginDTO loginData);
        Task<string> LogOutAsync();
        Task<string> RegisterDoctorAsync(DoctorDto model);
    }
}
