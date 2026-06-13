using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using ProgramacionWeb3.PropertiesManager.Web.Models;

namespace ProgramacionWeb3.PropertiesManager.Web.Controllers
{
    [Authorize(Roles = "Properties Administrator")]
    public class BuildingsController : Controller
    {
        private readonly IBuildingService _buildingService;
        private readonly IProvinceService _provinceService;

        public BuildingsController(IBuildingService buildingService, IProvinceService provinceService)
        {
            _buildingService = buildingService;
            _provinceService = provinceService;
        }

        [Authorize(Roles = "Properties Administrator")]
        public async Task<IActionResult> Index()
        {
            // TODO: Reemplazar por el ID real del usuario administrador logueado (desde Claims/Sesión)
            int userIdLogueado = 1;

            // Llamamos a tu servicio que ya incluye Address, Locality y Province
            var edificios = await _buildingService.ObtenerEdificiosPorUsuarioAsync(userIdLogueado);

            return View(edificios);
        }

        [HttpGet]
        [Authorize(Roles = "Properties Administrator")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Provinces = new SelectList(await _provinceService.GetAllAsync(), "Id", "Name");
            return View(new BuildingFormViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Properties Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BuildingFormViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var buildingDto = new BuildingDto
                {
                    TotalSurfaceArea = viewModel.TotalSurfaceArea,
                    ExpensesDueDate = viewModel.ExpensesDueDate,
                    AdministratorUserId = 1, //GET CURRENT USER
                    AddressDto = new AddressDto
                    {
                        Street = viewModel.AddressViewModel.Street,
                        Number = viewModel.AddressViewModel.Number,
                        ProvinceId = viewModel.AddressViewModel.ProvinceId,
                    }
                };

                await _buildingService.CrearEdificioAsync(buildingDto);

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Provinces = new SelectList(await _provinceService.GetAllAsync(), "Id", "Name");
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Properties Administrator")]
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
            var listaProvincias = await _provinceService.GetAllAsync();

            // 3. Armamos el ViewModel mapeando los datos que ya existen en la base de datos
            //var viewModel = new BuildingFormViewModel
            //{
            //    Building = edificio,
            //    Provinces = new SelectList(listaProvincias, "Id", "Name", edificio.Address?.ProvinceId),
            //    SelectedProvinceId = edificio.Address?.ProvinceId ?? 0
            //};

            return View(new BuildingFormViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Properties Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BuildingFormViewModel viewModel)
        {
            //// Seguridad: Nos aseguramos que el ID de la URL sea el mismo del objeto que se edita
            //if (id != viewModel.Building.Id) return BadRequest("Los IDs no coinciden.");

            //// Asignamos la provincia seleccionada en el combo al objeto Address
            //if (viewModel.Building.Address != null)
            //{
            //    viewModel.Building.Address.ProvinceId = viewModel.SelectedProvinceId;
            //}

            //// Limpiamos las validaciones de EF que no vienen del formulario (igual que en Create)
            //ModelState.Remove("Building.Address.Province");
            //ModelState.Remove("Building.AdministratorUser");
            //ModelState.Remove("Building.Units");

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        // Mantenemos datos que el usuario no edita en este formulario para que no se pierdan o se vuelvan null
            //        viewModel.Building.IsActive = true;
            //        viewModel.Building.UserId = 1; // TODO: Mantener el ID real del dueño actual

            //        // Llamamos al servicio para actualizar
            //        await _buildingService.ActualizarEdificioAsync(viewModel.Building);

            //        return RedirectToAction(nameof(Index));
            //    }
            //    catch (Exception ex)
            //    {
            //        ModelState.AddModelError("", "Error al actualizar: " + ex.Message);
            //    }
            //}

            //// Si algo falló, recargamos las provincias para no romper la vista
            //var listaProvincias = await _provinceRepository.ObtenerTodasAsync();
            //viewModel.Provinces = new SelectList(listaProvincias, "Id", "Name", viewModel.SelectedProvinceId);
            //return View(viewModel);
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