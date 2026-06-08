using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services
{
    public interface IExpenseService
    {
        Task<IEnumerable<Expense>> GetExpensesByBuildingAsync(int buildingId);
        Task<Expense?> GetExpenseByIdAsync(int id);
        Task CreateExpenseAsync(Expense expense);
        Task UpdateExpenseAsync(Expense expense);
        Task DeleteExpenseAsync(int id);
    }
}
