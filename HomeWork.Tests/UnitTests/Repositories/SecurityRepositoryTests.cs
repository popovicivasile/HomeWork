using HomeWork.Data;
using HomeWork.Data.Domain;
using HomeWork.Data.Repository.Abstract;
using HomeWork.Data.Repository.Real;
using HomeWork.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using Xunit;

namespace HomeWork.Tests.UnitTests.Repositories
{
    public class SecurityRepositoryTests
    {
        private readonly DentalDbContext _dbContext;
        private readonly Mock<UserManager<UserRegistration>> _userManagerMock;
        private readonly Mock<SignInManager<UserRegistration>> _signInManagerMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<IMailService> _mailServiceMock;
        private readonly SecurityRepository _repository;

        public SecurityRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DentalDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Security_" + Guid.NewGuid().ToString())
                .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            _dbContext = new DentalDbContext(options);

            _userManagerMock = new Mock<UserManager<UserRegistration>>(
                new Mock<IUserStore<UserRegistration>>().Object, null, null, null, null, null, null, null, null);
            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            var claimsFactoryMock = new Mock<IUserClaimsPrincipalFactory<UserRegistration>>();
            _signInManagerMock = new Mock<SignInManager<UserRegistration>>(
                _userManagerMock.Object, contextAccessorMock.Object, claimsFactoryMock.Object, null, null, null, null);
            _configMock = new Mock<IConfiguration>();
            _configMock.Setup(c => c["Jwt:Key"]).Returns("super-secret-key-1234567890");
            _configMock.Setup(c => c["Jwt:Issuer"]).Returns("test-issuer");
            _configMock.Setup(c => c["Jwt:Audience"]).Returns("test-audience");

            _mailServiceMock = new Mock<IMailService>();

            _repository = new SecurityRepository(_dbContext, _configMock.Object, _userManagerMock.Object, _signInManagerMock.Object, _mailServiceMock.Object);
        }

        [Fact]
        public async Task SignInAsync_ValidData_Success()
        {
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

            _mailServiceMock.Setup(ms => ms.SendAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            )).Returns(Task.CompletedTask);

            var result = await _repository.SignInAsync(registrationData);

            Assert.True(result.Succeeded);
            _userManagerMock.Verify(um => um.AddToRoleAsync(It.IsAny<UserRegistration>(), "Patient"), Times.Once);
        }
    }
}