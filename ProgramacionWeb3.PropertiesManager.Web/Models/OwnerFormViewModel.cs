using System.ComponentModel.DataAnnotations;

namespace ProgramacionWeb3.PropertiesManager.Web.Models
{
    public class OwnerFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre completo no puede superar los 100 caracteres")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [StringLength(100, ErrorMessage = "El email no puede superar los 100 caracteres")]
        public string Email { get; set; } = string.Empty;
    }
}