using Microsoft.AspNetCore.Mvc;
using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using ProgramacionWeb3.PropertiesManager.Web.Models;

namespace ProgramacionWeb3.PropertiesManager.Web.Controllers
{
    public class OwnersController : Controller
    {

        private IOwnerService _ownerService;

        public OwnersController(IOwnerService ownerService) 
        {
            this._ownerService = ownerService;
        }


        public async Task<IActionResult> Index()
        {
            var ownersDto = await _ownerService.ObtenerOwnersAsync();

            var ownersViewModel = ownersDto.Select(ownerDto => new OwnerFormViewModel
            {
                Id = ownerDto.Id,
                FullName = ownerDto.FullName,
                Email = ownerDto.Email
            });

            return View(ownersViewModel);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View(new OwnerFormViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Crear(OwnerFormViewModel ownerFVM)
        {
            if (!ModelState.IsValid)
            {
                return View(ownerFVM);
            }

            var ownerDto = new OwnerDto
            {
                FullName = ownerFVM.FullName,
                Email = ownerFVM.Email
            };

            var creado = await _ownerService.CrearOwnerAsync(ownerDto);

            if (!creado)
            {
                ModelState.AddModelError("Email" , "Ya existe un Owner con ese email");

                return View(ownerFVM);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var ownerObtenidoDto = await _ownerService.ObtenerOwnerPorIdAsync(id);

            if (ownerObtenidoDto == null)
            {
                return NotFound();
            }

            var viewModel = new OwnerFormViewModel
            {
                Id = ownerObtenidoDto.Id,
                FullName = ownerObtenidoDto.FullName,
                Email = ownerObtenidoDto.Email
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(OwnerFormViewModel ownerFVM)
        {
            if (!ModelState.IsValid)
            {
                return View(ownerFVM);
            }

            var ownerDto = new OwnerDto
            {
                Id = ownerFVM.Id,
                FullName = ownerFVM.FullName,
                Email = ownerFVM.Email
            };

            var actualizado = await _ownerService.ActualizarOwnerAsync(ownerDto);

            if (!actualizado)
            {
                ModelState.AddModelError("Email", "Ya existe otro Owner con ese email");

                return View(ownerFVM);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
       public async Task<IActionResult> Eliminar(int id)
        {
            var ownerDto = await _ownerService.ObtenerOwnerPorIdAsync(id);

            if (ownerDto == null)
            {
                return NotFound();
            }
            var viewModel = new OwnerFormViewModel
            {
                Id = ownerDto.Id,
                FullName = ownerDto.FullName,
                Email = ownerDto.Email
            };
            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Eliminar")]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            await _ownerService.EliminarOwnerPorIdAsync(id);

            return RedirectToAction("Index");
        }
    }
}
