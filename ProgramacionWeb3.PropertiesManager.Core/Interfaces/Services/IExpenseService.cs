using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services
{
    public interface IExpenseService
    {
        Task<IEnumerable<Expense>> ObtenerGastosPorEdificioAsync(int buildingId);
        Task<Expense?> ObtenerGastoPorIdAsync(int id);
        Task CrearGastoAsync(Expense expense, List<int>? unitIds = null);
        Task ActualizarGastoAsync(Expense expense, List<int>? unitIds = null);
        Task EliminarGastoAsync(int id);
    }
}
