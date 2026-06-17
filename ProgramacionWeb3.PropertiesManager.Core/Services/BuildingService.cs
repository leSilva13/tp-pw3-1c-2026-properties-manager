using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProgramacionWeb3.PropertiesManager.Core.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly IBuildingRepository _buildingRepository;

        // Inyección directa del repositorio de Consorcios
        public BuildingService(IBuildingRepository buildingRepository)
        {
            _buildingRepository = buildingRepository;
        }

        public async Task<IEnumerable<Building>> ObtenerEdificiosPorUsuarioAsync(int userId)
        {
            return await _buildingRepository.GetBuildingsByUserIdWithRelationshipsAsync(userId);
        }

        public async Task<Building?> ObtenerEdificioPorIdAsync(int id)
        {
            return await _buildingRepository.GetBuildingByIdWithRelationshipsAsync(id);
        }

        public async Task<int> CrearEdificioAsync(BuildingDto buildingDto)
        {
            var building = new Building
            {
                ExpensesDueDate = buildingDto.ExpensesDueDate,
                TotalSurfaceArea = buildingDto.TotalSurfaceArea,
                UserId = buildingDto.AdministratorUserId,
                Address = new Address
                {
                    Street = buildingDto.AddressDto.Street,
                    Number = buildingDto.AddressDto.Number,
                    ProvinceId = buildingDto.AddressDto.ProvinceId,
                    LocalityId = buildingDto.AddressDto.LocalityId,
                    Latitude = buildingDto.AddressDto.Latitude,   
                    Longitude = buildingDto.AddressDto.Longitude
                }
            };

            await _buildingRepository.AddAsync(building);
            await _buildingRepository.SaveAsync();

            return building.Id; // EF popula el Id tras el SaveChangesAsync
        }

        public async Task ActualizarEdificioAsync(BuildingDto buildingDto, int addressId)
        {
            var building = new Building
            {
                Id = buildingDto.Id, // Le dice a EF qué edificio editar
                ExpensesDueDate = buildingDto.ExpensesDueDate,
                TotalSurfaceArea = buildingDto.TotalSurfaceArea,
                UserId = buildingDto.AdministratorUserId,
                IsActive = buildingDto.IsActive,

                AddressId = addressId, // Clave foránea del edificio
                Address = new Address
                {
                    Id = addressId,   //Clave primaria de la dirección. ¡Evita el duplicado!
                    Street = buildingDto.AddressDto.Street,
                    Number = buildingDto.AddressDto.Number,
                    ProvinceId = buildingDto.AddressDto.ProvinceId,
                    LocalityId = buildingDto.AddressDto.LocalityId,
                    Latitude = buildingDto.AddressDto.Latitude,
                    Longitude = buildingDto.AddressDto.Longitude
                }
            };

            // Le avisamos al repositorio/DbContext que el objeto cambió
            _buildingRepository.Update(building);
            await _buildingRepository.SaveAsync();
        }

        public async Task<bool> BajaDeEdificioAsync(int id)
        {
            var edificio = await _buildingRepository.GetByIdAsync(id);

            if (edificio == null)
                return false;

            edificio.IsActive = false;

            await _buildingRepository.UpdateAsync(edificio);
            await _buildingRepository.SaveAsync();

            return true;
        }

        public async Task<bool> AltaDeEdificioAsync(int id)
        {
            var edificio = await _buildingRepository.GetByIdAsync(id);

            if (edificio == null)
                return false;

            edificio.IsActive = true;

            await _buildingRepository.UpdateAsync(edificio);
            await _buildingRepository.SaveAsync();

            return true;
        }
    }
}
