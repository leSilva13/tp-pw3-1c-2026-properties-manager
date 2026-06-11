using Microsoft.EntityFrameworkCore;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Repositories
{
    public class ProvinceRepository : BaseRepository<Province>, IProvinceRepository
    {
        public ProvinceRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Province>> ObtenerTodasAsync()
        {
            // Ordenamos alfabéticamente por nombre las provincias del HasData
            return await _dbContext.Provinces
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ToListAsync<Province>();
        }
    }
}
