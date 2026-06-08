using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<(bool Success, string? Error)> RegisterOwnerAsync(string fullName, string email, string password);
        Task<User?> LogicAsync(string email, string password);
    }
}
