using Microsoft.EntityFrameworkCore;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Repositories
{
    public class LocalityRepository : BaseRepository<Locality>, ILocalityRepository
    {
        public LocalityRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Locality>> ObtenerTodasAsync()
        {
            // Ordenamos alfabéticamente por nombre las provincias del HasData
            return await _dbContext.Localities
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ToListAsync<Locality>();
        }
    }
}
