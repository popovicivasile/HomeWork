using System.Net.Mail;
using System.Net;

namespace HomeWork.Data.ServicesCall
{
    public class MailService
    {
        public async Task SendAsync(string Address, string subject, string content)
        {
            var fromAddress = new MailAddress("agstarnet@gmail.com", "From Name");
            var toAddress = new MailAddress(Address, "To Name");
            const string fromPassword = "RTKrtk!23";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = content
            })
            {
                await smtp.SendMailAsync(message);
            }
        }
    }
}
