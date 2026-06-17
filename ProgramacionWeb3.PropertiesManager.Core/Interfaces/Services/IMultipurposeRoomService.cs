using ProgramacionWeb3.PropertiesManager.Core.DTOs;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services
{
    public interface IMultipurposeRoomService
    {
        Task<BaseResponseDto> CreateAsync(MultipurposeRoomDto multipurposeRoomDto);

        Task<MultipurposeRoomDto> GetByIdAsync(int multipurposeRoomId);

        Task<IEnumerable<MultipurposeRoomDto>> GetAllByBuildingAsync(int buildingId);
        
        Task<BaseResponseDto> UpdateAsync(MultipurposeRoomDto multipurposeRoomDto);
    }
}
