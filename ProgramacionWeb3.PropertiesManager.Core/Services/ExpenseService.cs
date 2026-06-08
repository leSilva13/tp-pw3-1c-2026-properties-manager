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

        public async Task<IEnumerable<Expense>> ObtenerGastosPorEdificioAsync(int buildingId)
        {
            return await _uow.Expenses.GetByBuildingIdAsync(buildingId);
        }

        public async Task<Expense?> ObtenerGastoPorIdAsync(int id)
        {
            return await _uow.Expenses.GetByIdWithRelationshipsAsync(id);
        }

        public async Task CrearGastoAsync(Expense expense, List<int>? unitIds = null)
        {
            expense.CreationDate = DateTime.Now;
            await _uow.Expenses.AddAsync(expense);
            await _uow.SaveAsync();

            if (!expense.IsGlobal && unitIds != null && unitIds.Any())
            {
                foreach (var unitId in unitIds)
                {
                    await _uow.ExpenseDistributions.AddAsync(new ExpenseDistribution
                    {
                        ExpenseId = expense.Id,
                        UnitId = unitId
                    });
                }
                await _uow.SaveAsync();
            }
        }

        public async Task ActualizarGastoAsync(Expense expense, List<int>? unitIds = null)
        {
            _uow.Expenses.Update(expense);

            await _uow.ExpenseDistributions.DeleteByExpenseIdAsync(expense.Id);

            if (!expense.IsGlobal && unitIds != null && unitIds.Any())
            {
                foreach (var unitId in unitIds)
                {
                    await _uow.ExpenseDistributions.AddAsync(new ExpenseDistribution
                    {
                        ExpenseId = expense.Id,
                        UnitId = unitId
                    });
                }
            }

            await _uow.SaveAsync();
        }

        public async Task EliminarGastoAsync(int id)
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
