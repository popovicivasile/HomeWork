using HomeWork.Controllers;
using HomeWork.Core.RefStaticList;
using HomeWork.Data.Domain.ValueObjects;
using HomeWork.Data.Repository.Abstract;
using HomeWork.Models.Booking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
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

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "patient1")
            }, "mock"));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
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
            var bookingDto = new BookingDto
            {
                ProcedureId = Guid.Parse(RefDentalProceduresList.C),
                DoctorId = "1",
                AppointmentTime = DateTime.UtcNow
            };

            _bookingRepoMock.Setup(repo => repo.BookAppointmentAsync(bookingDto, "patient1"))
                .ReturnsAsync("Appointment booked successfully.");

            var result = await _controller.BookAppointment(bookingDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as string; 

            Assert.NotNull(response);
            Assert.Equal("Appointment booked successfully.", response);
        }


    }
}