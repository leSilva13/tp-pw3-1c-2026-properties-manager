using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories
{
    internal interface IBuildingRepository : IBaseRepository<Building>
    {
        Task<IEnumerable<Building>> GetBuildingsByUserIdWithRelationshipsAsync(int userId);

        Task<Building?> GetBuildingByIdWithRelationshipsAsync(int id);
    }
}