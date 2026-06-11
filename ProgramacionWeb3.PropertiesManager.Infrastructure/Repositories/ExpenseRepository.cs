using Microsoft.EntityFrameworkCore;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Repositories
{
    public class ExpenseRepository : BaseRepository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Expense>> GetByBuildingIdAsync(int buildingId)
        {
            return await _dbContext.Expenses
                .Include(e => e.ExpenseType)
                .Where(e => e.BuildingId == buildingId)
                .OrderByDescending(e => e.BillingDate)
                .ToListAsync();
        }

        public async Task<Expense?> GetByIdWithRelationshipsAsync(int id)
        {
            return await _dbContext.Expenses
                .Include(e => e.ExpenseType)
                .Include(e => e.ExpenseDistributions)
                    .ThenInclude(ed => ed.Unit)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
