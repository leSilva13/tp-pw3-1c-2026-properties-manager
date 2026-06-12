using ProgramacionWeb3.PropertiesManager.Core.DTOs;

namespace ProgramacionWeb3.PropertiesManager.Web.Models
{
    public class ExpenseDeleteViewModel
    {
        public ExpenseDto Expense { get; set; } = new ExpenseDto();
        public string BuildingLabel { get; set; } = string.Empty;
    }
}
