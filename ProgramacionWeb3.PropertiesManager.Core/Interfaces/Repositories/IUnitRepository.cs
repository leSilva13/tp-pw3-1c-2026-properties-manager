using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories
{
    public interface IUnitRepository : IBaseRepository<Unit>
    {
        Task<Unit?> GetByUnitIdWithOwnerAndBuildingAsync(int id);

        Task<IEnumerable<Unit>> GetByBuildingIdAsync(int buildingId);

        Task SaveAsync();
    }
}
