using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;

namespace ProgramacionWeb3.PropertiesManager.Core.Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitOfWork _uow;

        public UnitService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<Unit>> ObtenerUnidadesPorConsorcioAsync(int buildingId)
        {
            return await _uow.Units.GetByBuildingIdAsync(buildingId);
        }

        public async Task<Unit?> ObtenerUnidadParaEditarAsync(int unitId)
        {
            return await _uow.Units.GetByUnitIdWithOwnerAndBuildingAsync(unitId);
        }

        public async Task CrearUnidadAsync(Unit unit)
        {
            await _uow.Units.AddAsync(unit);
            await _uow.SaveAsync();
        }

        public async Task ActualizarUnidadAsync(Unit unit)
        {
            _uow.Units.Update(unit);
            await _uow.SaveAsync();
        }

        public async Task EliminarUnidadAsync(int unitId)
        {
            var unit = await _uow.Units.GetByIdAsync(unitId);

            if (unit == null)
            {
                return;
            }

            _uow.Units.Delete(unit);
            await _uow.SaveAsync();
        }
        public async Task CrearUnidadConOwnerAsync(Unit unit, string ownerFullName, string ownerEmail)
        {
            var owner = await _uow.Owners.GetByEmailAsync(ownerEmail);

            if (owner == null)
            {
                owner = new Owner
                {
                    FullName = ownerFullName,
                    Email = ownerEmail
                };

                await _uow.Owners.AddAsync(owner);
            }

            unit.Owner = owner;

            await _uow.Units.AddAsync(unit);
            await _uow.SaveAsync();
        }
    }
}
