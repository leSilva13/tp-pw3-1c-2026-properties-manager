using Microsoft.EntityFrameworkCore;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;
using ProgramacionWeb3.PropertiesManager.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Repositories
{
    public class BuildingRepository : BaseRepository<Building>, IBuildingRepository
    {
        // Hereda el dbContext de BaseRepository (asumiendo que se llama _dbContext)
        public BuildingRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Building>> GetBuildingsByUserIdWithRelationshipsAsync(int userId)
        {
            // Tarea 2.2: Incluye de forma anidada la infraestructura edilicia
            // Requisito: Ordenado por nombre ascendentemente
            return await _dbContext.Buildings
                .Include(b => b.Address)
                    .ThenInclude(a => a.Locality)
                        .ThenInclude(l => l.Province)
                .Where(b => b.UserId == userId)
                .OrderBy(b => b.Id)
                .ToListAsync();
        }

        public async Task<Building?> GetBuildingByIdWithRelationshipsAsync(int id)
        {
            return await _dbContext.Buildings
                .Include(b => b.Address)
                    .ThenInclude(a => a.Locality)
                        .ThenInclude(l => l.Province)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}