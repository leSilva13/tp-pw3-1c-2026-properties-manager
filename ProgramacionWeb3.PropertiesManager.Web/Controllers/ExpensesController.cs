using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using ProgramacionWeb3.PropertiesManager.Web.Models;
using ProgramacionWeb3.PropertiesManager.Web.Services;

namespace ProgramacionWeb3.PropertiesManager.Web.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly IExpenseService _expenseService;
        private readonly IBuildingService _buildingService;
        private readonly IUnitOfWork _uow;
        private readonly IFileService _fileService;

        public ExpensesController(
            IExpenseService expenseService,
            IBuildingService buildingService,
            IUnitOfWork uow,
            IFileService fileService)
        {
            _expenseService = expenseService;
            _buildingService = buildingService;
            _uow = uow;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index(int buildingId)
        {
            var gastos = await _expenseService.ObtenerGastosPorEdificioAsync(buildingId);
            var edificio = await _buildingService.ObtenerEdificioPorIdAsync(buildingId);
            var viewModel = new ExpenseIndexViewModel
            {
                Gastos = gastos,
                BuildingId = buildingId,
                BuildingLabel = edificio != null ? $"{edificio.Address.Street} {edificio.Address.Number}" : $"Consorcio #{buildingId}"
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int buildingId)
        {
            var edificio = await _buildingService.ObtenerEdificioPorIdAsync(buildingId);
            if (edificio == null) return NotFound();

            var viewModel = await ConstruirViewModelAsync();
            viewModel.SelectedBuildingId = buildingId;
            viewModel.BuildingLabel = $"{edificio.Address.Street} {edificio.Address.Number}";
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int buildingId, ExpenseFormViewModel viewModel, IFormFile? receiptFile, string? submitAction)
        {
            viewModel.Expense.ExpenseTypeId = viewModel.SelectedExpenseTypeId;
            viewModel.Expense.BuildingId = buildingId;
            viewModel.SelectedBuildingId = buildingId;

            ModelState.Remove("Expense.ExpenseType");
            ModelState.Remove("Expense.Building");
            ModelState.Remove("Expense.ExpenseDistributions");

            if (receiptFile == null || receiptFile.Length == 0)
                ModelState.AddModelError("receiptFile", "El comprobante es requerido.");

            if (ModelState.IsValid)
            {
                try
                {
                    viewModel.Expense.ReceiptPath = await _fileService.SaveReceiptAsync(receiptFile!);
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("receiptFile", ex.Message);
                    var ed = await _buildingService.ObtenerEdificioPorIdAsync(buildingId);
                    viewModel = await ConstruirViewModelAsync(viewModel);
                    viewModel.SelectedBuildingId = buildingId;
                    viewModel.BuildingLabel = ed != null ? $"{ed.Address.Street} {ed.Address.Number}" : $"Consorcio #{buildingId}";
                    return View(viewModel);
                }

                var unitIds = viewModel.Expense.IsGlobal ? null : viewModel.SelectedUnitIds;
                await _expenseService.CrearGastoAsync(viewModel.Expense, unitIds);

                if (submitAction == "saveAndCreate")
                {
                    TempData["SuccessMessage"] = $"Gasto \"{viewModel.Expense.Description}\" creado con éxito.";
                    return RedirectToAction(nameof(Create), new { buildingId });
                }

                return RedirectToAction(nameof(Index), new { buildingId });
            }

            var edificio = await _buildingService.ObtenerEdificioPorIdAsync(buildingId);
            viewModel = await ConstruirViewModelAsync(viewModel);
            viewModel.SelectedBuildingId = buildingId;
            viewModel.BuildingLabel = edificio != null ? $"{edificio.Address.Street} {edificio.Address.Number}" : $"Consorcio #{buildingId}";
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var gasto = await _expenseService.ObtenerGastoPorIdAsync(id);
            if (gasto == null) return NotFound();

            var edificio = await _buildingService.ObtenerEdificioPorIdAsync(gasto.BuildingId);
            var viewModel = await ConstruirViewModelAsync();
            viewModel.Expense = gasto;
            viewModel.SelectedExpenseTypeId = gasto.ExpenseTypeId;
            viewModel.SelectedBuildingId = gasto.BuildingId;
            viewModel.BuildingLabel = edificio != null ? $"{edificio.Address.Street} {edificio.Address.Number}" : $"Consorcio #{gasto.BuildingId}";
            viewModel.SelectedUnitIds = gasto.ExpenseDistributions?.Select(ed => ed.UnitId).ToList() ?? new();
            viewModel.AvailableUnits = await _uow.Units.GetByBuildingIdAsync(gasto.BuildingId);
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
                    try
                    {
                        var oldPath = viewModel.Expense.ReceiptPath;
                        viewModel.Expense.ReceiptPath = await _fileService.SaveReceiptAsync(receiptFile);
                        var unitIds = viewModel.Expense.IsGlobal ? null : viewModel.SelectedUnitIds;
                        await _expenseService.ActualizarGastoAsync(viewModel.Expense, unitIds);
                        _fileService.DeleteReceipt(oldPath);
                    }
                    catch (InvalidOperationException ex)
                    {
                        ModelState.AddModelError("receiptFile", ex.Message);
                        viewModel = await ConstruirViewModelAsync(viewModel);
                        viewModel.AvailableUnits = await _uow.Units.GetByBuildingIdAsync(viewModel.SelectedBuildingId);
                        viewModel.BuildingLabel = $"Consorcio #{viewModel.SelectedBuildingId}";
                        return View(viewModel);
                    }
                }
                else
                {
                    var unitIds = viewModel.Expense.IsGlobal ? null : viewModel.SelectedUnitIds;
                    await _expenseService.ActualizarGastoAsync(viewModel.Expense, unitIds);
                }

                return RedirectToAction(nameof(Index), new { buildingId = viewModel.Expense.BuildingId });
            }

            viewModel = await ConstruirViewModelAsync(viewModel);
            viewModel.AvailableUnits = await _uow.Units.GetByBuildingIdAsync(viewModel.SelectedBuildingId);
            viewModel.BuildingLabel = $"Consorcio #{viewModel.SelectedBuildingId}";
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var gasto = await _expenseService.ObtenerGastoPorIdAsync(id);
            if (gasto == null) return NotFound();

            var edificio = await _buildingService.ObtenerEdificioPorIdAsync(gasto.BuildingId);
            var viewModel = new ExpenseDeleteViewModel
            {
                Expense = gasto,
                BuildingLabel = edificio != null ? $"{edificio.Address.Street} {edificio.Address.Number}" : $"Consorcio #{gasto.BuildingId}"
            };
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gasto = await _expenseService.ObtenerGastoPorIdAsync(id);
            int buildingId = gasto?.BuildingId ?? 0;
            var receiptPath = gasto?.ReceiptPath;

            await _expenseService.EliminarGastoAsync(id);
            _fileService.DeleteReceipt(receiptPath);

            return RedirectToAction(nameof(Index), new { buildingId });
        }

        [HttpGet]
        public async Task<IActionResult> DescargarComprobante(int id)
        {
            var gasto = await _expenseService.ObtenerGastoPorIdAsync(id);
            if (gasto == null || string.IsNullOrEmpty(gasto.ReceiptPath)) return NotFound();

            var filePath = _fileService.GetReceiptPhysicalPath(gasto.ReceiptPath);
            if (filePath == null || !System.IO.File.Exists(filePath)) return NotFound();

            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            var contentType = extension switch
            {
                ".pdf" => "application/pdf",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };

            return PhysicalFile(filePath, contentType, Path.GetFileName(filePath));
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerUnidadesPorEdificio(int buildingId)
        {
            var unidades = await _uow.Units.GetByBuildingIdAsync(buildingId);
            return Json(unidades.Select(u => new { u.Id, nombre = $"Piso {u.Floor} - Depto {u.ApartmentNumber}" }));
        }

        private async Task<ExpenseFormViewModel> ConstruirViewModelAsync(ExpenseFormViewModel? viewModel = null)
        {
            var tipos = await _uow.ExpenseTypes.GetAllAsync();
            viewModel ??= new ExpenseFormViewModel();
            viewModel.ExpenseTypes = new SelectList(tipos, "Id", "Name");
            return viewModel;
        }
    }
}
