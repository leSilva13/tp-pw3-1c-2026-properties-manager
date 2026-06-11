using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories
{
    public interface IProvinceRepository : IBaseRepository<Province>
    {
        Task<IEnumerable<Province>> ObtenerTodasAsync();
    }
}
