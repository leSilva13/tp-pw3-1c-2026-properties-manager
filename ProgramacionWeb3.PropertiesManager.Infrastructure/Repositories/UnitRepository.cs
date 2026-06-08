using Microsoft.EntityFrameworkCore;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Repositories
{
    public class UnitRepository : BaseRepository<Unit>, IUnitRepository
    {
        public UnitRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Unit>> GetByBuildingIdAsync(int buildingId)
        {
            return await _dbContext.Units
                .Where(u => u.BuildingId == buildingId)
                .ToListAsync();
        }
    }
}
