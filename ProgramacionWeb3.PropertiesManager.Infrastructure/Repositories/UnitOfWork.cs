using ProgramacionWeb3.PropertiesManager.Core.Interfaces;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        public IUserRepository Users { get; private set; }
        public IExpenseRepository Expenses { get; private set; }
        public IExpenseDistributionRepository ExpenseDistributions { get; private set; }
        public IUnitRepository Units { get; private set; }
        public IExpenseTypeRepository ExpenseTypes { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _dbContext = context;
            Users = new UserRepository(_dbContext);
            Expenses = new ExpenseRepository(_dbContext);
            ExpenseDistributions = new ExpenseDistributionRepository(_dbContext);
            Units = new UnitRepository(_dbContext);
            ExpenseTypes = new ExpenseTypeRepository(_dbContext);
        }

        public async Task<int> SaveAsync() => await _dbContext.SaveChangesAsync();

        public void Dispose() => _dbContext.Dispose();
    }
}
