using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services
{
    public interface IBuildingService
    {
        Task<IEnumerable<Building>> ObtenerEdificiosPorUsuarioAsync(int userId);
        Task<Building?> ObtenerEdificioPorIdAsync(int id);
        Task<int> CrearEdificioAsync(BuildingDto buildingDto);
        Task ActualizarEdificioAsync(BuildingDto building, int addressId);
        Task EliminarEdificioAsync(int id);
        Task<bool> BajaDeEdificioAsync(int id);
    }
}
