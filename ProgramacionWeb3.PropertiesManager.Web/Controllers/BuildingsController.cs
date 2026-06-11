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
            var listaProvincias = await _provinceRepository.ObtenerTodasAsync();
            viewModel.Provinces = new SelectList(listaProvincias, "Id", "Name", viewModel.SelectedProvinceId);
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0) return BadRequest("ID inválido.");

            // 1. Buscamos el edificio con todas sus relaciones (dirección, provincia, etc.)
            var edificio = await _buildingService.ObtenerEdificioPorIdAsync(id);

            // Validamos que exista y que no esté con baja lógica
            if (edificio == null || !edificio.IsActive)
            {
                return NotFound("El edificio no existe o fue dado de baja.");
            }

            // 2. Cargamos el combo de provincias (igual que en el Create)
            var listaProvincias = await _provinceRepository.ObtenerTodasAsync();

            // 3. Armamos el ViewModel mapeando los datos que ya existen en la base de datos
            var viewModel = new BuildingFormViewModel
            {
                Building = edificio,
                Provinces = new SelectList(listaProvincias, "Id", "Name", edificio.Address?.ProvinceId),
                SelectedProvinceId = edificio.Address?.ProvinceId ?? 0
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BuildingFormViewModel viewModel)
        {
            // Seguridad: Nos aseguramos que el ID de la URL sea el mismo del objeto que se edita
            if (id != viewModel.Building.Id) return BadRequest("Los IDs no coinciden.");

            // Asignamos la provincia seleccionada en el combo al objeto Address
            if (viewModel.Building.Address != null)
            {
                viewModel.Building.Address.ProvinceId = viewModel.SelectedProvinceId;
            }

            // Limpiamos las validaciones de EF que no vienen del formulario (igual que en Create)
            ModelState.Remove("Building.Address.Province");
            ModelState.Remove("Building.AdministratorUser");
            ModelState.Remove("Building.Units");

            if (ModelState.IsValid)
            {
                try
                {
                    // Mantenemos datos que el usuario no edita en este formulario para que no se pierdan o se vuelvan null
                    viewModel.Building.IsActive = true;
                    viewModel.Building.UserId = 1; // TODO: Mantener el ID real del dueño actual

                    // Llamamos al servicio para actualizar
                    await _buildingService.ActualizarEdificioAsync(viewModel.Building);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al actualizar: " + ex.Message);
                }
            }

            // Si algo falló, recargamos las provincias para no romper la vista
            var listaProvincias = await _provinceRepository.ObtenerTodasAsync();
            viewModel.Provinces = new SelectList(listaProvincias, "Id", "Name", viewModel.SelectedProvinceId);
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