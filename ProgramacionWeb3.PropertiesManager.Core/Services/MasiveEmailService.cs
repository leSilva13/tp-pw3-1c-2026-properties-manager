using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;

namespace ProgramacionWeb3.PropertiesManager.Core.Services
{
    public class MasiveEmailService : IMasiveEmailService
    {
        private readonly IConfiguration _configuration;

        // Inyectamos IConfiguration para leer los datos del appsettings.json
        public MasiveEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviarNotificacionMasivaAsync(List<string> destinatarios, string asunto, string cuerpo)
        {
            if (destinatarios == null || destinatarios.Count == 0) return;

            // Leemos la configuración mapeando exactamente con tu appsettings.json
            var smtpServer = _configuration["EmailSettings:Host"];

            // Parseo seguro del puerto
            if (!int.TryParse(_configuration["EmailSettings:Port"], out int smtpPort))
            {
                smtpPort = 587;
            }

            var senderEmail = _configuration["EmailSettings:Username"];
            var senderPassword = _configuration["EmailSettings:Password"];
            var fromAddress = _configuration["EmailSettings:FromAddress"];

            using var client = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true
            };

            using var mailMessage = new MailMessage
            {
                // Usamos fromAddress que también definiste en tu JSON
                From = new MailAddress(fromAddress, "Administración de Consorcios"),
                Subject = asunto,
                Body = cuerpo,
                IsBodyHtml = false
            };

            foreach (var email in destinatarios)
            {
                mailMessage.Bcc.Add(email);
            }

            await client.SendMailAsync(mailMessage);
        }
    }
}