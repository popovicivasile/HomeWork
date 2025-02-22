using HomeWork.Data;
using HomeWork.Data.Domain;
using HomeWork.Data.Repository.Real;
using HomeWork.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HomeWork.Tests.UnitTests.Repositories
{
    public class SecurityRepositoryTests
    {
        private readonly Mock<DentalDbContext> _dbContextMock;
        private readonly Mock<UserManager<UserRegistration>> _userManagerMock;
        private readonly Mock<SignInManager<UserRegistration>> _signInManagerMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly SecurityRepository _repository;

        public SecurityRepositoryTests()
        {
            _dbContextMock = new Mock<DentalDbContext>(new DbContextOptions<DentalDbContext>());
            _userManagerMock = new Mock<UserManager<UserRegistration>>(
                new Mock<IUserStore<UserRegistration>>().Object, null, null, null, null, null, null, null, null);
            _signInManagerMock = new Mock<SignInManager<UserRegistration>>(
                _userManagerMock.Object, new Mock<IHttpContextAccessor>().Object, null, null, null, null, null);
            _configMock = new Mock<IConfiguration>();
            _configMock.Setup(c => c["Jwt:Key"]).Returns("super-secret-key-1234567890");
            _configMock.Setup(c => c["Jwt:Issuer"]).Returns("test-issuer");
            _configMock.Setup(c => c["Jwt:Audience"]).Returns("test-audience");

            _repository = new SecurityRepository(_dbContextMock.Object, _configMock.Object, _userManagerMock.Object, _signInManagerMock.Object);
        }

        [Fact]
        public async Task SignInAsync_ValidData_Success()
        {
            // Arrange
            var registrationData = new RegistrationDto
            {
                UserName = "testuser",
                Email = "test@example.com",
                FirstName = "Test",
                LastName = "User",
                PhoneNumber = "1234567890",
                Password = "Password123"
            };
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<UserRegistration>(), registrationData.Password))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<UserRegistration>(), "Patient"))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<UserRegistration>()))
                .ReturnsAsync("token");

            var result = await _repository.SignInAsync(registrationData);

            Assert.True(result.Succeeded);
            _userManagerMock.Verify(um => um.AddToRoleAsync(It.IsAny<UserRegistration>(), "Patient"), Times.Once);
        }

        [Fact]
        public async Task RegisterDoctorAsync_ValidData_Success()
        {
            var doctorDto = new DoctorDto
            {
                UserName = "drtest",
                Email = "dr@example.com",
                FirstName = "Dr",
                LastName = "Test",
                PhoneNumber = "0987654321",
                Password = "DrPass123",
                RefDentalProcedureId = new List<Guid> { Guid.NewGuid() }
            };
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<UserRegistration>(), doctorDto.Password))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<UserRegistration>(), "Doctor"))
                .ReturnsAsync(IdentityResult.Success);
            _dbContextMock.Setup(db => db.DoctorDentalProcedures.Add(It.IsAny<DoctorDentalProcedure>()));
            _dbContextMock.Setup(db => db.SaveChangesAsync(default)).ReturnsAsync(1);

            var result = await _repository.RegisterDoctorAsync(doctorDto);

            Assert.Equal("Doctor registered successfully", result);
            _dbContextMock.Verify(db => db.DoctorDentalProcedures.Add(It.IsAny<DoctorDentalProcedure>()), Times.Once);
        }
    }
}