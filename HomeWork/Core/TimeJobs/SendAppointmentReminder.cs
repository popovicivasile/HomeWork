using HomeWork.Core.RefStaticList;
using HomeWork.Data;
using HomeWork.Data.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace HomeWork.Core.TimeJobs
{
    public class SendAppointmentReminder : IJob
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public SendAppointmentReminder(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    var _dbContext = scope.ServiceProvider.GetRequiredService<DentalDbContext>();
                    MailService mailService = new MailService(config);
                    var tomorrow = DateTime.UtcNow.AddDays(1).Date;
                    var appointments = await _dbContext.ProcedureRegistrationCards
                        .Where(prc => prc.AppointmentTime.Date == tomorrow && prc.StatusId == Guid.Parse(RefStatusTypeList.Confirmed))
                        .Include(prc => prc.Patient)
                        .Include(prc => prc.Doctor)
                        .Include(prc => prc.Procedure)
                        .ToListAsync();

                    foreach (var appointment in appointments)
                    {
                        await mailService.SendAsync(appointment.Patient.Email, "Appointment Reminder",
                            $"Reminder: You have an appointment with Dr. {appointment.Doctor.FirstName} {appointment.Doctor.LastName} " +
                            $"for {appointment.Procedure.Name} tomorrow at {appointment.AppointmentTime}.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
