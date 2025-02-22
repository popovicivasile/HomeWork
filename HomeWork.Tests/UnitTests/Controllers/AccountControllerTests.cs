using HomeWork.Controllers;
using HomeWork.Data.Repository.Abstract;
using HomeWork.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace HomeWork.Tests.UnitTests.Controllers
{
    public class AccountControllerTests
    {
        private readonly Mock<ISecurityRepository> _securityRepoMock;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            _securityRepoMock = new Mock<ISecurityRepository>();
            _controller = new AccountController(_securityRepoMock.Object);
        }

        
        [Fact]
        public async Task SignIn_ValidData_ReturnsOk()
        {
            // Arrange
            var securityRepoMock = new Mock<ISecurityRepository>();
            securityRepoMock.Setup(repo => repo.SignInAsync(It.IsAny<RegistrationDto>()))
                .ReturnsAsync(IdentityResult.Success);
            var controller = new AccountController(securityRepoMock.Object);
            var model = new RegistrationDto
            {
                UserName = "testuser",
                Email = "test@example.com",
                FirstName = "Test",
                LastName = "User",
                PhoneNumber = "1234567890",
                Password = "Password123"
            };

            var result = await controller.SignIn(model);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<IdentityResult>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            var loginDto = new LoginDTO { Email = "test@example.com", Password = "Pass123" };
            var loginInfo = new LoginInformationDto { Token = "jwt-token", ExpirationTime = DateTime.Now.AddHours(1) };
            _securityRepoMock.Setup(repo => repo.LogInAsync(loginDto)).ReturnsAsync(loginInfo);

            var result = await _controller.Login(loginDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(loginInfo, okResult.Value);
        }
    }
}