using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories
{
    public interface IOwnerRepository : IBaseRepository<Owner>
    {
        Task<Owner?> GetByEmailAsync(string email); 
    }
}
