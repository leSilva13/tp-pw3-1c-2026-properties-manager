using ProgramacionWeb3.PropertiesManager.Core.DTOs;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services
{
    public interface IMultipurposeRoomReservationService
    {
        Task<BaseResponseDto> CreateAsync(MultipurposeRoomReservationDto multipurposeRoomReservationDto);

        Task<MultipurposeRoomReservationDto> GetByIdAsync(int multipurposeRoomReservationId);

        Task<IEnumerable<MultipurposeRoomReservationDto>> GetAllByBuildingAsync(int buildingId);

        Task<BaseResponseDto> UpdateAsync(MultipurposeRoomReservationDto multipurposeRoomReservationDto);
    }
}
