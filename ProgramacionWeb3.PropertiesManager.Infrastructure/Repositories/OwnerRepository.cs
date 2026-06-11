using Microsoft.EntityFrameworkCore;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Repositories
{
    public class OwnerRepository : BaseRepository<Owner>, IOwnerRepository  
    {
        public OwnerRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<Owner?> GetByEmailAsync(string email)
        {
            return await _dbContext.Owners.FirstOrDefaultAsync(o => o.Email == email);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
