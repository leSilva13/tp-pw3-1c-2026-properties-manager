using Microsoft.AspNetCore.Mvc.Rendering;
using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Web.Models
{
    public class ExpenseFormViewModel
    {
        public Expense Expense { get; set; } = new Expense();

        public int SelectedExpenseTypeId { get; set; }
        public int SelectedBuildingId { get; set; }
        public List<int> SelectedUnitIds { get; set; } = new();

        public SelectList? ExpenseTypes { get; set; }
        public SelectList? Buildings { get; set; }
        public IEnumerable<ProgramacionWeb3.PropertiesManager.Core.Entities.Unit> AvailableUnits { get; set; } = new List<ProgramacionWeb3.PropertiesManager.Core.Entities.Unit>();
    }
}
