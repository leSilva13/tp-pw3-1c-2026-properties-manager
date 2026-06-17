using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories
{
    public interface ILocalityRepository : IBaseRepository<Locality>
    {
        Task<IEnumerable<Locality>> ObtenerTodasAsync();
    }
}
