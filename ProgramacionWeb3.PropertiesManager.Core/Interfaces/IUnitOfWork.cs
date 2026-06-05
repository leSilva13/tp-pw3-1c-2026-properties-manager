using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IUnitRepository Units { get; }
        IOwnerRepository Owners { get; }
        Task<int> SaveAsync();
    }
}
