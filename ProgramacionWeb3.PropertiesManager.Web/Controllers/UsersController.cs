using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProgramacionWeb3.PropertiesManager.Core.Common.Enums;
using ProgramacionWeb3.PropertiesManager.Core.Common.Helpers;
using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using ProgramacionWeb3.PropertiesManager.Web.Models;

namespace ProgramacionWeb3.PropertiesManager.Web.Controllers
{
    [Authorize(Roles = "Admin,Properties Administrator")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UsersController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Properties Administrator")]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();

            return View(users.Select(u => 
                new UserViewModel 
                { 
                    Id = u.Id,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    RoleName = ((UserRoleEnum)u.RoleId).ToEnumMemberString(),
                }));
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Properties Administrator")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = new SelectList(await _roleService.GetAllAsync(), "Id", "Name");
            return View(new UserViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Properties Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var userDto = new UserDto
                {
                    Email = viewModel.Email,
                    RoleId = User.IsInRole(UserRoleEnum.Admin.ToEnumMemberString()) ? viewModel.RoleId : (int)UserRoleEnum.Owner,
                    OwnerDto = viewModel.RoleId == (int)UserRoleEnum.Owner 
                        ? new OwnerDto { FullName = viewModel.OwnerFullName, Email = viewModel.Email } 
                        : null,
                };

                await _userService.CreateUserAsync(userDto);

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Roles = new SelectList(await _roleService.GetAllAsync(), "Id", "Name");
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Properties Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Properties Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(int id)
        {
            await _userService.ActivateUserAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
