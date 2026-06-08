using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;

namespace ProgramacionWeb3.PropertiesManager.Core.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IUnitOfWork _uow;

        public ExpenseService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<Expense>> GetExpensesByBuildingAsync(int buildingId)
        {
            return await _uow.Expenses.GetByBuildingIdAsync(buildingId);
        }

        public async Task<Expense?> GetExpenseByIdAsync(int id)
        {
            return await _uow.Expenses.GetByIdWithRelationshipsAsync(id);
        }

        public async Task CreateExpenseAsync(Expense expense)
        {
            expense.CreationDate = DateTime.Now;
            await _uow.Expenses.AddAsync(expense);
            await _uow.SaveAsync();
        }

        public async Task UpdateExpenseAsync(Expense expense)
        {
            _uow.Expenses.Update(expense);
            await _uow.SaveAsync();
        }

        public async Task DeleteExpenseAsync(int id)
        {
            var expense = await _uow.Expenses.GetByIdAsync(id);
            if (expense != null)
            {
                _uow.Expenses.Delete(expense);
                await _uow.SaveAsync();
            }
        }
    }
}
