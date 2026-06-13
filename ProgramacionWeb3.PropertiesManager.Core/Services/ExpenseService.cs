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
            var receiptPath = expenseDto.ReceiptPath;
            if (receiptContent != null)
                receiptPath = await _fileService.SaveReceiptAsync(receiptContent, receiptFileName!);

            var expense = new Expense
            {
                Description = expenseDto.Description,
                Amount = expenseDto.Amount,
                ExpenseMonth = expenseDto.ExpenseMonth,
                ExpenseYear = expenseDto.ExpenseYear,
                IsGlobal = expenseDto.IsGlobal,
                BillingDate = expenseDto.BillingDate,
                ReceiptPath = receiptPath,
                ExpenseTypeId = expenseDto.ExpenseTypeId,
                BuildingId = expenseDto.BuildingId,
                CreationDate = DateTime.Now
            };

            await _expenseRepository.AddAsync(expense);
            await _uow.SaveChangesAsync();

            if (!expense.IsGlobal && expenseDto.UnitIds.Any())
            {
                foreach (var unitId in expenseDto.UnitIds)
                {
                    await _expenseDistributionRepository.AddAsync(new ExpenseDistribution
                    {
                        ExpenseId = expense.Id,
                        UnitId = unitId
                    });
                }
                await _uow.SaveChangesAsync();
            }
        }

        public async Task ActualizarGastoAsync(ExpenseDto expenseDto, Stream? receiptContent, string? receiptFileName)
        {
            var expense = await _expenseRepository.GetByIdAsync(expenseDto.Id);
            if (expense == null) return;

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
            expense.BuildingId = expenseDto.BuildingId;

            if (comprobanteNuevo != null)
                expense.ReceiptPath = comprobanteNuevo;

            await _expenseDistributionRepository.DeleteByExpenseIdAsync(expense.Id);

            if (!expense.IsGlobal && expenseDto.UnitIds.Any())
            {
                foreach (var unitId in expenseDto.UnitIds)
                {
                    await _expenseDistributionRepository.AddAsync(new ExpenseDistribution
                    {
                        ExpenseId = expense.Id,
                        UnitId = unitId
                    });
                }
            }

            await _uow.SaveChangesAsync();

            if (comprobanteNuevo != null && !string.IsNullOrEmpty(comprobanteAnterior))
                _fileService.DeleteReceipt(comprobanteAnterior);
        }

        public async Task EliminarGastoAsync(int id)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);
            if (expense == null) return;

            var comprobante = expense.ReceiptPath;
            _expenseRepository.Delete(expense);
            await _uow.SaveChangesAsync();

            _fileService.DeleteReceipt(comprobante);
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
                UnitIds = expense.ExpenseDistributions?.Select(ed => ed.UnitId).ToList() ?? new()
            };
        }
    }
}
