using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using ProgramacionWeb3.PropertiesManager.Web.Models;

namespace ProgramacionWeb3.PropertiesManager.Web.Controllers
{
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

        public async Task<IActionResult> Index(int id)
        {
            var buildingId = id;    
            var building = await _buildingService.ObtenerEdificioPorIdAsync(buildingId);

            ViewBag.BuildingId = buildingId;
            ViewBag.BuildingName = ObtenerBuildingName(building, buildingId);

            var unitsDto = await _unitService.ObtenerUnidadesPorConsorcioAsync(buildingId);

            return View(unitsDto);
        }

        [HttpGet]
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
        public async Task<IActionResult> Create(UnitFormViewModel unitFVM)
        {
            if (!ModelState.IsValid)
            {
                var building = await _buildingService.ObtenerEdificioPorIdAsync(unitFVM.BuildingId);

                unitFVM.BuildingName = ObtenerBuildingName(building, unitFVM.BuildingId);

                unitFVM.Owners = await ObtenerOwnersSelectListAsync(unitFVM.OwnerId);

                return View(unitFVM);
            }

            var unitDto = new UnitDto
            {
                BuildingId = unitFVM.BuildingId,
                Floor = unitFVM.Floor,
                ApartmentNumber = unitFVM.ApartmentNumber,
                AreaInMeters = unitFVM.AreaInMeters,
                OwnerId = unitFVM.OwnerId
            };

            await _unitService.CrearUnidadAsync(unitDto);

            return RedirectToAction("Index", new { id = unitDto.BuildingId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var unitDto = await _unitService.ObtenerUnidadParaEditarAsync(id);

            if (unitDto == null)
            {
                return NotFound();
            }

            var unitViewModel = new UnitFormViewModel
            {
                Id = unitDto.Id,
                BuildingId = unitDto.BuildingId,
                BuildingName = unitDto.BuildingName,
                Floor = unitDto.Floor,
                ApartmentNumber = unitDto.ApartmentNumber,
                AreaInMeters = unitDto.AreaInMeters,    
                OwnerId = unitDto.OwnerId ?? 0,
                Owners = await ObtenerOwnersSelectListAsync(unitDto.OwnerId)
            };

            return View(unitViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(UnitFormViewModel unitFVM)
        {
            if (!ModelState.IsValid)
            {
                var building = await _buildingService.ObtenerEdificioPorIdAsync(unitFVM.BuildingId);

                unitFVM.BuildingName = ObtenerBuildingName(building, unitFVM.BuildingId);

                unitFVM.Owners = await ObtenerOwnersSelectListAsync(unitFVM.OwnerId);

                return View(unitFVM);
            }

            var unitDto = new UnitDto
            {
                Id = unitFVM.Id,
                BuildingId = unitFVM.BuildingId,
                Floor = unitFVM.Floor,
                ApartmentNumber = unitFVM.ApartmentNumber,
                AreaInMeters = unitFVM.AreaInMeters,
                OwnerId = unitFVM.OwnerId
            };

            await _unitService.ActualizarUnidadAsync(unitDto);

            return RedirectToAction("Index", new { id = unitDto.BuildingId });

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var unitDto = await _unitService.ObtenerUnidadParaEditarAsync(id);

            if (unitDto == null)
            {
                return NotFound();
            }

            var unitViewModel = new UnitFormViewModel
            {
                Id = unitDto.Id,
                BuildingId = unitDto.BuildingId,
                BuildingName = unitDto.BuildingName,
                Floor = unitDto.Floor,
                ApartmentNumber = unitDto.ApartmentNumber,
                AreaInMeters = unitDto.AreaInMeters,
                OwnerId = unitDto.OwnerId ?? 0
            };

            return View(unitViewModel);
        }


        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var unitDto = await _unitService.ObtenerUnidadParaEditarAsync(id);

            if (unitDto == null)
            {
                return NotFound();
            }

            var buildingId = unitDto.BuildingId;

            await _unitService.EliminarUnidadAsync(id);

            return RedirectToAction("Index", new { id = buildingId });
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
