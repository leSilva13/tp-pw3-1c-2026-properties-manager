using Microsoft.EntityFrameworkCore;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Repositories
{
    public class MultipurposeRoomReservationRepository : BaseRepository<MultipurposeRoomReservation>, IMultipurposeRoomReservationRepository
    {
        public MultipurposeRoomReservationRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<MultipurposeRoomReservation>> GetAllByBuildingAsync(int buildingId)
        {
            return await _dbContext.MultipurposeRoomReservations.Where(mr => mr.MultipurposeRoom.BuildingId == buildingId).ToListAsync();
        }
    }
}
