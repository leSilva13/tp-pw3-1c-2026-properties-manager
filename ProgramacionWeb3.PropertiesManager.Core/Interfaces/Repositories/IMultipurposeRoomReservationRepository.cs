using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories
{
    public interface IMultipurposeRoomReservationRepository : IBaseRepository<MultipurposeRoomReservation>
    {
        Task<IEnumerable<MultipurposeRoomReservation>> GetAllByBuildingAsync(int buildingId);
    }
}
