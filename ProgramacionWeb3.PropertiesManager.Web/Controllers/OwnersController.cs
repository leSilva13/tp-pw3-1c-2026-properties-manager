using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using ProgramacionWeb3.PropertiesManager.Web.Models;
using ProgramacionWeb3.PropertiesManager.Web.Models.Building;
using ProgramacionWeb3.PropertiesManager.Web.Models.Owner;
using ProgramacionWeb3.PropertiesManager.Web.Models.Unit;
using ProgramacionWeb3.PropertiesManager.Web.Security;

namespace ProgramacionWeb3.PropertiesManager.Web.Controllers
{
    [Authorize(Roles = "Properties Administrator, Owner")]
    public class OwnersController : Controller
    {

        private IOwnerService _ownerService;
        private IOwnerDashboardService _ownerDashboardService;

        public OwnersController(IOwnerService ownerService, IOwnerDashboardService ownerDashboardService) 
        {
            _ownerService = ownerService;
            _ownerDashboardService = ownerDashboardService;
        }


        [Authorize(Roles = "Properties Administrator")]
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
        [Authorize(Roles = "Properties Administrator")]
        public IActionResult Crear()
        {
            return View(new OwnerFormViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Properties Administrator")]
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
        [Authorize(Roles = "Properties Administrator")]
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
        [Authorize(Roles = "Properties Administrator")]
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
        [Authorize(Roles = "Properties Administrator")]
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
        [Authorize(Roles = "Properties Administrator")]
        [ActionName("Eliminar")]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            await _ownerService.EliminarOwnerPorIdAsync(id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Dashboard()
        {
            var ownerDashboardDto = await _ownerDashboardService.GetDashboardDataAsync(User.GetUserId());

            var ownerDashboardViewModel = new OwnerDashboardViewModel
            {
                BuildingViewModels = ownerDashboardDto.BuildingDtos
                    .Select(b => 
                        new BuildingViewModel
                        {
                            Id = b.Id,
                            ExpensesDueDate = b.ExpensesDueDate,
                            FullAddress = string.Format("{0} {1}, {2}", 
                                b.AddressDto.Street, b.AddressDto.Number, b.AddressDto.ProvinceName),
                            MultipurposeRoomViewModels = b.MultipurposeRoomDtos.Select(mr =>
                                new MultipurposeRoomViewModel
                                {
                                    Floor = mr.Floor,
                                    IsActive = mr.IsActive,
                                    Name = mr.Name,
                                    Id = mr.Id,
                                }),
                            UnitViewModels = b.UnitDtos.Select(u =>
                                new UnitViewModel
                                {
                                    Id = u.Id,
                                    ApartmentNumber = u.ApartmentNumber,
                                    Floor = u.Floor,
                                })
                        }),
                MultipurposeRoomReservationViewModels = ownerDashboardDto.MultipurposeRoomReservationDtos
                    .Select(mrr =>
                        new MultipurposeRoomReservationViewModel
                        {
                            Id = mrr.Id,
                            ReservationDate = mrr.ReservationDate,
                            Comments = mrr.Comments,
                            Shift = mrr.Shift,
                            WasWellDelivered = mrr.WasWellDelivered,
                            MultipurposeRoomViewModel = new MultipurposeRoomViewModel
                            {
                                Id = mrr.MultipurposeRoomDto.Id,
                                Name = mrr.MultipurposeRoomDto.Name
                            }
                        })
            };

            return View(ownerDashboardViewModel);
        }
    }
}
