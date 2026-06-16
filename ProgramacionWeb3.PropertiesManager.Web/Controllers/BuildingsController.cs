using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using ProgramacionWeb3.PropertiesManager.Web.Models;
using System.Security.Claims;
using System.Globalization;

namespace ProgramacionWeb3.PropertiesManager.Web.Controllers
{
    [Authorize(Roles = "Properties Administrator")]
    public class BuildingsController : Controller
    {
        private readonly IBuildingService _buildingService;
        private readonly IProvinceService _provinceService;
        private readonly ILocalityService _localityService;

        public BuildingsController(IBuildingService buildingService, IProvinceService provinceService, ILocalityService localityService)
        {
            _buildingService = buildingService;
            _provinceService = provinceService;
            _localityService = localityService;
        }

        [Authorize(Roles = "Properties Administrator")]
        public async Task<IActionResult> Index()
        {
            // Recuperamos el ID del ClaimTypes.NameIdentifier que guardó el AccountController
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userIdLogueado))
            {
                return Unauthorized("No se pudo determinar el usuario de la sesión.");
            }

            // Pasamos el ID real de la sesión al servicio
            var edificios = await _buildingService.ObtenerEdificiosPorUsuarioAsync(userIdLogueado);

            return View(edificios);
        }

        [HttpGet]
        [Authorize(Roles = "Properties Administrator")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Provinces = new SelectList(await _provinceService.GetAllAsync(), "Id", "Name");
            ViewBag.Localities = new SelectList(await _localityService.GetAllAsync(), "Id", "Name");
            return View(new BuildingFormViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Properties Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BuildingFormViewModel viewModel, string redirectAction)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int userIdLogueado = int.Parse(userIdClaim);

                string latRaw = string.IsNullOrWhiteSpace(viewModel.AddressViewModel.Latitude) ? "0.0" : viewModel.AddressViewModel.Latitude;
                string lonRaw = string.IsNullOrWhiteSpace(viewModel.AddressViewModel.Longitude) ? "0.0" : viewModel.AddressViewModel.Longitude;

                // Parseamos usando la cultura invariable (fuerza a leer el punto decimal)
                decimal latitudConvertida = decimal.Parse(latRaw, CultureInfo.InvariantCulture);
                decimal longitudConvertida = decimal.Parse(lonRaw, CultureInfo.InvariantCulture);

                var buildingDto = new BuildingDto
                {
                    TotalSurfaceArea = viewModel.TotalSurfaceArea,
                    ExpensesDueDate = viewModel.ExpensesDueDate,
                    AdministratorUserId = userIdLogueado,
                    AddressDto = new AddressDto
                    {
                        Street = viewModel.AddressViewModel.Street,
                        Number = viewModel.AddressViewModel.Number,
                        ProvinceId = viewModel.AddressViewModel.ProvinceId,
                        LocalityId = viewModel.AddressViewModel.LocalityId,
                        Latitude = latitudConvertida,
                        Longitude = longitudConvertida
                    }
                };

                int newBuildingId = await _buildingService.CrearEdificioAsync(buildingDto);

                return redirectAction switch
                {
                    "createAnother" => RedirectToAction(nameof(Create)),
                    "createUnits"   => RedirectToAction("Create", "Units", new { id = newBuildingId }),
                    _               => RedirectToAction(nameof(Index))
                };
            }

            ViewBag.Provinces = new SelectList(await _provinceService.GetAllAsync(), "Id", "Name");
            ViewBag.Localities = new SelectList(await _localityService.GetAllAsync(), "Id", "Name");
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Properties Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0) return BadRequest("ID inválido.");

            // 1. Buscamos el edificio con todas sus relaciones
            var edificio = await _buildingService.ObtenerEdificioPorIdAsync(id);

            // Validamos que exista y que no esté con baja lógica
            if (edificio == null || !edificio.IsActive)
            {
                return NotFound("El edificio no existe o fue dado de baja.");
            }

            // 2. Cargamos el combo de provincias en el ViewBag (igual que en Create)
            ViewBag.Provinces = new SelectList(await _provinceService.GetAllAsync(), "Id", "Name", edificio.Address?.ProvinceId);
            ViewBag.Localities = new SelectList(await _localityService.GetAllAsync(), "Id", "Name", edificio.Address?.LocalityId);

            // 3. Armamos el ViewModel mapeando los datos de la Entidad hacia el modelo plano
            var viewModel = new BuildingFormViewModel
            {
                Id = edificio.Id,
                AddressId = edificio.AddressId, // Guardamos el ID de la dirección para el POST
                ExpensesDueDate = edificio.ExpensesDueDate,
                TotalSurfaceArea = edificio.TotalSurfaceArea,
                IsActive = edificio.IsActive,
                AddressViewModel = new AddressViewModel
                {
                    Street = edificio.Address?.Street,
                    Number = edificio.Address?.Number ?? "0",
                    ProvinceId = edificio.Address?.ProvinceId ?? 0,
                    LocalityId = edificio.Address?.LocalityId ?? 0
                    
                }
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Properties Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BuildingFormViewModel viewModel)
        {
            // Seguridad: Nos aseguramos que el ID de la URL sea el mismo del formulario
            if (id != viewModel.Id) return BadRequest("Los IDs no coinciden.");

            if (ModelState.IsValid)
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int userIdLogueado = int.Parse(userIdClaim);
                try
                {
                    // Mapeamos el ViewModel al BuildingDto de tu líder
                    var buildingDto = new BuildingDto
                    {
                        Id = viewModel.Id, // El DTO tiene Id de Building
                        TotalSurfaceArea = viewModel.TotalSurfaceArea,
                        ExpensesDueDate = viewModel.ExpensesDueDate,
                        IsActive = viewModel.IsActive,
                        AdministratorUserId = userIdLogueado, 
                        AddressDto = new AddressDto
                        {
                            Street = viewModel.AddressViewModel.Street,
                            Number = viewModel.AddressViewModel.Number,
                            ProvinceId = viewModel.AddressViewModel.ProvinceId,
                            LocalityId = viewModel.AddressViewModel.LocalityId
                        }
                    };

                    // Enviamos el DTO de tu líder y el addressId que recuperamos de la vista
                    await _buildingService.ActualizarEdificioAsync(buildingDto, viewModel.AddressId);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al actualizar: " + ex.Message);
                }
            }

            // Si algo falló, recargamos las provincias en el ViewBag para no romper la vista
            ViewBag.Provinces = new SelectList(await _provinceService.GetAllAsync(), "Id", "Name", viewModel.AddressViewModel?.ProvinceId);
            ViewBag.Localities = new SelectList(await _localityService.GetAllAsync(), "Id", "Name", viewModel.AddressViewModel?.LocalityId);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Properties Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID de edificio inválido.");
            }

            // Llamamos al servicio para que realice la baja lógica
            bool exito = await _buildingService.BajaDeEdificioAsync(id);

            if (!exito)
            {
                return NotFound("No se encontró el edificio que desea eliminar.");
            }

            // Redirigimos al Index, donde ahora el edificio ya no debería aparecer
            return RedirectToAction(nameof(Index));
        }
    }
}