using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext dbContext) : base(dbContext) {  }

        public Task<User?> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
