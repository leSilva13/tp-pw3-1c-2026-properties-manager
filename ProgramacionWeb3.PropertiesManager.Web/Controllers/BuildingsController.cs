using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using ProgramacionWeb3.PropertiesManager.Infrastructure.Repositories;
using ProgramacionWeb3.PropertiesManager.Web.Models;

namespace ProgramacionWeb3.PropertiesManager.Web.Controllers
{
    //[Authorize]
    public class BuildingsController : Controller
    {
        private readonly IBuildingService _buildingService;
        private readonly IProvinceRepository _provinceRepository;

        public BuildingsController(IBuildingService buildingService, IProvinceRepository provinceRepository)
        {
            _buildingService = buildingService;
            _provinceRepository = provinceRepository;
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
            var listaProvincias = await _provinceRepository.ObtenerTodasAsync();


            var viewModel = new BuildingFormViewModel
            {
                Provinces = new SelectList(listaProvincias, "Id", "Name")
                //Localities = new SelectList(new List<Locality>(), "Id", "Name")
            };

            viewModel.Building.IsActive = true;
            viewModel.Building.CreationDate = DateTime.Now;

            // Asignamos el ID de usuario administrador (Acordate de poner un ID que exista en tu tabla Users)
            viewModel.Building.UserId = 1;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BuildingFormViewModel viewModel)
        {
            // Mapeamos el ID de la provincia seleccionada en el combo directamente a la dirección
            if (viewModel.Building.Address != null)
            {
                viewModel.Building.Address.ProvinceId = viewModel.SelectedProvinceId;
            }

            // 🚀 LA SOLUCIÓN: Le decimos a .NET que no valide la entidad compleja "Province" 
            // ni al "AdministratorUser", ya que se completan en el backend o la base de datos.
            ModelState.Remove("Building.Address.Province");
            ModelState.Remove("Building.AdministratorUser"); // Por las dudas, si también chillaba
            ModelState.Remove("Building.Units");
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


        [HttpPost]
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