using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;

namespace ProgramacionWeb3.PropertiesManager.Core.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IUnitOfWork _uow;
        private readonly IExpenseRepository _expenseRepository;
        private readonly IExpenseDistributionRepository _expenseDistributionRepository;

        public ExpenseService(IUnitOfWork uow, IExpenseRepository expenseRepository, IExpenseDistributionRepository expenseDistributionRepository)
        {
            _uow = uow;
            _expenseRepository = expenseRepository;
            _expenseDistributionRepository = expenseDistributionRepository;
        }

        public async Task<IEnumerable<Expense>> ObtenerGastosPorEdificioAsync(int buildingId)
        {
            return await _expenseRepository.GetByBuildingIdAsync(buildingId);
        }

        public async Task<Expense?> ObtenerGastoPorIdAsync(int id)
        {
            return await _expenseRepository.GetByIdWithRelationshipsAsync(id);
        }

        public async Task CrearGastoAsync(Expense expense, List<int>? unitIds = null)
        {
            expense.CreationDate = DateTime.Now;
            await _expenseRepository.AddAsync(expense);
            await _uow.SaveChangesAsync();

            if (!expense.IsGlobal && unitIds != null && unitIds.Any())
            {
                foreach (var unitId in unitIds)
                {
                    await _expenseDistributionRepository.AddAsync(new ExpenseDistribution
                    {
                        ExpenseId = expense.Id,
                        UnitId = unitId
                    });
                }
                await _uow.SaveChangesAsync();
            }
        }

        public async Task ActualizarGastoAsync(Expense expense, List<int>? unitIds = null)
        {
            _expenseRepository.Update(expense);

            await _expenseDistributionRepository.DeleteByExpenseIdAsync(expense.Id);

            if (!expense.IsGlobal && unitIds != null && unitIds.Any())
            {
                foreach (var unitId in unitIds)
                {
                    await _expenseDistributionRepository.AddAsync(new ExpenseDistribution
                    {
                        ExpenseId = expense.Id,
                        UnitId = unitId
                    });
                }
            }

            await _uow.SaveChangesAsync();
        }

        public async Task EliminarGastoAsync(int id)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);
            if (expense != null)
            {
                _expenseRepository.Delete(expense);
                await _uow.SaveChangesAsync();
            }
        }
    }
}
