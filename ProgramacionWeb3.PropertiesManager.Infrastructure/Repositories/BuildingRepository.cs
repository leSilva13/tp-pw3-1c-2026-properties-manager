using Microsoft.EntityFrameworkCore;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Repositories
{
    public class BuildingRepository : BaseRepository<Building>, IBuildingRepository
    {
        public BuildingRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Building>> GetBuildingsByUserIdWithRelationshipsAsync(int userId)
        {
            return await _dbContext.Buildings
                .Include(b => b.Address)
                    .ThenInclude(l => l.Province)
                .Where(b => b.UserId == userId)
                .OrderBy(b => b.Id)
                .ToListAsync();
        }

        public async Task<Building?> GetBuildingByIdWithRelationshipsAsync(int id)
        {
            return await _dbContext.Buildings
                .Include(b => b.Address)
                    .ThenInclude(l => l.Province)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
