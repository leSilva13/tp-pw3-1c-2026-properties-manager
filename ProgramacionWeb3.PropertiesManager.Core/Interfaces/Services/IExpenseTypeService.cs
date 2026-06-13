using ProgramacionWeb3.PropertiesManager.Core.DTOs;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services
{
    public interface IExpenseTypeService
    {
        Task<IEnumerable<ExpenseTypeDto>> ObtenerTiposDeGastoAsync();
    }
}
