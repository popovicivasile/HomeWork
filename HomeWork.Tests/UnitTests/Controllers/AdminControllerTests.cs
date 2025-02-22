using HomeWork.Controllers;
using HomeWork.Data.Domain;
using HomeWork.Data.Domain.ValueObjects;
using HomeWork.Data.Repository.Abstract;
using HomeWork.Models;
using HomeWork.Models.Admin;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HomeWork.Tests.UnitTests.Controllers
{
    public class AdminControllerTests
    {
        private readonly Mock<IAdminRepository> _adminRepoMock;
        private readonly Mock<ISecurityRepository> _securityRepoMock;
        private readonly AdminController _controller;

        public AdminControllerTests()
        {
            _adminRepoMock = new Mock<IAdminRepository>();
            _securityRepoMock = new Mock<ISecurityRepository>();
            _controller = new AdminController(_adminRepoMock.Object, _securityRepoMock.Object);
        }

        [Fact]
        public async Task GetAllDoctors_ReturnsOkWithDoctorList()
        {
            var doctors = new List<UserRegistration> { new UserRegistration { Id = "1", FirstName = "John" } };
            _adminRepoMock.Setup(repo => repo.GetAllDoctorsAsync()).ReturnsAsync(doctors);

            var result = await _controller.GetAllDoctors();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(doctors, okResult.Value);
        }

        [Fact]
        public async Task CreateProcedure_ValidData_ReturnsOk()
        {
            var procedureDto = new ProcedureDto { Name = "TestProc", DurationInMinutes = 45 };
            var procedure = new RefDentalProcedures { Id = Guid.NewGuid(), Name = "TestProc", DurationInMinutes = 45 };
            _adminRepoMock.Setup(repo => repo.CreateProcedureAsync(procedureDto)).ReturnsAsync(procedure);

            var result = await _controller.CreateProcedure(procedureDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(procedure, okResult.Value);
        }
    }
}