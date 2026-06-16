namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toAddress, string subject, string body);

        Task SendEmailAsync(string[] toAddress, string subject, string body);
    }
}
