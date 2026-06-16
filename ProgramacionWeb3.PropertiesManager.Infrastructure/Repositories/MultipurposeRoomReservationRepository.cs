using Microsoft.EntityFrameworkCore;
using ProgramacionWeb3.PropertiesManager.Core.Common.Enums;
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

        public async Task<MultipurposeRoomReservation?> GetByRoomReservationDateAndShiftAsync(int multipurposeRoomId, DateOnly reservationDate, MultipurposeRoomShiftEnum multipurposeRoomShiftEnum)
        {
            return await _dbContext.MultipurposeRoomReservations.FirstOrDefaultAsync(mrr => mrr.MultipurposeRoomId == multipurposeRoomId && mrr.ReservationDate == reservationDate && mrr.Shift == multipurposeRoomShiftEnum);
        }

        public async Task<MultipurposeRoomReservation?> GetFullByIdAsync(int multipurposeRoomReservationId)
        {
            return await _dbContext.MultipurposeRoomReservations
                .Include(mrr => mrr.MultipurposeRoom)
                .Include(mrr => mrr.Unit)
                    .ThenInclude(u => u.Building)
                .Include(mrr => mrr.Unit)
                    .ThenInclude(u => u.Owner)
                .FirstOrDefaultAsync(mrr => mrr.Id == multipurposeRoomReservationId);
        }

        public async Task<MultipurposeRoomReservation?> GetNextReservationAsync(int multipurposeRoomId, DateOnly reservationDate, MultipurposeRoomShiftEnum multipurposeRoomShiftEnum)
        {
            return await _dbContext.MultipurposeRoomReservations
                .Include(r => r.Unit)
                    .ThenInclude(u => u.Owner)
                .Where(r => r.MultipurposeRoomId == multipurposeRoomId &&
                            (r.ReservationDate > reservationDate ||
                            (r.ReservationDate == reservationDate && r.Shift > multipurposeRoomShiftEnum)))
                .OrderBy(r => r.ReservationDate)
                .ThenBy(r => r.Shift)
                .FirstOrDefaultAsync();
        }

        public async Task<MultipurposeRoomReservation?> GetPreviousReservationAsync(int multipurposeRoomId, DateOnly reservationDate, MultipurposeRoomShiftEnum multipurposeRoomShiftEnum)
        {
            return await _dbContext.MultipurposeRoomReservations
                .Include(r => r.Unit)
                    .ThenInclude(u => u.Owner)
                .Where(r => r.MultipurposeRoomId == multipurposeRoomId &&
                            (r.ReservationDate < reservationDate ||
                            (r.ReservationDate == reservationDate && r.Shift < multipurposeRoomShiftEnum)))
                .OrderByDescending(r => r.ReservationDate)
                .ThenByDescending(r => r.Shift)
                .FirstOrDefaultAsync();
        }
    }
}
