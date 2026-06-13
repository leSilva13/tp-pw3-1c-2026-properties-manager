using Microsoft.EntityFrameworkCore;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext dbContext) : base(dbContext) {  }

        public Task<User?> GetActiveUserByEmailAsync(string email)
        {
            return _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            return _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
