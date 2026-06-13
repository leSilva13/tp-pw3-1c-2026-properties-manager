using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using ProgramacionWeb3.PropertiesManager.Web.Models;

namespace ProgramacionWeb3.PropertiesManager.Web.Controllers
{
    [Authorize(Roles = "Properties Administrator")]
    public class UnitsController : Controller
    {

        private IUnitService _unitService;

        private IOwnerService _ownerService;

        private IBuildingService _buildingService;
        public UnitsController(IUnitService unitService, IOwnerService ownerService,IBuildingService buildingService)
        {
            this._unitService = unitService;
            this._ownerService = ownerService;
            this._buildingService = buildingService;
        }

        [Authorize(Roles = "Properties Administrator")]
        public async Task<IActionResult> Index(int buildingId)
        {
            ViewBag.buildingId = buildingId;
            var units = await _unitService.ObtenerUnidadesPorConsorcioAsync(buildingId);

            return View(units);
        }

        [HttpGet]
        [Authorize(Roles = "Properties Administrator")]
        public async Task<IActionResult> Create(int id)
        {
            var buildingId = id;

            var building = await _buildingService.ObtenerEdificioPorIdAsync(buildingId);

            var unitViewModel = new UnitFormViewModel
            {
                BuildingId = buildingId,
                BuildingName = ObtenerBuildingName(building, buildingId),
                Owners = await ObtenerOwnersSelectListAsync()
            };

            return View(unitViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Properties Administrator")]
        public async Task<IActionResult> Create(UnitFormViewModel unitFVM)
        {
            if (!ModelState.IsValid)
            {
                unitFVM.Owners = await ObtenerOwnersSelectListAsync();

                return View(unitFVM);
            }

            var unit = new Unit
            {
                BuildingId = unitFVM.BuildingId,
                Floor = unitFVM.Floor,
                ApartmentNumber = unitFVM.ApartmentNumber,
                AreaInMeters = unitFVM.AreaInMeters,
                OwnerId = unitFVM.OwnerId
            };

            await _unitService.CrearUnidadAsync(unit);

            return RedirectToAction("Index", new { buildingId = unit.BuildingId });
        }

        [HttpGet]
        [Authorize(Roles = "Properties Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var unit = await _unitService.ObtenerUnidadParaEditarAsync(id);

            if (unit == null)
            {
                return NotFound();
            }

            var unitViewModel = new UnitFormViewModel
            {
                Id = unit.Id,
                BuildingId = unit.BuildingId,
                BuildingName = ObtenerBuildingName(unit.Building, unit.BuildingId),
                Floor = unit.Floor,
                ApartmentNumber = unit.ApartmentNumber,
                AreaInMeters = unit.AreaInMeters,
                OwnerId = unit.OwnerId ?? 0,
                Owners = await ObtenerOwnersSelectListAsync(unit.OwnerId)
            };

            return View(unitViewModel);
        }


        [HttpPost]
        [Authorize(Roles = "Properties Administrator")]
        public async Task<IActionResult> Edit(UnitFormViewModel unitFVM)
        {
            if (!ModelState.IsValid)
            {
                unitFVM.Owners = await ObtenerOwnersSelectListAsync(unitFVM.OwnerId);

                return View(unitFVM);
            }

            var unit = new Unit
            {
                Id = unitFVM.Id,
                BuildingId = unitFVM.BuildingId,
                Floor = unitFVM.Floor,
                ApartmentNumber = unitFVM.ApartmentNumber,
                AreaInMeters = unitFVM.AreaInMeters,
                OwnerId = unitFVM.OwnerId
            };

            await _unitService.ActualizarUnidadAsync(unit);

            return RedirectToAction("Index", new { buildingId = unit.BuildingId });

        }







        [HttpGet]
        [Authorize(Roles = "Properties Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var unit = await _unitService.ObtenerUnidadParaEditarAsync(id);

            if (unit == null)
            {
                return NotFound();
            }

            return View(unit);
        }


        [HttpPost]
        [Authorize(Roles = "Properties Administrator")]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var unit = await _unitService.ObtenerUnidadParaEditarAsync(id);

            if (unit == null)
            {
                return NotFound();
            }

            var buildingId = unit.BuildingId;

            await _unitService.EliminarUnidadAsync(id);

            return RedirectToAction("Index", new { buildingId });
        }


        private async Task<SelectList> ObtenerOwnersSelectListAsync(int? ownerSeleccionadoId = null)
        {
            var owners = await _ownerService.ObtenerOwnersAsync();

            var ownersItems = owners.Select(owner => new
            {
                Id = owner.Id,
                Texto = owner.FullName + " - " + owner.Email
            });

            return new SelectList(ownersItems, "Id", "Texto", ownerSeleccionadoId);
        }

        private string ObtenerBuildingName(Building? building, int buildingId)
        {
            if (building != null && building.Address != null)
            {
                return building.Address.Street + " " + building.Address.Number;
            }

            return "Edificio #" + buildingId;
        }
    }
}
