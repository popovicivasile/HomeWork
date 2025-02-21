using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork.Models.Booking
{
    public class BookingDto
    {
        public Guid ProcedureId { get; set; }
        public string DoctorId { get; set; }
        public DateTime AppointmentTime { get; set; }
    }
}
