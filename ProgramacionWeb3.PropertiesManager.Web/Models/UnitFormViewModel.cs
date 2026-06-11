using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ProgramacionWeb3.PropertiesManager.Web.Models
{
    public class UnitFormViewModel
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El consorcio es obligatorio")]
        public int BuildingId { get; set; }

        public string? BuildingName { get; set; }

        [Required(ErrorMessage = "El piso es obligatorio")]
        [StringLength(10, ErrorMessage = "El piso no puede superar los 10 caracteres")]
        public string Floor { get; set; } = string.Empty;

        [Required(ErrorMessage = "El departamento es obligatorio")]
        [StringLength(10, ErrorMessage = "El departamento no puede superar los 10 caracteres")]
        public string ApartmentNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "La superficie es obligatoria")]
        [Range(1, 1000000, ErrorMessage = "La superficie debe ser mayor a cero")]
        public decimal AreaInMeters { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un dueño")]
        public int OwnerId { get; set; }

        public SelectList? Owners { get; set; }
    }
}