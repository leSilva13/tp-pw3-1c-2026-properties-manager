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

        public async Task CrearEdificioAsync(BuildingDto buildingDto)
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
                }
            };

            await _buildingRepository.AddAsync(building);
            await _buildingRepository.SaveAsync();
        }

        public async Task ActualizarEdificioAsync(Building building)
        {
            _buildingRepository.Update(building);
            await _buildingRepository.SaveAsync();
        }

        public async Task EliminarEdificioAsync(int id)
        {
            var building = await _buildingRepository.GetByIdAsync(id);
            if (building != null)
            {
                _buildingRepository.Delete(building);
                await _buildingRepository.SaveAsync();
            }
        }

        public async Task<bool> BajaDeEdificioAsync(int id)
        {
            // 1. Buscamos el edificio por su ID usando el repositorio
            var edificio = await _buildingRepository.GetByIdAsync(id); // O el método que uses para buscar por ID

            if (edificio == null)
            {
                return false; // Retornamos false si el edificio no existe
            }

            // 2. Aplicamos la lógica de negocio del "Borrado Lógico"
            edificio.IsActive = false;

            // 3. Actualizamos y guardamos el cambio en la base de datos
            await _buildingRepository.UpdateAsync(edificio);
            await _buildingRepository.SaveAsync();

            return true;
        }        
    }
}
