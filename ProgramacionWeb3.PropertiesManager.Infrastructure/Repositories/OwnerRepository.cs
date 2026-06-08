using Microsoft.EntityFrameworkCore;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;


namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Repositories
{
    public class OwnerRepository : BaseRepository<Owner>, IOwnerRepository
    {
        private readonly AppDbContext _dbContext;

        public OwnerRepository(AppDbContext dbContext) : base(dbContext) { _dbContext = dbContext; }

        public async Task<Owner?> GetByEmailAsync(string email) => await _dbContext.Owners.FirstOrDefaultAsync(x => x.Email == email);
    }
}
