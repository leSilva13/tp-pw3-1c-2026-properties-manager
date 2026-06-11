using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services
{
    public interface IUnitService
    {
        Task<IEnumerable<Unit>> ObtenerUnidadesPorConsorcioAsync(int buildingId);

        Task<Unit?> ObtenerUnidadParaEditarAsync(int unitId);

        Task CrearUnidadAsync(Unit unit);

        Task ActualizarUnidadAsync(Unit unit);

        Task EliminarUnidadAsync(int unitId);
    }
}