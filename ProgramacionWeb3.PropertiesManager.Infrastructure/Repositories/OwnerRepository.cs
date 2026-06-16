using Microsoft.EntityFrameworkCore;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;


namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Repositories
{
    public class OwnerRepository : BaseRepository<Owner>, IOwnerRepository
    {   
        public OwnerRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<Owner?> GetByEmailAsync(string email) => await _dbContext.Owners.FirstOrDefaultAsync(x => x.Email == email);

        public async Task<Owner?> GetByUserIdAsync(int userId)
        {
            return await _dbContext.Owners
                .Include(o => o.Units)
                    .ThenInclude(u => u.Building)
                    .ThenInclude(b => b.MultipurposeRooms.Where(mr => mr.IsActive))
                .Include(o => o.Units)
                    .ThenInclude(u => u.Building)
                    .ThenInclude(b => b.Address)
                    .ThenInclude(a => a.Province)
                .Include(o => o.Units)
                    .ThenInclude(u => u.MultipurposeRoomReservations)
                    .ThenInclude(mrr => mrr.MultipurposeRoom)
                .FirstOrDefaultAsync(o => o.UserId == userId);
        }
    }
}
