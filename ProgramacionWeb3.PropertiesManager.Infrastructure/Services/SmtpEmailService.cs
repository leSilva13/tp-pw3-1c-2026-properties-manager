using Microsoft.Extensions.Configuration;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using ProgramacionWeb3.PropertiesManager.Infrastructure.DTOs;
using System.Net;
using System.Net.Mail;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly EmailConfig _emailConfig;

        public SmtpEmailService(IConfiguration config)
        {
            _emailConfig = new EmailConfig
            {
                Host = config["EmailSettings:Host"],
                Port = int.Parse(config["EmailSettings:Port"]),
                FromAddress = config["EmailSettings:FromAddress"],
                Username = config["EmailSettings:Username"],
                Password = config["EmailSettings:Password"]
            };
        }

        public async Task SendEmailAsync(string toAddress, string subject, string body)
        {
            using var client = new SmtpClient(_emailConfig.Host, _emailConfig.Port)
            {
                Credentials = new NetworkCredential(_emailConfig.Username, _emailConfig.Password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailConfig.FromAddress, "Properties Manager System"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toAddress);

            await client.SendMailAsync(mailMessage);
        }

        public async Task SendEmailAsync(string[] toAddress, string subject, string body)
        {
            using var client = new SmtpClient(_emailConfig.Host, _emailConfig.Port)
            {
                Credentials = new NetworkCredential(_emailConfig.Username, _emailConfig.Password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailConfig.FromAddress, "Properties Manager System"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(string.Join(",", toAddress));

            await client.SendMailAsync(mailMessage);
        }
    }
}
