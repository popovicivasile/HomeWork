using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork.Models.Users
{
    public class DoctorDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(3)]
        public List<Guid> RefDentalProcedureId { get; set; } = new List<Guid>();
    }
}
