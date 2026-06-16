using ProgramacionWeb3.PropertiesManager.Core.Common.Enums;
using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories
{
    public interface IMultipurposeRoomReservationRepository : IBaseRepository<MultipurposeRoomReservation>
    {
        Task<IEnumerable<MultipurposeRoomReservation>> GetAllByBuildingAsync(int buildingId);

        Task<MultipurposeRoomReservation?> GetByRoomReservationDateAndShiftAsync(int multipurposeRoomId, DateOnly reservationDate, MultipurposeRoomShiftEnum multipurposeRoomShiftEnum);

        Task<MultipurposeRoomReservation?> GetNextReservationAsync(int multipurposeRoomId, DateOnly reservationDate, MultipurposeRoomShiftEnum multipurposeRoomShiftEnum);

        Task<MultipurposeRoomReservation?> GetPreviousReservationAsync(int multipurposeRoomId, DateOnly reservationDate, MultipurposeRoomShiftEnum multipurposeRoomShiftEnum);

        Task<MultipurposeRoomReservation?> GetFullByIdAsync(int multipurposeRoomReservationId);
    }
}
