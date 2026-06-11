using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services
{
    public interface IOwnerService
    {
        Task<IEnumerable<Owner>> ObtenerOwnersAsync();

        Task<Owner?> ObtenerOwnerPorIdAsync(int id);

        Task CrearOwnerAsync(Owner owner);

        Task ActualizarOwnerAsync(Owner owner);

        Task EliminarOwnerPorIdAsync(int id);

    }
}
