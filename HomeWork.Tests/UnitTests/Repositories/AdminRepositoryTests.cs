using HomeWork.Data;
using HomeWork.Data.Domain;
using HomeWork.Data.Domain.ValueObjects;
using HomeWork.Data.Repository.Real;
using HomeWork.Models;
using HomeWork.Models.Admin;
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
    public class AdminRepositoryTests
    {
        private readonly DentalDbContext _dbContext;
        private readonly Mock<UserManager<UserRegistration>> _userManagerMock;

        public AdminRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DentalDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Admin")
                .Options;
            _dbContext = new DentalDbContext(options);

            _userManagerMock = new Mock<UserManager<UserRegistration>>(
                new Mock<IUserStore<UserRegistration>>().Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public async Task GetAllDoctorsAsync_ReturnsDoctors()
        {
            var doctor = new UserRegistration { Id = "doctor1", UserName = "drtest" };
            _userManagerMock.Setup(um => um.GetUsersInRoleAsync("Doctor")).ReturnsAsync(new List<UserRegistration> { doctor });
            var repo = new AdminRepository(_dbContext, _userManagerMock.Object);

            var doctors = await repo.GetAllDoctorsAsync();

            Assert.Single(doctors);
            Assert.Equal("drtest", doctors[0].UserName);
        }

        [Fact]
        public async Task CreateProcedureAsync_AddsProcedure()
        {
            var repo = new AdminRepository(_dbContext, _userManagerMock.Object);
            var procedureDto = new ProcedureDto { Name = "TestProc", DurationInMinutes = 45 };

            var procedure = await repo.CreateProcedureAsync(procedureDto);

            Assert.Equal("TestProc", procedure.Name);
            Assert.Equal(45, procedure.DurationInMinutes);
            Assert.Single(_dbContext.RefDentalProcedures);
        }
    }
}