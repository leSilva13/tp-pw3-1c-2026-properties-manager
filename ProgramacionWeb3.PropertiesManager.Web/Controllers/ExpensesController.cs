using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using ProgramacionWeb3.PropertiesManager.Web.Models;

namespace ProgramacionWeb3.PropertiesManager.Web.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly IExpenseService _expenseService;
        private readonly IBuildingService _buildingService;
        private readonly IExpenseTypeService _expenseTypeService;
        private readonly IUnitService _unitService;

        public ExpensesController(
            IExpenseService expenseService,
            IBuildingService buildingService,
            IExpenseTypeService expenseTypeService,
            IUnitService unitService)
        {
            _expenseService = expenseService;
            _buildingService = buildingService;
            _expenseTypeService = expenseTypeService;
            _unitService = unitService;
        }

        public async Task<IActionResult> Index(int buildingId)
        {
            var gastos = await _expenseService.ObtenerGastosPorEdificioAsync(buildingId);
            var viewModel = new ExpenseIndexViewModel
            {
                Gastos = gastos,
                BuildingId = buildingId,
                BuildingLabel = await ObtenerNombreConsorcioAsync(buildingId)
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
            viewModel.BuildingLabel = edificio.Address != null
                ? $"{edificio.Address.Street} {edificio.Address.Number}"
                : $"Consorcio #{buildingId}";
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int buildingId, ExpenseFormViewModel viewModel, IFormFile? receiptFile, string? submitAction)
        {
            viewModel.SelectedBuildingId = buildingId;

            ValidarComprobante(receiptFile, requerido: true);

            if (!ModelState.IsValid)
            {
                await RecargarViewModelAsync(viewModel, buildingId);
                return View(viewModel);
            }

            var dto = MapearADto(viewModel, buildingId);
            try
            {
                using var contenido = receiptFile!.OpenReadStream();
                await _expenseService.CrearGastoAsync(dto, contenido, receiptFile.FileName);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("receiptFile", ex.Message);
                await RecargarViewModelAsync(viewModel, buildingId);
                return View(viewModel);
            }

            if (submitAction == "saveAndCreate")
            {
                TempData["SuccessMessage"] = $"Gasto \"{viewModel.Description}\" creado con éxito.";
                return RedirectToAction(nameof(Create), new { buildingId });
            }

            return RedirectToAction(nameof(Index), new { buildingId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var gasto = await _expenseService.ObtenerGastoPorIdAsync(id);
            if (gasto == null) return NotFound();

            var viewModel = new ExpenseFormViewModel
            {
                Id = gasto.Id,
                Description = gasto.Description,
                Amount = gasto.Amount,
                ExpenseMonth = gasto.ExpenseMonth,
                ExpenseYear = gasto.ExpenseYear,
                IsGlobal = gasto.IsGlobal,
                BillingDate = gasto.BillingDate,
                ReceiptPath = gasto.ReceiptPath,
                SelectedExpenseTypeId = gasto.ExpenseTypeId,
                SelectedBuildingId = gasto.BuildingId,
                SelectedUnitIds = gasto.UnitIds
            };
            await RecargarViewModelAsync(viewModel, gasto.BuildingId);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ExpenseFormViewModel viewModel, IFormFile? receiptFile)
        {
            ValidarComprobante(receiptFile, requerido: false);

            if (!ModelState.IsValid)
            {
                await RecargarViewModelAsync(viewModel, viewModel.SelectedBuildingId);
                return View(viewModel);
            }

            var dto = MapearADto(viewModel, viewModel.SelectedBuildingId);
            try
            {
                if (receiptFile != null && receiptFile.Length > 0)
                {
                    using var contenido = receiptFile.OpenReadStream();
                    await _expenseService.ActualizarGastoAsync(dto, contenido, receiptFile.FileName);
                }
                else
                {
                    await _expenseService.ActualizarGastoAsync(dto, null, null);
                }
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("receiptFile", ex.Message);
                await RecargarViewModelAsync(viewModel, viewModel.SelectedBuildingId);
                return View(viewModel);
            }

            return RedirectToAction(nameof(Index), new { buildingId = viewModel.SelectedBuildingId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var gasto = await _expenseService.ObtenerGastoPorIdAsync(id);
            if (gasto == null) return NotFound();

            var viewModel = new ExpenseDeleteViewModel
            {
                Expense = gasto,
                BuildingLabel = await ObtenerNombreConsorcioAsync(gasto.BuildingId)
            };
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int buildingId)
        {
            await _expenseService.EliminarGastoAsync(id);

            return RedirectToAction(nameof(Index), new { buildingId });
        }

        [HttpGet]
        public async Task<IActionResult> DescargarComprobante(int id)
        {
            var filePath = await _expenseService.ObtenerRutaComprobanteAsync(id);
            if (filePath == null) return NotFound();

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
            var unidades = await _unitService.ObtenerUnidadesPorConsorcioAsync(buildingId);
            return Json(unidades.Select(u => new { id = u.Id, nombre = $"Piso {u.Floor} - Depto {u.ApartmentNumber}" }));
        }

        private void ValidarComprobante(IFormFile? receiptFile, bool requerido)
        {
            if (requerido && (receiptFile == null || receiptFile.Length == 0))
                ModelState.AddModelError("receiptFile", "El comprobante es requerido.");
        }

        private static ExpenseDto MapearADto(ExpenseFormViewModel viewModel, int buildingId)
        {
            return new ExpenseDto
            {
                Id = viewModel.Id,
                Description = viewModel.Description ?? string.Empty,
                Amount = viewModel.Amount,
                ExpenseMonth = viewModel.ExpenseMonth,
                ExpenseYear = viewModel.ExpenseYear,
                IsGlobal = viewModel.IsGlobal,
                BillingDate = viewModel.BillingDate,
                ReceiptPath = viewModel.ReceiptPath,
                ExpenseTypeId = viewModel.SelectedExpenseTypeId,
                BuildingId = buildingId,
                UnitIds = viewModel.SelectedUnitIds
            };
        }

        private async Task<ExpenseFormViewModel> ConstruirViewModelAsync(ExpenseFormViewModel? viewModel = null)
        {
            var tipos = await _expenseTypeService.ObtenerTiposDeGastoAsync();
            viewModel ??= new ExpenseFormViewModel();
            viewModel.ExpenseTypes = new SelectList(tipos, "Id", "Name");
            return viewModel;
        }

        private async Task RecargarViewModelAsync(ExpenseFormViewModel viewModel, int buildingId)
        {
            await ConstruirViewModelAsync(viewModel);
            viewModel.SelectedBuildingId = buildingId;
            viewModel.AvailableUnits = await _unitService.ObtenerUnidadesPorConsorcioAsync(buildingId);
            viewModel.BuildingLabel = await ObtenerNombreConsorcioAsync(buildingId);
        }

        private async Task<string> ObtenerNombreConsorcioAsync(int buildingId)
        {
            var edificio = await _buildingService.ObtenerEdificioPorIdAsync(buildingId);
            return edificio?.Address != null
                ? $"{edificio.Address.Street} {edificio.Address.Number}"
                : $"Consorcio #{buildingId}";
        }
    }
}
