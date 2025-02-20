using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork.Models.Users
{
    public class LoginInformationDto
    {
        public string Token { get; set; }
        public DateTimeOffset ExpirationTime { get; set; }
    }
}
