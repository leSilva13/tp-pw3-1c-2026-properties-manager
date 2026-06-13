using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using ProgramacionWeb3.PropertiesManager.Web.Models;

namespace ProgramacionWeb3.PropertiesManager.Web.Controllers
{
    [Authorize(Roles = "Properties Administrator")]
    public class OwnersController : Controller
    {

        private IOwnerService _ownerService;

        public OwnersController(IOwnerService ownerService) 
        {
            this._ownerService = ownerService;
        }


        [Authorize(Roles = "Properties Administrator")]
        public async Task<IActionResult> Index()
        {
            var owners = await _ownerService.ObtenerOwnersAsync();

            return View(owners);
        }

        [HttpGet]
        [Authorize(Roles = "Properties Administrator")]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Properties Administrator")]
        public async Task<IActionResult> Crear(OwnerFormViewModel ownerFVM)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("No se pudo");
                return View(ownerFVM);
            }
            var owner = new Owner
            {
                FullName = ownerFVM.FullName,
                Email = ownerFVM.Email
            };

            await _ownerService.CrearOwnerAsync(owner);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Properties Administrator")]
        public async Task<IActionResult> Editar(int id)
        {
            var ownerObtenido = await _ownerService.ObtenerOwnerPorIdAsync(id);

            if (ownerObtenido == null)
            {
                return NotFound();
            }

            var viewModel = new OwnerFormViewModel
            {
                Id = ownerObtenido.Id,
                FullName = ownerObtenido.FullName,
                Email = ownerObtenido.Email
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Properties Administrator")]
        public async Task<IActionResult> Editar(OwnerFormViewModel ownerFVM)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("No se pudo");
                return View(ownerFVM);
            }
            var owner = new Owner
            {
                Id = ownerFVM.Id,
                FullName = ownerFVM.FullName,
                Email = ownerFVM.Email
            };
            await _ownerService.ActualizarOwnerAsync(owner);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Properties Administrator")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var owner = await _ownerService.ObtenerOwnerPorIdAsync(id);

            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        [HttpPost]
        [Authorize(Roles = "Properties Administrator")]
        [ActionName("Eliminar")]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            await _ownerService.EliminarOwnerPorIdAsync(id);

            return RedirectToAction("Index");
        }
    }
}
