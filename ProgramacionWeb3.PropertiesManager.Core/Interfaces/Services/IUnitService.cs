using ProgramacionWeb3.PropertiesManager.Core.DTOs;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services
{
    public interface IUnitService
    {
        Task<IEnumerable<UnitDto>> ObtenerUnidadesPorConsorcioAsync(int buildingId);

        Task<UnitDto?> ObtenerUnidadParaEditarAsync(int unitId);

        Task CrearUnidadAsync(UnitDto unitDto);

        Task ActualizarUnidadAsync(UnitDto unitDto);

        Task EliminarUnidadAsync(int unitId);
    }
}