using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;

namespace ProgramacionWeb3.PropertiesManager.Core.Services
{
    public class MultipurposeRoomReservationService : IMultipurposeRoomReservationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMultipurposeRoomReservationRepository _multipurposeRoomReservationRepository;

        public MultipurposeRoomReservationService(IUnitOfWork unitOfWork, IMultipurposeRoomReservationRepository multipurposeRoomReservationRepository)
        {
            _unitOfWork = unitOfWork;
            _multipurposeRoomReservationRepository = multipurposeRoomReservationRepository;
        }

        public async Task<BaseResponseDto> CreateAsync(MultipurposeRoomReservationDto multipurposeRoomReservationDto)
        {
            try
            {
                var multipurposeRoomReservation = new MultipurposeRoomReservation
                {
                    Comments = multipurposeRoomReservationDto.Comments,
                    MultipurposeRoomId = multipurposeRoomReservationDto.MultipurposeRoomDto.Id,
                    ReservationDate = multipurposeRoomReservationDto.ReservationDate,
                    Shift = multipurposeRoomReservationDto.Shift,
                    UnitId = multipurposeRoomReservationDto.UnitId,
                    WasWellDelivered = false,
                };

                await _multipurposeRoomReservationRepository.AddAsync(multipurposeRoomReservation);
                await _unitOfWork.SaveChangesAsync();

                return new BaseResponseDto { Id = multipurposeRoomReservation.Id, IsSuccess = true, Message = "Exito." };
            }
            catch (Exception)
            {
                return new BaseResponseDto { IsSuccess = false, Message = "Ocurrió un error. Intente nuevamente." };
            }
        }

        public async Task<MultipurposeRoomReservationDto> GetByIdAsync(int multipurposeRoomReservationId)
        {
            var multipurposeRoomReservation = await _multipurposeRoomReservationRepository.GetByIdAsync(multipurposeRoomReservationId);
            if (multipurposeRoomReservation == null)
                return null;
            
            return new MultipurposeRoomReservationDto
            {
                Id = multipurposeRoomReservation.Id,
                Comments = multipurposeRoomReservation.Comments,
                ReservationDate = multipurposeRoomReservation.ReservationDate,
                Shift= multipurposeRoomReservation.Shift,
                UnitId = multipurposeRoomReservation.UnitId, 
                WasWellDelivered = multipurposeRoomReservation.WasWellDelivered,
                MultipurposeRoomDto = new MultipurposeRoomDto 
                { 
                    Id = multipurposeRoomReservation.Unit.Id 
                }
            };
        }

        public async Task<IEnumerable<MultipurposeRoomReservationDto>> GetAllByBuildingAsync(int buildingId)
        {
            var multipurposeRoomReservations = await _multipurposeRoomReservationRepository.GetAllByBuildingAsync(buildingId);
            return multipurposeRoomReservations.Select(mrr =>
                new MultipurposeRoomReservationDto
                {
                    Id = mrr.Id,
                    Comments = mrr.Comments,
                    ReservationDate = mrr.ReservationDate,
                    Shift = mrr.Shift,
                    UnitId = mrr.UnitId,
                    WasWellDelivered = mrr.WasWellDelivered,
                    MultipurposeRoomDto = new MultipurposeRoomDto
                    {
                        Id = mrr.Unit.Id
                    }
                }).ToList();
        }

        public async Task<BaseResponseDto> UpdateAsync(MultipurposeRoomReservationDto multipurposeRoomReservationDto)
        {
            try
            {
                var multipurposeRoomReservation = new MultipurposeRoomReservation
                {
                    Id = multipurposeRoomReservationDto.Id,
                    Comments = multipurposeRoomReservationDto.Comments,
                    ReservationDate = multipurposeRoomReservationDto.ReservationDate,
                    Shift = multipurposeRoomReservationDto.Shift,
                    UnitId = multipurposeRoomReservationDto.UnitId,
                    WasWellDelivered = multipurposeRoomReservationDto.WasWellDelivered,
                };

                _multipurposeRoomReservationRepository.Update(multipurposeRoomReservation);
                await _unitOfWork.SaveChangesAsync();

                return new BaseResponseDto { Id = multipurposeRoomReservation.Id, IsSuccess = true, Message = "Exito." };
            }
            catch (Exception)
            {
                return new BaseResponseDto { IsSuccess = false, Message = "Ocurrió un error. Intente nuevamente." };
            }
        }
    }
}
