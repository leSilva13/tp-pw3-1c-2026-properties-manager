using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;

namespace ProgramacionWeb3.PropertiesManager.Core.Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitRepository _unitRepository;

        public UnitService(IUnitOfWork unitOfWork, IUnitRepository unitRepository)
        {
            _unitOfWork = unitOfWork;
            _unitRepository = unitRepository;
        }

        public async Task<IEnumerable<Unit>> ObtenerUnidadesPorConsorcioAsync(int buildingId)
        {
            return await _unitRepository.GetByBuildingIdAsync(buildingId);
        }

        public async Task<Unit?> ObtenerUnidadParaEditarAsync(int unitId)
        {
            return await _unitRepository.GetByUnitIdWithOwnerAndBuildingAsync(unitId);
        }

        public async Task CrearUnidadAsync(Unit unit)
        {
            await _unitRepository.AddAsync(unit);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ActualizarUnidadAsync(Unit unit)
        {
            _unitRepository.Update(unit);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task EliminarUnidadAsync(int unitId)
        {
            var unit = await _unitRepository.GetByIdAsync(unitId);

            if (unit == null)
            {
                return;
            }

            _unitRepository.Delete(unit);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
