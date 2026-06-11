using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IExpenseRepository Expenses { get; }
        IExpenseDistributionRepository ExpenseDistributions { get; }
        IUnitRepository Units { get; }
        IExpenseTypeRepository ExpenseTypes { get; }
        IOwnerRepository Owners { get; }
        Task<int> SaveAsync();
    }
}
