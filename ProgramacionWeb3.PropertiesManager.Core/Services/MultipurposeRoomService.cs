using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;

namespace ProgramacionWeb3.PropertiesManager.Core.Services
{
    public class MultipurposeRoomService : IMultipurposeRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMultipurposeRoomRepository _multipurposeRoomRepository;

        public MultipurposeRoomService(IUnitOfWork unitOfWork, IMultipurposeRoomRepository multipurposeRoomRepository)
        {
            _unitOfWork = unitOfWork;
            _multipurposeRoomRepository = multipurposeRoomRepository;
        }

        public async Task<BaseResponseDto> CreateAsync(MultipurposeRoomDto multipurposeRoomDto)
        {
            try
            {
                var multipurposeRoom = new MultipurposeRoom
                {
                    Name = multipurposeRoomDto.Name,
                    Floor = multipurposeRoomDto.Floor,
                    BuildingId = multipurposeRoomDto.BuildingId,
                };

                await _multipurposeRoomRepository.AddAsync(multipurposeRoom);
                await _unitOfWork.SaveChangesAsync();

                return new BaseResponseDto { Id = multipurposeRoom.Id, IsSuccess = true, Message = "Exito." };
            }
            catch (Exception)
            {
                return new BaseResponseDto { IsSuccess = false, Message = "Ocurrió un error. Intente nuevamente." };
            }
        }

        public async Task<MultipurposeRoomDto> GetByIdAsync(int multipurposeRoomId)
        {
            var multipurposeRoom = await _multipurposeRoomRepository.GetByIdAsync(multipurposeRoomId);
            return new MultipurposeRoomDto 
            { 
                Id = multipurposeRoom.Id, 
                Name = multipurposeRoom.Name,
                Floor = multipurposeRoom.Floor,
                IsActive = multipurposeRoom.IsActive,
                BuildingId = multipurposeRoom.BuildingId
            };
        }

        public async Task<IEnumerable<MultipurposeRoomDto>> GetAllByBuildingAsync(int buildingId)
        {
            var multipurposeRooms = await _multipurposeRoomRepository.GetAllByBuildingAsync(buildingId);
            return multipurposeRooms.Select(mr =>
                new MultipurposeRoomDto
                {
                    Id = mr.Id,
                    Name = mr.Name,
                    Floor = mr.Floor,
                    BuildingId = mr.BuildingId,
                    IsActive = mr.IsActive,
                }).ToList();
        }

        public async Task<BaseResponseDto> UpdateAsync(MultipurposeRoomDto multipurposeRoomDto)
        {
            try
            {
                var multipurposeRoom = new MultipurposeRoom
                {
                    Id = multipurposeRoomDto.Id,
                    Name = multipurposeRoomDto.Name,
                    Floor = multipurposeRoomDto.Floor,
                    IsActive = multipurposeRoomDto.IsActive,
                    BuildingId = multipurposeRoomDto.BuildingId,
                };

                _multipurposeRoomRepository.Update(multipurposeRoom);
                await _unitOfWork.SaveChangesAsync();

                return new BaseResponseDto { Id = multipurposeRoom.Id, IsSuccess = true, Message = "Exito." };
            }
            catch (Exception)
            {
                return new BaseResponseDto { IsSuccess = false, Message = "Ocurrió un error. Intente nuevamente." };
            }
        }
    }
}
