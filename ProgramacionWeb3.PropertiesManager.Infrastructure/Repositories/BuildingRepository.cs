using Microsoft.EntityFrameworkCore;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Repositories
{
    public class BuildingRepository : BaseRepository<Building>, IBuildingRepository
    {
        public BuildingRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<Building?> GetByIdWithAddressAsync(int id)
        {
            return await _dbContext.Set<Building>()
                .Include(b => b.Address)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
