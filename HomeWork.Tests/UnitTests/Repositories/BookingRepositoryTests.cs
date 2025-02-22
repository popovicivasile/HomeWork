using HomeWork.Data;
using HomeWork.Data.Domain;
using HomeWork.Data.Domain.ValueObjects;
using HomeWork.Data.Repository.Real;
using HomeWork.Models.Booking;
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
    public class BookingRepositoryTests
    {
        private readonly DentalDbContext _dbContext;
        private readonly Mock<UserManager<UserRegistration>> _userManagerMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<MailService> _mailServiceMock;

        public BookingRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DentalDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Booking")
                .Options;
            _dbContext = new DentalDbContext(options);

            _userManagerMock = new Mock<UserManager<UserRegistration>>(
                new Mock<IUserStore<UserRegistration>>().Object, null, null, null, null, null, null, null, null);
            _configMock = new Mock<IConfiguration>();
            _mailServiceMock = new Mock<MailService>(_configMock.Object);
        }

        [Fact]
        public async Task BookAppointmentAsync_ValidData_Success()
        {
            // Arrange
            var patient = new UserRegistration { Id = "patient1", Email = "patient@example.com" };
            var doctor = new UserRegistration { Id = "doctor1" };
            var procedure = new RefDentalProcedures { Id = Guid.NewGuid(), Name = "A", DurationInMinutes = 30 };
            _dbContext.Users.AddRange(patient, doctor);
            _dbContext.RefDentalProcedures.Add(procedure);
            _dbContext.DoctorDentalProcedures.Add(new DoctorDentalProcedure { UserId = "doctor1", RefDentalProcedureId = procedure.Id });
            await _dbContext.SaveChangesAsync();

            var repo = new BookingRepository(_dbContext, _userManagerMock.Object, _configMock.Object, _mailServiceMock.Object);
            var bookingDto = new BookingDto
            {
                ProcedureId = procedure.Id,
                DoctorId = "doctor1",
                AppointmentTime = DateTime.UtcNow.AddDays(1)
            };
            _userManagerMock.Setup(um => um.FindByIdAsync("patient1")).ReturnsAsync(patient);
            _userManagerMock.Setup(um => um.FindByIdAsync("doctor1")).ReturnsAsync(doctor);

            // Act
            var result = await repo.BookAppointmentAsync(bookingDto, "patient1");

            // Assert
            Assert.Equal("Appointment booked successfully.", result);
            Assert.Single(_dbContext.ProcedureRegistrationCards);
            _mailServiceMock.Verify(ms => ms.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetBookingStatsAsync_ReturnsStats()
        {
            // Arrange
            var doctor = new UserRegistration { Id = "doctor1", FirstName = "John", LastName = "Doe" };
            var procedure = new RefDentalProcedures { Id = Guid.NewGuid(), Name = "A", DurationInMinutes = 30 };
            _dbContext.Users.Add(doctor);
            _dbContext.RefDentalProcedures.Add(procedure);
            _dbContext.ProcedureRegistrationCards.Add(new ProcedureRegistrationCard
            {
                DoctorId = "doctor1",
                ProcedureId = procedure.Id,
                AppointmentTime = DateTime.UtcNow,
                StatusId = Guid.Parse("your-confirmed-guid") // Replace with actual Confirmed GUID
            });
            await _dbContext.SaveChangesAsync();

            var repo = new BookingRepository(_dbContext, _userManagerMock.Object, _configMock.Object, _mailServiceMock.Object);

            // Act
            var stats = await repo.GetBookingStatsAsync("doctor1", DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1), procedure.Id);

            // Assert
            Assert.Single(stats);
            Assert.Equal("John Doe", stats[0].DoctorName);
        }
    }
}