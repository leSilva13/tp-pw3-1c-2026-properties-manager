using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using ProgramacionWeb3.PropertiesManager.Web.Models;

namespace ProgramacionWeb3.PropertiesManager.Web.Controllers
{
    public class MultipurposeRoomsController : Controller
    {
        private readonly IMultipurposeRoomService _multipurposeRoomService;

        public MultipurposeRoomsController(IMultipurposeRoomService multipurposeRoomService) => _multipurposeRoomService = multipurposeRoomService;

        [Authorize(Roles = "Properties Administrator, Owner")]
        public async Task<IActionResult> Index(int buildingId)
        {
            ViewBag.buildingId = buildingId;
            var multipurposeRooms = await _multipurposeRoomService.GetAllByBuildingAsync(buildingId);

            return View(multipurposeRooms.Select(mr => new MultipurposeRoomViewModel { Id = mr.Id, Name = mr.Name, Floor = mr.Floor, IsActive = mr.IsActive, BuildingId = mr.BuildingId }).ToList());
        }

        [HttpGet]
        [Authorize(Roles = "Properties Administrator")]
        public async Task<IActionResult> Create(int buildingId)
        {
            return View(new MultipurposeRoomViewModel { BuildingId = buildingId });
        }

        [HttpPost]
        [Authorize(Roles = "Properties Administrator")]
        public async Task<IActionResult> Create(MultipurposeRoomViewModel multipurpuseRoomViewModel)
        {
            var multipurposeRoom = new MultipurposeRoomDto
            {
                BuildingId = multipurpuseRoomViewModel.BuildingId,
                Floor = multipurpuseRoomViewModel.Floor,
                Name = multipurpuseRoomViewModel.Name,
            };

            await _multipurposeRoomService.CreateAsync(multipurposeRoom);

            return RedirectToAction("Index", new { buildingId = multipurpuseRoomViewModel.BuildingId });
        }

        [HttpGet]
        [Authorize(Roles = "Properties Administrator")]
        public async Task<IActionResult> Edit(int multipurposeRoomId)
        {
            var multipurposeRoomDto = await _multipurposeRoomService.GetByIdAsync(multipurposeRoomId);
            return View(new MultipurposeRoomViewModel 
            { 
                Id = multipurposeRoomDto.Id,
                Name = multipurposeRoomDto.Name,
                Floor = multipurposeRoomDto.Floor,
                IsActive = multipurposeRoomDto.IsActive,
                BuildingId = multipurposeRoomDto.BuildingId,
            });
        }

        [HttpPost]
        [Authorize(Roles = "Properties Administrator")]
        public async Task<IActionResult> Edit(MultipurposeRoomViewModel multipurpuseRoomViewModel)
        {
            var multipurposeRoom = new MultipurposeRoomDto
            {
                Id = multipurpuseRoomViewModel.Id,
                Name = multipurpuseRoomViewModel.Name,
                Floor = multipurpuseRoomViewModel.Floor,
                IsActive = multipurpuseRoomViewModel.IsActive,
                BuildingId = multipurpuseRoomViewModel.BuildingId,
            };

            await _multipurposeRoomService.UpdateAsync(multipurposeRoom);

            return RedirectToAction("Index", new { buildingId = multipurpuseRoomViewModel.BuildingId });
        }
    }
}
