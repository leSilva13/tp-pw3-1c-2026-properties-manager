using System.IO;
using ProgramacionWeb3.PropertiesManager.Core.DTOs;
using ProgramacionWeb3.PropertiesManager.Core.Entities;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;

namespace ProgramacionWeb3.PropertiesManager.Core.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IUnitOfWork _uow;
        private readonly IExpenseRepository _expenseRepository;
        private readonly IExpenseDistributionRepository _expenseDistributionRepository;
        private readonly IFileService _fileService;

        public ExpenseService(IUnitOfWork uow, IExpenseRepository expenseRepository, IExpenseDistributionRepository expenseDistributionRepository, IFileService fileService)
        {
            _uow = uow;
            _expenseRepository = expenseRepository;
            _expenseDistributionRepository = expenseDistributionRepository;
            _fileService = fileService;
        }

        public async Task<IEnumerable<ExpenseDto>> ObtenerGastosPorEdificioAsync(int buildingId)
        {
            var gastos = await _expenseRepository.GetByBuildingIdAsync(buildingId);
            return gastos.Select(MapearADto).ToList();
        }

        public async Task<ExpenseDto?> ObtenerGastoPorIdAsync(int id)
        {
            var gasto = await _expenseRepository.GetByIdWithRelationshipsAsync(id);
            return gasto == null ? null : MapearADto(gasto);
        }

        public async Task CrearGastoAsync(ExpenseDto expenseDto, Stream? receiptContent, string? receiptFileName)
        {
            string? comprobanteNuevo = null;
            if (receiptContent != null)
                comprobanteNuevo = await _fileService.SaveReceiptAsync(receiptContent, receiptFileName!);

            var expense = new Expense
            {
                Description = expenseDto.Description,
                Amount = expenseDto.Amount,
                ExpenseMonth = expenseDto.ExpenseMonth,
                ExpenseYear = expenseDto.ExpenseYear,
                IsGlobal = expenseDto.IsGlobal,
                BillingDate = expenseDto.BillingDate,
                ReceiptPath = comprobanteNuevo ?? expenseDto.ReceiptPath,
                ExpenseTypeId = expenseDto.ExpenseTypeId,
                BuildingId = expenseDto.BuildingId,
                CreationDate = DateTime.UtcNow
            };

            if (!expense.IsGlobal && expenseDto.UnitIds.Any())
                expense.ExpenseDistributions = expenseDto.UnitIds
                    .Select(unitId => new ExpenseDistribution { UnitId = unitId })
                    .ToList();

            try
            {
                await _expenseRepository.AddAsync(expense);
                await _uow.SaveChangesAsync();
            }
            catch
            {
                if (comprobanteNuevo != null)
                    _fileService.DeleteReceipt(comprobanteNuevo);
                throw;
            }
        }

        public async Task<bool> ActualizarGastoAsync(ExpenseDto expenseDto, Stream? receiptContent, string? receiptFileName)
        {
            var expense = await _expenseRepository.GetByIdAsync(expenseDto.Id);
            if (expense == null) return false;

            var comprobanteAnterior = expense.ReceiptPath;
            string? comprobanteNuevo = null;
            if (receiptContent != null)
                comprobanteNuevo = await _fileService.SaveReceiptAsync(receiptContent, receiptFileName!);

            expense.Description = expenseDto.Description;
            expense.Amount = expenseDto.Amount;
            expense.ExpenseMonth = expenseDto.ExpenseMonth;
            expense.ExpenseYear = expenseDto.ExpenseYear;
            expense.IsGlobal = expenseDto.IsGlobal;
            expense.BillingDate = expenseDto.BillingDate;
            expense.ExpenseTypeId = expenseDto.ExpenseTypeId;

            if (comprobanteNuevo != null)
                expense.ReceiptPath = comprobanteNuevo;

            try
            {
                // Sincronización por diferencia: solo se borran las unidades que dejan de aplicar
                // y solo se insertan las nuevas. Evita el patrón borrar-todo + reinsertar, que con el
                // índice único (ExpenseId, UnitId) podría chocar al re-crear una clave existente.
                var distribucionesActuales = (await _expenseDistributionRepository.GetByExpenseIdAsync(expense.Id)).ToList();

                var unidadesDeseadas = !expense.IsGlobal
                    ? expenseDto.UnitIds.Distinct().ToHashSet()
                    : new HashSet<int>();

                var unidadesActuales = distribucionesActuales.Select(d => d.UnitId).ToHashSet();

                foreach (var distribucion in distribucionesActuales.Where(d => !unidadesDeseadas.Contains(d.UnitId)))
                    _expenseDistributionRepository.Delete(distribucion);

                foreach (var unitId in unidadesDeseadas.Where(id => !unidadesActuales.Contains(id)))
                    await _expenseDistributionRepository.AddAsync(new ExpenseDistribution
                    {
                        ExpenseId = expense.Id,
                        UnitId = unitId
                    });

                await _uow.SaveChangesAsync();
            }
            catch
            {
                if (comprobanteNuevo != null)
                    _fileService.DeleteReceipt(comprobanteNuevo);
                throw;
            }

            if (comprobanteNuevo != null && !string.IsNullOrEmpty(comprobanteAnterior))
                _fileService.DeleteReceipt(comprobanteAnterior);

            return true;
        }

        public async Task<bool> EliminarGastoAsync(int id)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);
            if (expense == null) return false;

            var comprobante = expense.ReceiptPath;
            _expenseRepository.Delete(expense);
            await _uow.SaveChangesAsync();

            _fileService.DeleteReceipt(comprobante);

            return true;
        }

        public async Task<string?> ObtenerRutaComprobanteAsync(int id)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);
            if (expense == null || string.IsNullOrEmpty(expense.ReceiptPath))
                return null;

            var fullPath = _fileService.GetReceiptPhysicalPath(expense.ReceiptPath);
            return fullPath != null && File.Exists(fullPath) ? fullPath : null;
        }

        private static ExpenseDto MapearADto(Expense expense)
        {
            return new ExpenseDto
            {
                Id = expense.Id,
                Description = expense.Description,
                Amount = expense.Amount,
                ExpenseMonth = expense.ExpenseMonth,
                ExpenseYear = expense.ExpenseYear,
                IsGlobal = expense.IsGlobal,
                BillingDate = expense.BillingDate,
                ReceiptPath = expense.ReceiptPath,
                CreationDate = expense.CreationDate,
                ExpenseTypeId = expense.ExpenseTypeId,
                ExpenseTypeName = expense.ExpenseType?.Name,
                BuildingId = expense.BuildingId,
                BuildingName = expense.Building?.Address != null ? BuildingName(expense.Building.Address) : string.Empty,
                UnitIds = expense.ExpenseDistributions?.Select(ed => ed.UnitId).ToList() ?? new()
            };
        }

        private static string BuildingName(Address address)
        {
            return string.Format("{0} {1}, {2}, {3}",
                address.Street, address.Number, address.Locality.Name, address.Province.Name);
        }

        public async Task<IEnumerable<ExpenseDto>> GetAllByOwner(int ownerId)
        {
            var expenses = await _expenseRepository.GetByOwnerAsync(ownerId);
            return expenses.Select(e => MapearADto(e));
        }
    }
}
