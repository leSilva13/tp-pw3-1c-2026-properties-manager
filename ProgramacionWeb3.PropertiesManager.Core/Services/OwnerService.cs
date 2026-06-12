using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;

namespace ProgramacionWeb3.PropertiesManager.Core.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOwnerRepository _ownerRepository;

        public OwnerService(IUnitOfWork unitOfWork, IOwnerRepository ownerRepository)
        {
            _unitOfWork = unitOfWork;
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
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ActualizarOwnerAsync(Owner owner)
        {
            _ownerRepository.Update(owner);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task EliminarOwnerPorIdAsync(int id)
        {
            var owner = await _ownerRepository.GetByIdAsync(id);

            if (owner == null)
            {
                return;
            }

            _ownerRepository.Delete(owner);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
