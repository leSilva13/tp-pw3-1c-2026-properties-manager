using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;

namespace ProgramacionWeb3.PropertiesManager.Core.Services
{
    public class LocalityService  : ILocalityService
    {
        private readonly ILocalityRepository _localityRepository;

        public LocalityService(ILocalityRepository localityRepository)
        {
            _localityRepository = localityRepository;
        }

        public async Task<IEnumerable<LocalityDto>> GetAllAsync()
        {
            var provinces = await _localityRepository.GetAllAsync();
            return provinces.Select(p => new LocalityDto { Id = p.Id, Name = p.Name }).ToList();
        }
    }
}
