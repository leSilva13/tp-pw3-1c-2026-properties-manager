using ProgramacionWeb3.PropertiesManager.Core.Interfaces;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        public IUserRepository Users { get; private set; }
        public IUnitRepository Units { get; private set; }
        public IOwnerRepository Owners { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _dbContext = context;
            Users = new UserRepository(_dbContext);
            Units = new UnitRepository(_dbContext);
            Owners = new OwnerRepository(_dbContext);
        }

        public async Task<int> SaveAsync() => await _dbContext.SaveChangesAsync();

        public void Dispose() => _dbContext.Dispose();
    }
}
