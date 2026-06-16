using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using ProgramacionWeb3.PropertiesManager.Web.Models;

namespace ProgramacionWeb3.PropertiesManager.Web.Controllers
{
    public class MultipurposeRoomReservationsController : Controller
    {
        private readonly IMultipurposeRoomService _multipurposeRoomService;
        private readonly IMultipurposeRoomReservationService _multipurposeRoomReservationService;

        public MultipurposeRoomReservationsController(IMultipurposeRoomReservationService multipurposeRoomReservationService, IMultipurposeRoomService multipurposeRoomService)
        {
            _multipurposeRoomService = multipurposeRoomService;
            _multipurposeRoomReservationService = multipurposeRoomReservationService;
        }

        [Authorize(Roles = "Properties Administrator")]
        public async Task<IActionResult> Index(int buildingId)
        {
            ViewBag.buildingId = buildingId;
            var multipurposeRoomReservations = await _multipurposeRoomReservationService.GetAllByBuildingAsync(buildingId);

            return View(multipurposeRoomReservations.Select(mrr => new MultipurposeRoomReservationViewModel { Id = mrr.Id }).ToList());
        }

        [HttpGet]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Create(int unitId, int buildingId)
        {
            ViewBag.MultipurposeRooms = new SelectList(await _multipurposeRoomService.GetAllByBuildingAsync(buildingId), "Id", "Name");
            return View(new MultipurposeRoomReservationViewModel { UnitId = unitId, BuildingId = buildingId });
        }

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Create(MultipurposeRoomReservationViewModel multipurposeRoomReservationViewModel)
        {
            var multipurposeRoomReservationDto = new MultipurposeRoomReservationDto
            {
                Id = multipurposeRoomReservationViewModel.Id,
                Comments = multipurposeRoomReservationViewModel.Comments,
                ReservationDate = multipurposeRoomReservationViewModel.ReservationDate,
                Shift = multipurposeRoomReservationViewModel.Shift,
                UnitId = multipurposeRoomReservationViewModel.UnitId,
                WasWellDelivered = multipurposeRoomReservationViewModel.WasWellDelivered,
                MultipurposeRoomDto = new MultipurposeRoomDto
                {
                    Id = multipurposeRoomReservationViewModel.MultipurposeRoomId
                }
            };

            var response = await _multipurposeRoomReservationService.CreateAsync(multipurposeRoomReservationDto);

            if (response.IsSuccess)
            {
                TempData["SuccessMessage"] = response.Message ?? "¡Reserva confirmada con éxito!";
                return RedirectToAction("Dashboard", "Owners");
            }
            else
            {
                TempData["ErrorMessage"] = response.Message ?? "Ocurrio un error!";
                ViewBag.MultipurposeRooms = new SelectList(await _multipurposeRoomService.GetAllByBuildingAsync(multipurposeRoomReservationViewModel.BuildingId), "Id", "Name");
                return View("Create", multipurposeRoomReservationViewModel);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Edit(int multipurposeRoomReservationId)
        {
            var multipurposeRoomReservationDto = await _multipurposeRoomReservationService.GetByIdAsync(multipurposeRoomReservationId);
            ViewBag.MultipurposeRooms = new SelectList(await _multipurposeRoomService.GetAllByBuildingAsync(multipurposeRoomReservationDto.BuildingId), "Id", "Name");

            return View(new MultipurposeRoomReservationViewModel 
            { 
                Id = multipurposeRoomReservationDto.Id,
                Comments = multipurposeRoomReservationDto.Comments,
                ReservationDate = multipurposeRoomReservationDto.ReservationDate,
                Shift = multipurposeRoomReservationDto.Shift,
                UnitId = multipurposeRoomReservationDto.UnitId,
                WasWellDelivered = multipurposeRoomReservationDto.WasWellDelivered,
                BuildingId = multipurposeRoomReservationDto.BuildingId,
                MultipurposeRoomId = multipurposeRoomReservationDto.MultipurposeRoomDto.Id,
                MultipurposeRoomViewModel = new MultipurposeRoomViewModel
                {
                    Id = multipurposeRoomReservationDto.MultipurposeRoomDto.Id,
                }
            });
        }

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Edit(MultipurposeRoomReservationViewModel multipurposeRoomReservationViewModel)
        {
            var multipurposeRoomReservationDto = new MultipurposeRoomReservationDto
            {
                Id = multipurposeRoomReservationViewModel.Id,
                Comments = multipurposeRoomReservationViewModel.Comments,
                ReservationDate = multipurposeRoomReservationViewModel.ReservationDate,
                Shift = multipurposeRoomReservationViewModel.Shift,
                UnitId = multipurposeRoomReservationViewModel.UnitId,
                WasWellDelivered = multipurposeRoomReservationViewModel.WasWellDelivered,
                MultipurposeRoomDto = new MultipurposeRoomDto
                {
                    Id = multipurposeRoomReservationViewModel.MultipurposeRoomId
                }
            };

            var response = await _multipurposeRoomReservationService.UpdateAsync(multipurposeRoomReservationDto);

            if (response.IsSuccess)
            {
                TempData["SuccessMessage"] = response.Message ?? "¡Reserva confirmada con éxito!";
                return RedirectToAction("Dashboard", "Owners");
            }
            else
            {
                TempData["ErrorMessage"] = response.Message ?? "Ocurrio un error!";
                ViewBag.MultipurposeRooms = new SelectList(await _multipurposeRoomService.GetAllByBuildingAsync(multipurposeRoomReservationViewModel.BuildingId), "Id", "Name");
                return View("Edit", multipurposeRoomReservationViewModel);
            }
        }


        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Delete(int multipurposeRoomReservationId)
        {
            var response = await _multipurposeRoomReservationService.DeleteAsync(multipurposeRoomReservationId);

            if (response.IsSuccess)
                TempData["SuccessMessage"] = response.Message;
            else
                TempData["ErrorMessage"] = response.Message;

            return RedirectToAction("Dashboard", "Owners");
        }
    }
}
