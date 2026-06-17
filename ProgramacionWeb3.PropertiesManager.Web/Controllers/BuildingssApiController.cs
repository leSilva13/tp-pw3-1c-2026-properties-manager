using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using System.Security.Claims;

namespace ProgramacionWeb3.PropertiesManager.Web.Controllers
{
    [Route("api/consorcios")]
    [ApiController]
    [Authorize(Roles = "Properties Administrator")] 
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
            
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userIdLogueado))
            {
                return Unauthorized();
            }

            var edificios = await _buildingService.ObtenerEdificiosPorUsuarioAsync(userIdLogueado);

            var resultadoJson = edificios
                .Where(e => e.IsActive)
                .Select(e => new
                {
                    id = e.Id,
                    direccion = $"{e.Address?.Street} {e.Address?.Number}",
                    latitud = e.Address.Latitude,
                    longitud = e.Address.Longitude
                });

            return Ok(resultadoJson);
        }
    }
}
