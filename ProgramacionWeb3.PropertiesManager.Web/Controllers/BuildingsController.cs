using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;

namespace ProgramacionWeb3.PropertiesManager.Web.Controllers
{
    [Authorize]
    public class BuildingsController : Controller
    {
        private readonly IBuildingService _buildingService;

        public BuildingsController(IBuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        // GET: /Buildings o /Buildings/Index
        public async Task<IActionResult> Index()
        {
            // TODO: Reemplazar por el ID real del usuario administrador logueado (desde Claims/Sesión)
            int userIdLogueado = 1;

            // Llamamos a tu servicio que ya incluye Address, Locality y Province
            var edificios = await _buildingService.ObtenerEdificiosPorUsuarioAsync(userIdLogueado);

            return View(edificios);
        }
    }
}