using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;

namespace ProgramacionWeb3.PropertiesManager.Core.Services
{
    public class ExpenseTypeService : IExpenseTypeService
    {
        private readonly IExpenseTypeRepository _expenseTypeRepository;

        public ExpenseTypeService(IExpenseTypeRepository expenseTypeRepository)
        {
            _expenseTypeRepository = expenseTypeRepository;
        }

        public async Task<IEnumerable<ExpenseTypeDto>> ObtenerTiposDeGastoAsync()
        {
            var tipos = await _expenseTypeRepository.GetAllAsync();
            return tipos.Select(t => new ExpenseTypeDto { Id = t.Id, Name = t.Name }).ToList();
        }
    }
}
