using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Repositories
{
    public class ExpenseTypeRepository : BaseRepository<ExpenseType>, IExpenseTypeRepository
    {
        public ExpenseTypeRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
