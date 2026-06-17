using Microsoft.AspNetCore.Mvc.Rendering;
using ProgramacionWeb3.PropertiesManager.Web.Models.Unit;
using System.ComponentModel.DataAnnotations;

namespace ProgramacionWeb3.PropertiesManager.Web.Models
{
    public class ExpenseFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La descripción es requerida.")]
        [StringLength(200, ErrorMessage = "La descripción no puede superar los 200 caracteres.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "El monto es requerido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero.")]
        public decimal? Amount { get; set; }

        [Required(ErrorMessage = "El mes es requerido.")]
        [Range(1, 12, ErrorMessage = "El mes debe estar entre 1 y 12.")]
        public int? ExpenseMonth { get; set; }

        [Required(ErrorMessage = "El año es requerido.")]
        [Range(2000, 2100, ErrorMessage = "El año no es válido.")]
        public int? ExpenseYear { get; set; }

        public bool IsGlobal { get; set; } = true;

        [Required(ErrorMessage = "La fecha de facturación es requerida.")]
        public DateTime BillingDate { get; set; }

        public string? ReceiptPath { get; set; }

        [Required(ErrorMessage = "Seleccione un tipo de gasto.")]
        public int? SelectedExpenseTypeId { get; set; }

        public int SelectedBuildingId { get; set; }
        public List<int> SelectedUnitIds { get; set; } = new();
        public string BuildingLabel { get; set; } = string.Empty;

        public SelectList? ExpenseTypes { get; set; }
        public IEnumerable<UnitViewModel> AvailableUnits { get; set; } = new List<UnitViewModel>();
    }
}
