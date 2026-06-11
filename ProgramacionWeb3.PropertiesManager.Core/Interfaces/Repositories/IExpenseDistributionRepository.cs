using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories
{
    public interface IExpenseDistributionRepository : IBaseRepository<ExpenseDistribution>
    {
        Task<IEnumerable<ExpenseDistribution>> GetByExpenseIdAsync(int expenseId);
        Task DeleteByExpenseIdAsync(int expenseId);
    }
}
