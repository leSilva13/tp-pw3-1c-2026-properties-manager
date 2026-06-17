using ProgramacionWeb3.PropertiesManager.Core.DTOs;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<BaseResponseDto> CreateUserAsync(UserDto userDto);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();

        Task DeleteUserAsync(int id);
        Task ActivateUserAsync(int id);
    }
}
