using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services
{
    public interface IAccountService
    {
        Task<(bool Success, string? Error)> RegisterOwnerAsync(string fullName, string email, string password);
        Task<User?> LogicAsync(string email, string password);
    }
}
