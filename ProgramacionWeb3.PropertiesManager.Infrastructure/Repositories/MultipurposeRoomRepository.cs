using Microsoft.EntityFrameworkCore;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Repositories
{
    public class MultipurposeRoomRepository : BaseRepository<MultipurposeRoom>, IMultipurposeRoomRepository
    {
        public MultipurposeRoomRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<MultipurposeRoom>> GetAllByBuildingAsync(int buildingId)
        {
            return await _dbContext.MultipurposeRooms.Where(mr => mr.BuildingId == buildingId).ToListAsync();
        }
    }
}
