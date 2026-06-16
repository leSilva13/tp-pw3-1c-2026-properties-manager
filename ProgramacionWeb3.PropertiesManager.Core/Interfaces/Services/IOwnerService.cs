using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services
{
    public interface IOwnerService
    {
        Task<IEnumerable<OwnerDto>> ObtenerOwnersAsync();

        Task<OwnerDto?> ObtenerOwnerPorIdAsync(int id);

        Task<bool> CrearOwnerAsync(OwnerDto ownerDto);

        Task<bool> ActualizarOwnerAsync(OwnerDto ownerDto);

        Task EliminarOwnerPorIdAsync(int id);

    }
}
