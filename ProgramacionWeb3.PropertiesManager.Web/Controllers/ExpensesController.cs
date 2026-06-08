using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using ProgramacionWeb3.PropertiesManager.Web.Models;
using ProgramacionWeb3.PropertiesManager.Web.Services;

namespace ProgramacionWeb3.PropertiesManager.Web.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly IExpenseService _expenseService;
        private readonly IExpenseTypeRepository _expenseTypeRepository;
        private readonly IBuildingRepository _buildingRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly IFileService _fileService;

        public ExpensesController(
            IExpenseService expenseService,
            IExpenseTypeRepository expenseTypeRepository,
            IBuildingRepository buildingRepository,
            IUnitRepository unitRepository,
            IFileService fileService)
        {
            _expenseService = expenseService;
            _expenseTypeRepository = expenseTypeRepository;
            _buildingRepository = buildingRepository;
            _unitRepository = unitRepository;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index(int buildingId)
        {
            var gastos = await _expenseService.ObtenerGastosPorEdificioAsync(buildingId);
            ViewBag.BuildingId = buildingId;
            return View(gastos);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = await ConstruirViewModelAsync();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExpenseFormViewModel viewModel, IFormFile? receiptFile)
        {
            viewModel.Expense.ExpenseTypeId = viewModel.SelectedExpenseTypeId;
            viewModel.Expense.BuildingId = viewModel.SelectedBuildingId;

            ModelState.Remove("Expense.ExpenseType");
            ModelState.Remove("Expense.Building");
            ModelState.Remove("Expense.ExpenseDistributions");

            if (ModelState.IsValid)
            {
                if (receiptFile != null && receiptFile.Length > 0)
                    viewModel.Expense.ReceiptPath = await _fileService.SaveReceiptAsync(receiptFile);

                var unitIds = viewModel.Expense.IsGlobal ? null : viewModel.SelectedUnitIds;
                await _expenseService.CrearGastoAsync(viewModel.Expense, unitIds);
                return RedirectToAction(nameof(Index), new { buildingId = viewModel.Expense.BuildingId });
            }

            viewModel = await ConstruirViewModelAsync(viewModel);
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var gasto = await _expenseService.ObtenerGastoPorIdAsync(id);
            if (gasto == null) return NotFound();

            var viewModel = await ConstruirViewModelAsync();
            viewModel.Expense = gasto;
            viewModel.SelectedExpenseTypeId = gasto.ExpenseTypeId;
            viewModel.SelectedBuildingId = gasto.BuildingId;
            viewModel.SelectedUnitIds = gasto.ExpenseDistributions?.Select(ed => ed.UnitId).ToList() ?? new();
            viewModel.AvailableUnits = await _unitRepository.GetByBuildingIdAsync(gasto.BuildingId);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ExpenseFormViewModel viewModel, IFormFile? receiptFile)
        {
            viewModel.Expense.ExpenseTypeId = viewModel.SelectedExpenseTypeId;
            viewModel.Expense.BuildingId = viewModel.SelectedBuildingId;

            ModelState.Remove("Expense.ExpenseType");
            ModelState.Remove("Expense.Building");
            ModelState.Remove("Expense.ExpenseDistributions");

            if (ModelState.IsValid)
            {
                if (receiptFile != null && receiptFile.Length > 0)
                {
                    _fileService.DeleteReceipt(viewModel.Expense.ReceiptPath);
                    viewModel.Expense.ReceiptPath = await _fileService.SaveReceiptAsync(receiptFile);
                }

                var unitIds = viewModel.Expense.IsGlobal ? null : viewModel.SelectedUnitIds;
                await _expenseService.ActualizarGastoAsync(viewModel.Expense, unitIds);
                return RedirectToAction(nameof(Index), new { buildingId = viewModel.Expense.BuildingId });
            }

            viewModel = await ConstruirViewModelAsync(viewModel);
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var gasto = await _expenseService.ObtenerGastoPorIdAsync(id);
            if (gasto == null) return NotFound();
            return View(gasto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gasto = await _expenseService.ObtenerGastoPorIdAsync(id);
            if (gasto != null)
                _fileService.DeleteReceipt(gasto.ReceiptPath);

            await _expenseService.EliminarGastoAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerUnidadesPorEdificio(int buildingId)
        {
            var unidades = await _unitRepository.GetByBuildingIdAsync(buildingId);
            return Json(unidades.Select(u => new { u.Id, nombre = $"Piso {u.Floor} - Depto {u.ApartmentNumber}" }));
        }

        private async Task<ExpenseFormViewModel> ConstruirViewModelAsync(ExpenseFormViewModel? viewModel = null)
        {
            var tipos = await _expenseTypeRepository.GetAllAsync();
            var edificios = await _buildingRepository.GetAllAsync();

            viewModel ??= new ExpenseFormViewModel();
            viewModel.ExpenseTypes = new SelectList(tipos, "Id", "Name");
            viewModel.Buildings = new SelectList(edificios, "Id", "Id");

            return viewModel;
        }
    }
}
