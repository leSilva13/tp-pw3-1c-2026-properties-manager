using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;

namespace ProgramacionWeb3.PropertiesManager.Core.Services
{
    public class ProvinceService : IProvinceService
    {
        private readonly IProvinceRepository _provinceRepository;

        public ProvinceService(IProvinceRepository provinceRepository)
        {
            _provinceRepository = provinceRepository;
        }

        public async Task<IEnumerable<ProvinceDto>> GetAllAsync()
        {
            var provinces = await _provinceRepository.GetAllAsync();
            return provinces.Select(p => new ProvinceDto { Id = p.Id, Name = p.Name }).ToList();
        }
    }
}
