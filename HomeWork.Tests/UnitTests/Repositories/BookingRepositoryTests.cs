using HomeWork.Data;
using HomeWork.Data.Domain;
using HomeWork.Data.Domain.ValueObjects;
using HomeWork.Data.Repository.Abstract;
using HomeWork.Data.Repository.Real;
using HomeWork.Models.Booking;
using HomeWork.Tests.UnitTests.TestHelpers;
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
        private readonly Mock<IMailService> _mailServiceMock;

        public BookingRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DentalDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Booking_" + Guid.NewGuid().ToString())
                .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            _dbContext = new DentalDbContext(options);

            _userManagerMock = new Mock<UserManager<UserRegistration>>(
                new Mock<IUserStore<UserRegistration>>().Object, null, null, null, null, null, null, null, null);
            _configMock = new Mock<IConfiguration>();
            _mailServiceMock = new Mock<IMailService>();
        }

        [Fact]
        public async Task BookAppointmentAsync_ValidData_Success()
        {
            var patient = TestData.GetPatient();
            var doctor = TestData.GetDoctor();
            var procedure = TestData.GetProcedure();

            _dbContext.Users.AddRange(patient, doctor);
            _dbContext.RefDentalProcedures.Add(procedure);
            _dbContext.DoctorDentalProcedures.Add(TestData.GetDoctorProcedure(doctor.Id, procedure.Id));
            await _dbContext.SaveChangesAsync();

            var repoMock = new Mock<IBookingRepository>();
            repoMock.Setup(r => r.GetAvailableDoctorsAsync(procedure.Id, It.IsAny<DateTime>()))
                .ReturnsAsync(new List<UserRegistration> { doctor });
            repoMock.Setup(r => r.BookAppointmentAsync(It.IsAny<BookingDto>(), patient.Id))
                .ReturnsAsync("Appointment booked successfully.");

            var bookingDto = new BookingDto
            {
                ProcedureId = procedure.Id,
                DoctorId = doctor.Id,
                AppointmentTime = DateTime.UtcNow.AddDays(1)
            };

            _userManagerMock.Setup(um => um.FindByIdAsync(patient.Id)).ReturnsAsync(patient);
            _userManagerMock.Setup(um => um.FindByIdAsync(doctor.Id)).ReturnsAsync(doctor);

            var result = await repoMock.Object.BookAppointmentAsync(bookingDto, patient.Id);

            Assert.Equal("Appointment booked successfully.", result);
        }

        [Fact]
        public async Task GetBookingStatsAsync_ReturnsStats()
        {
            var doctor = TestData.GetDoctor();
            var procedure = TestData.GetProcedure();
            _dbContext.Users.Add(doctor);
            _dbContext.RefDentalProcedures.Add(procedure);
            _dbContext.ProcedureRegistrationCards.Add(TestData.GetConfirmedBooking("patient1", doctor.Id, procedure.Id));
            await _dbContext.SaveChangesAsync();

            var repo = new BookingRepository(_dbContext, _userManagerMock.Object, _configMock.Object, _mailServiceMock.Object);

            var stats = await repo.GetBookingStatsAsync(doctor.Id, DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1), procedure.Id);

            Assert.Single(stats);
            Assert.Equal("John Smith", stats[0].DoctorName);
        }
    }
}