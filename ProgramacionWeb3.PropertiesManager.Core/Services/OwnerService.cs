using ProgramacionWeb3.PropertiesManager.Core.DTOs;
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
        //inyectar el uof

        public OwnerService(IUnitOfWork unitOfWork, IOwnerRepository ownerRepository)
        {
            _unitOfWork = unitOfWork;
            _ownerRepository = ownerRepository;
        }

        public async Task<IEnumerable<OwnerDto>> ObtenerOwnersAsync()
        {
            var ownersEntidad = await _ownerRepository.GetAllAsync();

            var ownersDto = ownersEntidad
                .Where(owner => owner.IsActive)
                .Select(owner => new OwnerDto
                {
                    Id = owner.Id,
                    FullName = owner.FullName,
                    Email = owner.Email,
                    IsActive = owner.IsActive
                });

            return ownersDto;
        }

        public async Task<OwnerDto?> ObtenerOwnerPorIdAsync(int id)
        {
            var ownerEntidad = await _ownerRepository.GetByIdAsync(id);

            if (ownerEntidad == null || !ownerEntidad.IsActive)
            {
                return null;
            }

            var ownerDto = new OwnerDto
            {
                Id = ownerEntidad.Id,
                FullName = ownerEntidad.FullName,
                Email = ownerEntidad.Email,
                IsActive = ownerEntidad.IsActive
            };
            return ownerDto;
        }

        public async Task<bool> CrearOwnerAsync(OwnerDto ownerDto)
        {
            var ownerExistente = await _ownerRepository.GetByEmailAsync(ownerDto.Email);

            if (ownerExistente != null)
            {
                return false;
            }

            var ownerEntidad = new Owner
            {
                FullName = ownerDto.FullName,
                Email = ownerDto.Email,
                IsActive = true
            };

            await _ownerRepository.AddAsync(ownerEntidad);
        
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActualizarOwnerAsync(OwnerDto ownerDto)
        {
            var ownerEntidad = await _ownerRepository.GetByIdAsync(ownerDto.Id);

            if (ownerEntidad == null || !ownerEntidad.IsActive)
            {
                return false;
            }

            var ownerConMismoEmail = await _ownerRepository.GetByEmailAsync(ownerDto.Email);

            if (ownerConMismoEmail != null && ownerConMismoEmail.Id != ownerDto.Id)
            {
                return false;
            }

            ownerEntidad.FullName = ownerDto.FullName;
            ownerEntidad.Email = ownerDto.Email;

            _ownerRepository.Update(ownerEntidad);
            await _unitOfWork.SaveChangesAsync();
             return true;
        }

        public async Task EliminarOwnerPorIdAsync(int id)
        {
            var ownerEntidad = await _ownerRepository.GetByIdAsync(id);

            if (ownerEntidad == null)
            {
                return;
            }

            ownerEntidad.IsActive = false;

            _ownerRepository.Update(ownerEntidad);
            await _unitOfWork.SaveChangesAsync();
        }


    }
}
