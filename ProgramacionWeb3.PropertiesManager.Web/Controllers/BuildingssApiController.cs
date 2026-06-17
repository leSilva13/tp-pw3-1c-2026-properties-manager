using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using System.Security.Claims;

namespace ProgramacionWeb3.PropertiesManager.Web.Controllers
{
    [Route("api/consorcios")]
    [ApiController]
    [Authorize(Roles = "Properties Administrator")] // Protegido: Solo el administrador ve sus consorcios
    public class BuildingsApiController : ControllerBase
    {
        private readonly IBuildingService _buildingService;

        public BuildingsApiController(IBuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        [HttpGet("listado-mapa")]
        public async Task<IActionResult> GetListadoMapa()
        {
            // 1. Obtenemos el ID del usuario logueado desde sus Claims
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userIdLogueado))
            {
                return Unauthorized();
            }

            // 2. Buscamos los edificios en la base de datos
            var edificios = await _buildingService.ObtenerEdificiosPorUsuarioAsync(userIdLogueado);

            // 3. Proyectamos a un formato JSON anónimo y limpio para el mapa
            var resultadoJson = edificios.Select(e => new
            {
                id = e.Id,
                direccion = $"{e.Address?.Street} {e.Address?.Number}",
                latitud = e.Address.Latitude,   // 💡 Propiedades reales que se grabaron al crear/editar
                longitud = e.Address.Longitude
            });

            // Retorna un HTTP 200 con el JSON estructurado
            return Ok(resultadoJson);
        }
    }
}
