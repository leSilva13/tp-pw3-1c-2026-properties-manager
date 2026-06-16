using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories
{
    public interface IMultipurposeRoomRepository : IBaseRepository<MultipurposeRoom>
    {
        Task<IEnumerable<MultipurposeRoom>> GetAllByBuildingAsync(int buildingId);
    }
}
