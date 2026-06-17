using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetActiveUserByEmailAsync(string email);
    }
}