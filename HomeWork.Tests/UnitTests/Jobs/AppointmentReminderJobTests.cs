using HomeWork.Core.RefStaticList;
using HomeWork.Core.TimeJobs;
using HomeWork.Data;
using HomeWork.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace HomeWork.Tests.UnitTests.Jobs
{
    public class SendAppointmentReminderTests
    {
        [Fact]
        public async Task Execute_SendsRemindersForTomorrow()
        {
            var options = new DbContextOptionsBuilder<DentalDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Job")
                .Options;
            var dbContext = new DentalDbContext(options);
            var configMock = new Mock<IConfiguration>();
            var mailService = new Mock<MailService>(configMock.Object);
            var scopeMock = new Mock<IServiceScope>();
            scopeMock.Setup(s => s.ServiceProvider.GetService(typeof(DentalDbContext))).Returns(dbContext);
            scopeMock.Setup(s => s.ServiceProvider.GetService(typeof(IConfiguration))).Returns(configMock.Object);
            scopeMock.Setup(s => s.ServiceProvider.GetService(typeof(MailService))).Returns(mailService.Object);

            var scopeFactoryMock = new Mock<IServiceScopeFactory>();
            scopeFactoryMock.Setup(f => f.CreateScope()).Returns(scopeMock.Object);

            var job = new SendAppointmentReminder(scopeFactoryMock.Object);
            dbContext.ProcedureRegistrationCards.Add(new ProcedureRegistrationCard
            {
                PatientId = "1",
                DoctorId = "2",
                ProcedureId = Guid.NewGuid(),
                AppointmentTime = DateTime.UtcNow.AddDays(1),
                StatusId = Guid.Parse(RefStatusTypeList.Confirmed)
            });
            await dbContext.SaveChangesAsync();

            // Act
            await job.Execute(null);

            // Assert
            mailService.Verify(ms => ms.SendAsync(It.IsAny<string>(), "Appointment Reminder", It.IsAny<string>()), Times.Once);
        }
    }
}