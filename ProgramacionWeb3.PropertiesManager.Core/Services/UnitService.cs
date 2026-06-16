using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;

namespace ProgramacionWeb3.PropertiesManager.Core.Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitRepository _unitRepository;
        private readonly IOwnerRepository _ownerRepository;

        public UnitService(IUnitOfWork unitOfWork, IUnitRepository unitRepository,IOwnerRepository ownerRepository)
        {
            _unitOfWork = unitOfWork;
            _unitRepository = unitRepository;
            _ownerRepository = ownerRepository;
        }

        public async Task<IEnumerable<UnitDto>> ObtenerUnidadesPorConsorcioAsync(int buildingId)
        {
            var units = await _unitRepository.GetByBuildingIdAsync(buildingId);

            var unitsDto = units.Select(unit => new UnitDto
            {
                Id = unit.Id,
                BuildingId = unit.BuildingId,
                BuildingName = obtenerUnitName(unit),
                Floor = unit.Floor,
                ApartmentNumber = unit.ApartmentNumber,
                AreaInMeters = unit.AreaInMeters,
                OwnerId = unit.OwnerId,
                OwnerFullName = unit.Owner?.FullName,
                OwnerEmail = unit.Owner?.Email
            });

            return unitsDto;
        }

        public async Task<UnitDto?> ObtenerUnidadParaEditarAsync(int unitId)
        {
            var unitEntity = await _unitRepository.GetByUnitIdWithOwnerAndBuildingAsync(unitId);

            if (unitEntity == null)
            {
                return null;
            }

            string buildingName = obtenerUnitName(unitEntity);

            return new UnitDto
            {
                Id = unitEntity.Id,
                BuildingId = unitEntity.BuildingId,
                BuildingName = buildingName,
                Floor = unitEntity.Floor,
                ApartmentNumber = unitEntity.ApartmentNumber,
                AreaInMeters = unitEntity.AreaInMeters,
                OwnerId = unitEntity.OwnerId
            };
        }
        public async Task<(bool Success, string? Error)> CrearUnidadAsync(UnitDto unitDto)
        {
            var unit = new Unit
            {
                BuildingId = unitDto.BuildingId,
                Floor = unitDto.Floor,
                ApartmentNumber = unitDto.ApartmentNumber,
                AreaInMeters = unitDto.AreaInMeters
            };

            if (unitDto.NuevoOwner != null)
            {
                var ownerExistente = await _ownerRepository.GetByEmailAsync(unitDto.NuevoOwner.Email);

                if (ownerExistente != null)
                {
                    return (false, "Ya existe un Owner con ese email");
                }

                var nuevoOwner = new Owner
                {
                    FullName = unitDto.NuevoOwner.FullName,
                    Email = unitDto.NuevoOwner.Email,
                    IsActive = true
                };

                await _ownerRepository.AddAsync(nuevoOwner);

                unit.Owner = nuevoOwner;
            }
            else
            {
                if (!unitDto.OwnerId.HasValue)
                {
                    return (false, "Debe seleccionar o crear un dueño");
                }

                unit.OwnerId = unitDto.OwnerId;
            }

            await _unitRepository.AddAsync(unit);
            await _unitOfWork.SaveChangesAsync();

            return (true, null);
        }

        public async Task ActualizarUnidadAsync(UnitDto unitDto)
        {
            var unit = await _unitRepository.GetByIdAsync(unitDto.Id);

            if (unit == null)
            {
                return;
            }
            
            unit.BuildingId = unitDto.BuildingId;
            unit.Floor = unitDto.Floor;
            unit.ApartmentNumber = unitDto.ApartmentNumber;
            unit.AreaInMeters = unitDto.AreaInMeters;
            unit.OwnerId = unitDto.OwnerId;

            _unitRepository.Update(unit);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task EliminarUnidadAsync(int unitId)
        {
            var unit = await _unitRepository.GetByIdAsync(unitId);

            if (unit == null)
                return;

            unit.IsActive = false;

            _unitRepository.Update(unit);
            await _unitOfWork.SaveChangesAsync();
        }

        private string obtenerUnitName(Unit unit)
        {
            string buildingName;

            if (unit.Building != null && unit.Building.Address != null)
            {
               return buildingName = unit.Building.Address.Street + " " + unit.Building.Address.Number;
            }
            else
            {
                return buildingName = "Edificio #" + unit.BuildingId;
            }
        }
    }
}
