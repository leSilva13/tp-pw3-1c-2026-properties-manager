using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using ProgramacionWeb3.PropertiesManager.Core.Services;
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
            return View(new MultipurposeRoomReservationViewModel { UnitId = unitId });
        }

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Create(MultipurposeRoomReservationViewModel multipurpuseRoomReservationViewModel)
        {
            var multipurposeRoomReservationDto = new MultipurposeRoomReservationDto
            {
                Id = multipurpuseRoomReservationViewModel.Id,
                Comments = multipurpuseRoomReservationViewModel.Comments,
                ReservationDate = multipurpuseRoomReservationViewModel.ReservationDate,
                Shift = multipurpuseRoomReservationViewModel.Shift,
                UnitId = multipurpuseRoomReservationViewModel.UnitId,
                WasWellDelivered = multipurpuseRoomReservationViewModel.WasWellDelivered,
                MultipurposeRoomDto = new MultipurposeRoomDto
                {
                    Id = multipurpuseRoomReservationViewModel.MultipurposeRoomId
                }
            };

            await _multipurposeRoomReservationService.CreateAsync(multipurposeRoomReservationDto);

            return RedirectToAction("Dashboard", "Owners");
        }

        [HttpGet]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Edit(int multipurposeRoomReservationId)
        {
            var multipurposeRoomReservationDto = await _multipurposeRoomReservationService.GetByIdAsync(multipurposeRoomReservationId);
            return View(new MultipurposeRoomReservationViewModel 
            { 
                Id = multipurposeRoomReservationDto.Id,
            });
        }

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Edit(MultipurposeRoomReservationViewModel multipurposeRoomReservationViewModel)
        {
            var multipurposeRoomReservationDto = new MultipurposeRoomReservationDto
            {
                Id = multipurposeRoomReservationViewModel.Id,
            };

            await _multipurposeRoomReservationService.UpdateAsync(multipurposeRoomReservationDto);

            return RedirectToAction("Dashboard", "Owners");
        }
    }
}
