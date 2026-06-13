using Microsoft.Extensions.Configuration;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using System.Net;
using System.Net.Mail;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Services
{
    public class SmtpEmailService : IEmailService
    {

        private readonly IConfiguration _config;

        public SmtpEmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toAddress, string subject, string body)
        {
            var host = _config["EmailSettings:Host"];
            var port = int.Parse(_config["EmailSettings:Port"]);
            var username = _config["EmailSettings:Username"];
            var password = _config["EmailSettings:Password"];
            var fromAddress = _config["EmailSettings:FromAddress"];

            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromAddress, "Properties Manager System"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toAddress);

            await client.SendMailAsync(mailMessage);
        }
    }
}
