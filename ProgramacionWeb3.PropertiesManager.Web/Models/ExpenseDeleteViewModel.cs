using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Web.Models
{
    public class ExpenseDeleteViewModel
    {
        public Expense Expense { get; set; } = new Expense();
        public string BuildingLabel { get; set; } = string.Empty;
    }
}
