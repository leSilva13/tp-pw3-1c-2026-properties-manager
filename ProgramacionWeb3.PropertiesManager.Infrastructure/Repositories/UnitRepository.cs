using Microsoft.EntityFrameworkCore;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Repositories
{
    public class UnitRepository : BaseRepository<Unit>, IUnitRepository
    {
        public UnitRepository(AppDbContext dbContext) : base(dbContext){}

        public async Task<Unit?> GetByUnitIdWithOwnerAndBuildingAsync(int id)
        {
            return await _dbContext.Units
                .Include(unidad => unidad.Owner)
                .Include(unidad => unidad.Building)
                .FirstOrDefaultAsync(unidad => unidad.Id == id);
        }

        public async Task<IEnumerable<Unit>> GetByBuildingIdAsync(int buildingId)
        {
            return await _dbContext.Units
                .Include(unidad => unidad.Owner)
                .Where(unidad => unidad.BuildingId == buildingId)
                .OrderBy(unidad => unidad.Floor)
                .ThenBy(unidad => unidad.ApartmentNumber)
                .ToListAsync();
        }
    }
}