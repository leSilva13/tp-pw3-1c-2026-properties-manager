using ProgramacionWeb3.PropertiesManager.Core.DTOs;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(UserDto userDto);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
    }
}
