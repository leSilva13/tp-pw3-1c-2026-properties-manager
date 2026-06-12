using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProgramacionWeb3.PropertiesManager.Core.Common.Enums;
using ProgramacionWeb3.PropertiesManager.Core.Common.Helpers;
using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using ProgramacionWeb3.PropertiesManager.Web.Models;

namespace ProgramacionWeb3.PropertiesManager.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UsersController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        // GET: /Users/Index
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
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = new SelectList(await _roleService.GetAllAsync(), "Id", "Name");
            return View(new UserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var userDto = new UserDto
                {
                    Email = viewModel.Email,
                    RoleId = (int)UserRoleEnum.Owner,
                    //DESARROLLAR LOGICA DEPENDIENDO SI ES EL ADMIN EL LOGUEADO
                    //SI ES ADMIN SE HABILITAN LOS ROLES, SINO SOLO SE PUEDEN CREAR USUARIOS OWNERS
                    OwnerDto = new OwnerDto { FullName = viewModel.OwnerFullName, Email = viewModel.Email },
                };

                await _userService.CreateUserAsync(userDto);

                //NOTIFICACION POR EMAIL PARA QUE EL USUARIO PUEDA INGRESAR Y CAMBIAR O CREAR SU CONTRASEÑA

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Roles = new SelectList(await _roleService.GetAllAsync(), "Id", "Name");
            return View(viewModel);
        }
    }
}
