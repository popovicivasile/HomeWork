using HomeWork.Core.RefStaticList;
using HomeWork.Core.TimeJobs;
using HomeWork.Data;
using HomeWork.Data.Domain;
using HomeWork.Data.Domain.ValueObjects;
using HomeWork.Data.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using Microsoft.Extensions.Configuration;

namespace HomeWork.Tests.UnitTests.Jobs
{
    public class SendAppointmentReminderTests
    {
        [Fact]
        public async Task Execute_SendsRemindersForTomorrow()
        {
            var options = new DbContextOptionsBuilder<DentalDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
                .Options;
            var dbContext = new DentalDbContext(options);

            var tomorrow = DateTime.UtcNow.AddDays(1).Date;
            var patient = new UserRegistration
            {
                Email = "patient@example.com",
                FirstName = "Jane",
                LastName = "Doe",
                PhoneNumber = "123-456-7890" 
            };
            var doctor = new UserRegistration
            {
                FirstName = "John",
                LastName = "Smith",
                PhoneNumber = "987-654-3210" 
            };
            var procedure = new RefDentalProcedures { Name = "Checkup" };

            var appointment = new ProcedureRegistrationCard
            {
                Id = Guid.NewGuid(),
                PatientId = "patient1",
                Patient = patient,
                DoctorId = "doctor1",
                Doctor = doctor,
                ProcedureId = Guid.NewGuid(),
                Procedure = procedure,
                AppointmentTime = tomorrow,
                StatusId = Guid.Parse(RefStatusTypeList.Confirmed)
            };

            dbContext.ProcedureRegistrationCards.Add(appointment);
            await dbContext.SaveChangesAsync();

            var mailServiceMock = new Mock<IMailService>();
            var configMock = new Mock<IConfiguration>();

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(sp => sp.GetService(typeof(IConfiguration))).Returns(configMock.Object);
            serviceProviderMock.Setup(sp => sp.GetService(typeof(DentalDbContext))).Returns(dbContext);
            serviceProviderMock.Setup(sp => sp.GetService(typeof(IMailService))).Returns(mailServiceMock.Object);

            var scopeMock = new Mock<IServiceScope>();
            scopeMock.Setup(s => s.ServiceProvider).Returns(serviceProviderMock.Object);

            var scopeFactoryMock = new Mock<IServiceScopeFactory>();
            scopeFactoryMock.Setup(f => f.CreateScope()).Returns(scopeMock.Object);

            var job = new SendAppointmentReminder(scopeFactoryMock.Object);

            await job.Execute(null);

            mailServiceMock.Verify(ms => ms.SendAsync(
                "patient@example.com",
                "Appointment Reminder",
                It.Is<string>(body => body.Contains("Reminder:") && body.Contains("tomorrow"))),
                Times.Once());
        }
    }
}