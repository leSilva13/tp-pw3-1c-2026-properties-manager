using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using ProgramacionWeb3.PropertiesManager.Web.Models;
using ProgramacionWeb3.PropertiesManager.Web.Models.Unit;

namespace ProgramacionWeb3.PropertiesManager.Web.Controllers
{
    [Authorize(Roles = "Properties Administrator")]
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
            var edificio = await ObtenerEdificioPropioAsync(buildingId);
            if (edificio == null) return NotFound();

            var gastos = await _expenseService.ObtenerGastosPorEdificioAsync(buildingId);
            var viewModel = new ExpenseIndexViewModel
            {
                Gastos = gastos,
                BuildingId = buildingId,
                BuildingLabel = ConstruirEtiqueta(edificio)
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int buildingId)
        {
            var edificio = await ObtenerEdificioPropioAsync(buildingId);
            if (edificio == null) return NotFound();

            var viewModel = await ConstruirViewModelAsync();
            viewModel.SelectedBuildingId = buildingId;
            viewModel.BuildingLabel = ConstruirEtiqueta(edificio);
            await CargarUnidadesDisponiblesAsync(viewModel, buildingId);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int buildingId, ExpenseFormViewModel viewModel, IFormFile? receiptFile, string? submitAction)
        {
            viewModel.SelectedBuildingId = buildingId;

            if (!await EsEdificioPropioAsync(buildingId)) return NotFound();

            ValidarComprobante(receiptFile, requerido: true);
            ValidarUnidades(viewModel);

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
            if (!await EsEdificioPropioAsync(gasto.BuildingId)) return NotFound();

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
            // El consorcio del gasto se toma del registro real (no del hidden manipulable)
            var gastoExistente = await _expenseService.ObtenerGastoPorIdAsync(viewModel.Id);
            if (gastoExistente == null) return NotFound();
            if (!await EsEdificioPropioAsync(gastoExistente.BuildingId)) return NotFound();

            viewModel.SelectedBuildingId = gastoExistente.BuildingId;

            ValidarComprobante(receiptFile, requerido: false);
            ValidarUnidades(viewModel);

            if (!ModelState.IsValid)
            {
                await RecargarViewModelAsync(viewModel, viewModel.SelectedBuildingId);
                return View(viewModel);
            }

            var dto = MapearADto(viewModel, viewModel.SelectedBuildingId);
            bool actualizado;
            try
            {
                if (receiptFile != null && receiptFile.Length > 0)
                {
                    using var contenido = receiptFile.OpenReadStream();
                    actualizado = await _expenseService.ActualizarGastoAsync(dto, contenido, receiptFile.FileName);
                }
                else
                {
                    actualizado = await _expenseService.ActualizarGastoAsync(dto, null, null);
                }
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("receiptFile", ex.Message);
                await RecargarViewModelAsync(viewModel, viewModel.SelectedBuildingId);
                return View(viewModel);
            }

            if (!actualizado) return NotFound();

            return RedirectToAction(nameof(Index), new { buildingId = viewModel.SelectedBuildingId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var gasto = await _expenseService.ObtenerGastoPorIdAsync(id);
            if (gasto == null) return NotFound();
            if (!await EsEdificioPropioAsync(gasto.BuildingId)) return NotFound();

            var viewModel = new ExpenseDeleteViewModel
            {
                Expense = gasto,
                BuildingLabel = await ObtenerNombreConsorcioAsync(gasto.BuildingId)
            };
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gasto = await _expenseService.ObtenerGastoPorIdAsync(id);
            if (gasto == null) return NotFound();
            if (!await EsEdificioPropioAsync(gasto.BuildingId)) return NotFound();

            var eliminado = await _expenseService.EliminarGastoAsync(id);
            if (!eliminado) return NotFound();

            return RedirectToAction(nameof(Index), new { buildingId = gasto.BuildingId });
        }

        [HttpGet]
        public async Task<IActionResult> DescargarComprobante(int id)
        {
            var gasto = await _expenseService.ObtenerGastoPorIdAsync(id);
            if (gasto == null) return NotFound();
            if (!await EsEdificioPropioAsync(gasto.BuildingId)) return NotFound();

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

        private void ValidarComprobante(IFormFile? receiptFile, bool requerido)
        {
            if (requerido && (receiptFile == null || receiptFile.Length == 0))
                ModelState.AddModelError("receiptFile", "El comprobante es requerido.");
        }

        private void ValidarUnidades(ExpenseFormViewModel viewModel)
        {
            if (!viewModel.IsGlobal && (viewModel.SelectedUnitIds == null || viewModel.SelectedUnitIds.Count == 0))
                ModelState.AddModelError(string.Empty,
                    "Seleccione al menos una unidad para un gasto específico.");
        }

        private int? GetCurrentUserId()
            => int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;

        // Devuelve el consorcio solo si pertenece al administrador logueado; null en caso contrario.
        private async Task<Building?> ObtenerEdificioPropioAsync(int buildingId)
        {
            var edificio = await _buildingService.ObtenerEdificioPorIdAsync(buildingId);
            if (edificio == null || edificio.UserId != GetCurrentUserId())
                return null;
            return edificio;
        }

        private async Task<bool> EsEdificioPropioAsync(int buildingId)
            => await ObtenerEdificioPropioAsync(buildingId) != null;

        private static string ConstruirEtiqueta(Building edificio)
            => edificio.Address != null
                ? $"{edificio.Address.Street} {edificio.Address.Number}"
                : $"Consorcio #{edificio.Id}";

        private static ExpenseDto MapearADto(ExpenseFormViewModel viewModel, int buildingId)
        {
            return new ExpenseDto
            {
                Id = viewModel.Id,
                Description = viewModel.Description ?? string.Empty,
                Amount = viewModel.Amount ?? 0,
                ExpenseMonth = viewModel.ExpenseMonth ?? 0,
                ExpenseYear = viewModel.ExpenseYear ?? 0,
                IsGlobal = viewModel.IsGlobal,
                BillingDate = viewModel.BillingDate,
                ReceiptPath = viewModel.ReceiptPath,
                ExpenseTypeId = viewModel.SelectedExpenseTypeId ?? 0,
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
            viewModel.BuildingLabel = await ObtenerNombreConsorcioAsync(buildingId);
            await CargarUnidadesDisponiblesAsync(viewModel, buildingId);
        }

        private async Task CargarUnidadesDisponiblesAsync(ExpenseFormViewModel viewModel, int buildingId)
        {
            var unidades = await _unitService.ObtenerUnidadesPorConsorcioAsync(buildingId);
            viewModel.AvailableUnits = unidades.Select(u => new UnitViewModel
            {
                Id = u.Id,
                Floor = u.Floor,
                ApartmentNumber = u.ApartmentNumber
            });
        }

        private async Task<string> ObtenerNombreConsorcioAsync(int buildingId)
        {
            var edificio = await _buildingService.ObtenerEdificioPorIdAsync(buildingId);
            return edificio != null ? ConstruirEtiqueta(edificio) : $"Consorcio #{buildingId}";
        }
    }
}
