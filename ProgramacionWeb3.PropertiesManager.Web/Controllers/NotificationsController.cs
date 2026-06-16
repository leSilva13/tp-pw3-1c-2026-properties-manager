using Microsoft.AspNetCore.Mvc;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace ProgramacionWeb3.PropertiesManager.Web.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly INotificacionService _notificacionService;
        private readonly IBuildingService _buildingService;

        public NotificationsController(INotificacionService notificacionService, IBuildingService buildingService)
        {
            _notificacionService = notificacionService;
            _buildingService = buildingService;
        }

        public async Task<IActionResult> Index(int buildingId)
        {

            if (buildingId == null || buildingId <= 0)
            {
                return RedirectToAction("Index", "Buildings");
            }

            var building = await _buildingService.ObtenerEdificioPorIdAsync(buildingId);

            ViewBag.BuildingId = buildingId;
            ViewBag.BuildingAddress = building?.Address != null
                ? $"{building.Address.Street} {building.Address.Number}"
                : "Edificio";

            var notificaciones = await _notificacionService.ObtenerPorConsorcioAsync(buildingId);
            return View(notificaciones);
        }

        public IActionResult Create(int buildingId)
        {
            var model = new Notificacion { IdBuilding = buildingId };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Notificacion notificacion)
        {
            // 1. Ignoramos la validación de las propiedades que no vienen del form
            ModelState.Remove("Building"); // ¡OJO! Si tu propiedad de navegación se llama "Building", pon "Building"
            ModelState.Remove("CreationDate");

            if (ModelState.IsValid)
            {
                await _notificacionService.CrearAsync(notificacion);
                return RedirectToAction(nameof(Index), new { buildingId = notificacion.IdBuilding });
            }

            // Si sigue fallando, esto te ayudará a ver el error oculto haciendo debug
            var errores = ModelState.Values.SelectMany(v => v.Errors);

            return View(notificacion);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var notificacion = await _notificacionService.ObtenerPorIdAsync(id);
            if (notificacion == null || notificacion.SendDate != null)
            {
                return RedirectToAction("Index", "Buildings"); 
            }
            return View(notificacion);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Notificacion notificacion)
        {
            if (ModelState.IsValid)
            {
                var result = await _notificacionService.ActualizarAsync(notificacion);
                if (result) return RedirectToAction(nameof(Index), new { buildingId = notificacion.IdBuilding });

                ModelState.AddModelError("", "No se pudo editar. Es posible que ya haya sido enviada.");
            }
            return View(notificacion);
        }

        public async Task<IActionResult> Details(int id)
        {
            var notificacion = await _notificacionService.ObtenerPorIdAsync(id);
            if (notificacion == null) return NotFound();

            return View(notificacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int buildingId)
        {
            await _notificacionService.EliminarAsync(id);
            return RedirectToAction(nameof(Index), new { buildingId = buildingId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(int id, int buildingId)
        {
            await _notificacionService.EnviarNotificacionAsync(id);
            return RedirectToAction(nameof(Index), new { buildingId = buildingId });
        }
    }
}