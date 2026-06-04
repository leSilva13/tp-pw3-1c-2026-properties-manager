using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;

namespace ProgramacionWeb3.PropertiesManager.Web.Controllers
{
    //[Authorize]
    public class BuildingsController : Controller
    {
        private readonly IBuildingService _buildingService;

        public BuildingsController(IBuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        // GET: /Buildings o /Buildings/Index
        public  IActionResult Index()
        {
           

            return View();
        }
    }
}