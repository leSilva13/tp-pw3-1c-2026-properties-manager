using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;

namespace ProgramacionWeb3.PropertiesManager.Core.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly IBuildingRepository _buildingRepository;

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

        public async Task CrearEdificioAsync(Building building)
        {
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
    }
}
