using ProgramacionWeb3.PropertiesManager.Core.DTOs;

namespace ProgramacionWeb3.PropertiesManager.Web.Models
{
    public class ExpenseIndexViewModel
    {
        public IEnumerable<ExpenseDto> Gastos { get; set; } = new List<ExpenseDto>();
        public int BuildingId { get; set; }
        public string BuildingLabel { get; set; } = string.Empty;
    }
}
