using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Web.Models
{
    public class ExpenseIndexViewModel
    {
        public IEnumerable<Expense> Gastos { get; set; } = new List<Expense>();
        public int BuildingId { get; set; }
        public string BuildingLabel { get; set; } = string.Empty;
    }
}
