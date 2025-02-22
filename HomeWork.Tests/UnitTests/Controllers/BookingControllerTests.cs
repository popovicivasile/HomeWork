using HomeWork.Controllers;
using HomeWork.Data.Domain.ValueObjects;
using HomeWork.Data.Repository.Abstract;
using HomeWork.Models.Booking;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HomeWork.Tests.UnitTests.Controllers
{
    public class BookingControllerTests
    {
        private readonly Mock<IBookingRepository> _bookingRepoMock;
        private readonly BookingController _controller;

        public BookingControllerTests()
        {
            _bookingRepoMock = new Mock<IBookingRepository>();
            _controller = new BookingController(_bookingRepoMock.Object);
        }

        [Fact]
        public async Task GetAllProcedures_ReturnsOk()
        {
            var procedures = new List<RefDentalProcedures> { new RefDentalProcedures { Name = "A" } };
            _bookingRepoMock.Setup(repo => repo.GetAllProceduresAsync()).ReturnsAsync(procedures);

            var result = await _controller.GetAllProcedures();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(procedures, okResult.Value);
        }

        [Fact]
        public async Task BookAppointment_ValidData_ReturnsOk()
        {
            var bookingDto = new BookingDto { ProcedureId = Guid.NewGuid(), DoctorId = "1", AppointmentTime = DateTime.UtcNow };
            _bookingRepoMock.Setup(repo => repo.BookAppointmentAsync(bookingDto, "patient1"))
                .ReturnsAsync("Appointment booked successfully.");

            // Act
            var result = await _controller.BookAppointment(bookingDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            dynamic response = okResult.Value;
            Assert.Equal("Appointment booked successfully.", response.Message);
        }
    }
}