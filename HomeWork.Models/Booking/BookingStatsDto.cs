using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork.Models.Booking
{
    public class BookingStatsDto
    {
        public string DoctorName { get; set; }
        public string ProcedureName { get; set; }
        public int AppointmentCount { get; set; }
        public int TotalDuration { get; set; }
        public int CancelledCount { get; set; }
        public DateTimeOffset PeriodStart { get; set; }
        public DateTimeOffset PeriodEnd { get; set; }
    }
}
