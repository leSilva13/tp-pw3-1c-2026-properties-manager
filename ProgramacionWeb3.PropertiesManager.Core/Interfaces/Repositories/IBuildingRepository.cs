using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories
{
    public interface IBuildingRepository : IBaseRepository<Building>
    {
        Task<Building?> GetByIdWithAddressAsync(int id);
    }
}
