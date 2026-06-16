using Microsoft.EntityFrameworkCore;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Repositories
{
    public class ExpenseDistributionRepository : BaseRepository<ExpenseDistribution>, IExpenseDistributionRepository
    {
        public ExpenseDistributionRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<ExpenseDistribution>> GetByExpenseIdAsync(int expenseId)
        {
            return await _dbContext.ExpenseDistributions
                .Where(ed => ed.ExpenseId == expenseId)
                .ToListAsync();
        }
    }
}
