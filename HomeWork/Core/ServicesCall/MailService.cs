using System.Net.Mail;
using System.Net;

public class MailService
{
    private readonly IConfiguration _config;

    public MailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendAsync(string address, string subject, string content)
    {
        try
        {
            var fromAddress = new MailAddress(_config["Email:FromAddress"], "From Name");
            var toAddress = new MailAddress(address, "To Name");
            var fromPassword = _config["Email:Password"];

            using var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = content
            };

            await smtp.SendMailAsync(message);
        }
        catch (SmtpException ex)
        {
            throw new Exception($"Failed to send email: {ex.Message}", ex);
        }
    }
}