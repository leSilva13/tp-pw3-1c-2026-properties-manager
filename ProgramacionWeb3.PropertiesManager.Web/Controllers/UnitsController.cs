using Microsoft.AspNetCore.Mvc;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;

namespace ProgramacionWeb3.PropertiesManager.Web.Controllers
{
    public class UnitsController : Controller
    {

        private readonly IUnitService _unitService;
       
        public UnitsController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        public async Task<IActionResult> Index(int buildingId)
        {
            ViewBag.buildingId = buildingId;
            var units = await _unitService.ObtenerUnidadesPorConsorcioAsync(buildingId);

            return View(units);
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            var buildingId = id;

            ViewBag.BuildingName = $"Consorcio Pepito"; //si hay name sino usar la direccion o el id

            var unit = new Unit
            {
                BuildingId = buildingId
            };

            return View(unit);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Unit unit, string ownerFullName, string ownerEmail)
        {
            await _unitService.CrearUnidadConOwnerAsync(unit, ownerFullName, ownerEmail);

            return RedirectToAction("Index", new { id = unit.BuildingId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var unit = await _unitService.ObtenerUnidadParaEditarAsync(id);

            if (unit == null)
            {
                return NotFound();
            }

            return View(unit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Unit unit)
        {
            await _unitService.ActualizarUnidadAsync(unit); //ojo solo edita el unit pero si deseo editar tmbien el owner debo crear otro servicio indicando eso

            return RedirectToAction("Index", new { id = unit.BuildingId });
        }
        [HttpGet]
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

            return RedirectToAction("Index", new { id = buildingId });
        }
    }
}