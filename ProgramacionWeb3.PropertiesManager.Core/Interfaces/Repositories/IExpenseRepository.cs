using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories
{
    public interface IExpenseRepository : IBaseRepository<Expense>
    {
        Task<IEnumerable<Expense>> GetByBuildingIdAsync(int buildingId);
        Task<IEnumerable<Expense>> GetByOwnerAsync(int ownerId);
        Task<Expense?> GetByIdWithRelationshipsAsync(int id);
    }
}
