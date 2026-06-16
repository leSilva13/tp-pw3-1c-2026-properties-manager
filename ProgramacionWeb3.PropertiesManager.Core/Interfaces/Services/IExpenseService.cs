using System.IO;
using ProgramacionWeb3.PropertiesManager.Core.DTOs;

namespace ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services
{
    public interface IExpenseService
    {
        Task<IEnumerable<ExpenseDto>> ObtenerGastosPorEdificioAsync(int buildingId);
        Task<ExpenseDto?> ObtenerGastoPorIdAsync(int id);
        Task CrearGastoAsync(ExpenseDto expenseDto, Stream? receiptContent, string? receiptFileName);
        Task<bool> ActualizarGastoAsync(ExpenseDto expenseDto, Stream? receiptContent, string? receiptFileName);
        Task<bool> EliminarGastoAsync(int id);
        Task<string?> ObtenerRutaComprobanteAsync(int id);
    }
}
