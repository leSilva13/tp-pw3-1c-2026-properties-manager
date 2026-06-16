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
        private readonly IEmailService _emailService;
        private readonly IMultipurposeRoomReservationRepository _multipurposeRoomReservationRepository;

        public MultipurposeRoomReservationService(IUnitOfWork unitOfWork, IEmailService emailService, IMultipurposeRoomReservationRepository multipurposeRoomReservationRepository)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _multipurposeRoomReservationRepository = multipurposeRoomReservationRepository;
        }

        public async Task<BaseResponseDto> CreateAsync(MultipurposeRoomReservationDto multipurposeRoomReservationDto)
        {
            try
            {
                if (await _multipurposeRoomReservationRepository.GetByRoomReservationDateAndShiftAsync(multipurposeRoomReservationDto.MultipurposeRoomDto.Id, multipurposeRoomReservationDto.ReservationDate, multipurposeRoomReservationDto.Shift) != null)
                    return new BaseResponseDto { IsSuccess = false, Message = "El SUM ya se encuentra reservado para la fecha." };

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

                await SendEmailNotification(multipurposeRoomReservation.Id);

                return new BaseResponseDto { Id = multipurposeRoomReservation.Id, IsSuccess = true, Message = "Exito." };
            }
            catch (Exception)
            {
                return new BaseResponseDto { IsSuccess = false, Message = "Ocurrió un error. Intente nuevamente." };
            }
        }

        public async Task<MultipurposeRoomReservationDto> GetByIdAsync(int multipurposeRoomReservationId)
        {
            var multipurposeRoomReservation = await _multipurposeRoomReservationRepository.GetFullByIdAsync(multipurposeRoomReservationId);
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
                BuildingId = multipurposeRoomReservation.Unit.BuildingId,
                MultipurposeRoomDto = new MultipurposeRoomDto 
                { 
                    Id = multipurposeRoomReservation.MultipurposeRoomId,
                    Name = multipurposeRoomReservation.MultipurposeRoom.Name
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
                        Id = mrr.MultipurposeRoom.Id,
                        Name = mrr.MultipurposeRoom.Name
                    }
                }).ToList();
        }

        public async Task<BaseResponseDto> UpdateAsync(MultipurposeRoomReservationDto multipurposeRoomReservationDto)
        {
            try
            {

                if (await _multipurposeRoomReservationRepository.GetByRoomReservationDateAndShiftAsync(multipurposeRoomReservationDto.MultipurposeRoomDto.Id, multipurposeRoomReservationDto.ReservationDate, multipurposeRoomReservationDto.Shift) != null)
                    return new BaseResponseDto { IsSuccess = false, Message = "El SUM ya se encuentra reservado para la fecha." };

                var multipurposeRoomReservation = new MultipurposeRoomReservation
                {
                    Id = multipurposeRoomReservationDto.Id,
                    Comments = multipurposeRoomReservationDto.Comments,
                    ReservationDate = multipurposeRoomReservationDto.ReservationDate,
                    Shift = multipurposeRoomReservationDto.Shift,
                    UnitId = multipurposeRoomReservationDto.UnitId,
                    WasWellDelivered = multipurposeRoomReservationDto.WasWellDelivered,
                    MultipurposeRoomId = multipurposeRoomReservationDto.MultipurposeRoomDto.Id,
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

        public async Task<KeyHandoverInstructionsDto> GetKeyHandoverInstructionsAsync(int currentMultipurposeRoomReservationId)
        {
            var currentReservation = await _multipurposeRoomReservationRepository.GetByIdAsync(currentMultipurposeRoomReservationId);

            if (currentReservation == null)
                throw new Exception("No existe la reserva.");

            var previousTask = _multipurposeRoomReservationRepository.GetPreviousReservationAsync(
                currentReservation.MultipurposeRoomId, currentReservation.ReservationDate, currentReservation.Shift);

            var nextTask = _multipurposeRoomReservationRepository.GetNextReservationAsync(
                currentReservation.MultipurposeRoomId, currentReservation.ReservationDate, currentReservation.Shift);

            await Task.WhenAll(previousTask, nextTask);

            var prevReservation = previousTask.Result;
            var nextReservation = nextTask.Result;

            var dto = new KeyHandoverInstructionsDto();

            if (prevReservation != null)
            {
                dto.HasPreviousReservation = true;
                dto.PreviousOwnerEmail = prevReservation.Unit.Owner?.Email ?? string.Empty;
                dto.PreviousOwnerName = prevReservation.Unit.Owner?.FullName ?? string.Empty;
                dto.PreviousUnitNumber = prevReservation.Unit.Floor + prevReservation.Unit.ApartmentNumber;
                dto.PickUpInstructionMessage = $"Pedir las llaves a {dto.PreviousOwnerName} (Unidad {dto.PreviousUnitNumber}), quien tuvo el turno anterior.";
            }
            else
            {
                dto.HasPreviousReservation = false;
                dto.PickUpInstructionMessage = "Retirar las llaves en la guardia o administración.";
            }

            if (nextReservation != null)
            {
                dto.HasNextReservation = true;
                dto.NextOwnerEmail = nextReservation.Unit.Owner?.Email ?? string.Empty;
                dto.NextOwnerName = nextReservation.Unit.Owner?.FullName ?? string.Empty;
                dto.NextUnitNumber = nextReservation.Unit.Floor + nextReservation.Unit.ApartmentNumber;
                dto.DropOffInstructionMessage = $"Al finalizar, entregar las llaves a {dto.NextOwnerName} (Unidad {dto.NextUnitNumber}) que tiene el siguiente turno.";
            }
            else
            {
                dto.HasNextReservation = false;
                dto.DropOffInstructionMessage = "Al finalizar, devolver las llaves a la guardia o administración.";
            }

            return dto;
        }

        public async Task<BaseResponseDto> DeleteAsync(int multipurposeRoomReservationId)
        {
            var multipurposeRoomReservation = await _multipurposeRoomReservationRepository.GetByIdAsync(multipurposeRoomReservationId);
            if (multipurposeRoomReservation == null)
                return new BaseResponseDto { IsSuccess = false, Message = "Ocurrió un error. Intente nuevamente." };

            _multipurposeRoomReservationRepository.Delete(multipurposeRoomReservation);
            await _unitOfWork.SaveChangesAsync();

            return new BaseResponseDto { IsSuccess = true, Message = "Reserva eliminada." };
        }

        private async Task SendEmailNotification(int multipurposeRoomReservationId)
        {
            var multipurposeRoomReservation = await _multipurposeRoomReservationRepository.GetFullByIdAsync(multipurposeRoomReservationId);
            if (multipurposeRoomReservation == null)
                return;

            var keyHandoverInstructionsDto = await GetKeyHandoverInstructionsAsync(multipurposeRoomReservationId);

            var emailBody = $"Reserva del SUM {multipurposeRoomReservation.MultipurposeRoom.Name} para el dia {multipurposeRoomReservation.ReservationDate.ToString()} en el turno {multipurposeRoomReservation.Shift.ToString()}";

            emailBody += "<br>";
            emailBody += keyHandoverInstructionsDto.PickUpInstructionMessage;
            emailBody += "<br>";
            emailBody += keyHandoverInstructionsDto.DropOffInstructionMessage;

            var emailsToAddress = new[]
            {
                multipurposeRoomReservation.Unit.Owner?.Email,
                keyHandoverInstructionsDto.NextOwnerEmail,
                keyHandoverInstructionsDto.PreviousOwnerEmail
            }
            .Where(email => !string.IsNullOrWhiteSpace(email))
            .ToArray();

            if (emailsToAddress.Length > 0)
                await _emailService.SendEmailAsync(emailsToAddress, "Reserva de SUM", emailBody);
        }
    }
}
