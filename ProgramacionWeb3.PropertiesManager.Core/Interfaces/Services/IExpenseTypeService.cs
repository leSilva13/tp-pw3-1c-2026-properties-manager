using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services
{
    public interface IExpenseTypeService
    {
        Task<IEnumerable<ExpenseType>> ObtenerTiposDeGastoAsync();
    }
}
