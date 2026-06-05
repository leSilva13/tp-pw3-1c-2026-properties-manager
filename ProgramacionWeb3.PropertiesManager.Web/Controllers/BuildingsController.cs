using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using ProgramacionWeb3.PropertiesManager.Web.Models;

namespace ProgramacionWeb3.PropertiesManager.Web.Controllers
{
    //[Authorize]
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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // 1. Simulación de carga de provincias (Reemplazar por tu repositorio/servicio real)
            var listaProvincias = new List<Province> { new Province { Id = 1, Name = "Buenos Aires" } };

            var viewModel = new BuildingFormViewModel
            {
                Provinces = new SelectList(listaProvincias, "Id", "Name"),
                //Localities = new SelectList(new List<Locality>(), "Id", "Name") // Arranca vacío hasta que elijan provincia
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BuildingFormViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Asignamos el usuario administrador logueado al edificio
                viewModel.Building.UserId = 1; // TODO: Reemplazar por ID real de sesión

                // Asignamos la localidad elegida a la dirección del edificio
                if (viewModel.Building.Address != null)
                {
                    viewModel.Building.Address.ProvinceId = viewModel.SelectedProvinceId;
                    //viewModel.Building.Address.LocalityId = viewModel.SelectedLocalityId;

                }

                // Guardamos usando tu servicio asíncronico directo al repositorio
                await _buildingService.CrearEdificioAsync(viewModel.Building);

                return RedirectToAction(nameof(Index));
            }

            // Si el modelo no es válido, volvemos a cargar las provincias para no romper la vista
            // viewModel.Provinces = new SelectList(...);
            return View(viewModel);
        }
    }




}