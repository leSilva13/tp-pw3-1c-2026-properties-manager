using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories
{
    public interface IUnitRepository : IBaseRepository<Unit>
    {
        Task<IEnumerable<Unit>> GetByBuildingIdAsync(int buildingId);
    }
}
