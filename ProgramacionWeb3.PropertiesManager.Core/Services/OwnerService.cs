using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;

namespace ProgramacionWeb3.PropertiesManager.Core.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepository;

        public OwnerService(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }

        public async Task<IEnumerable<Owner>> ObtenerOwnersAsync()
        {
            return await _ownerRepository.GetAllAsync();
        }

        public async Task<Owner?> ObtenerOwnerPorIdAsync(int id)
        {
            return await _ownerRepository.GetByIdAsync(id);
        }

        public async Task CrearOwnerAsync(Owner owner)
        {
            await _ownerRepository.AddAsync(owner);
            await _ownerRepository.SaveAsync();
        }

        public async Task ActualizarOwnerAsync(Owner owner)
        {
            _ownerRepository.Update(owner);
            await _ownerRepository.SaveAsync();
        }

        public async Task EliminarOwnerPorIdAsync(int id)
        {
            var owner = await _ownerRepository.GetByIdAsync(id);

            if (owner == null)
            {
                return;
            }

            _ownerRepository.Delete(owner);
            await _ownerRepository.SaveAsync();
        }
    }
}
