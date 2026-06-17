using ProgramacionWeb3.PropertiesManager.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgramacionWeb3.PropertiesManager.Core.Entities
{
    [Table("Notificacion")]
    public class Notificacion
    {
        [Key]
        public int IdNotificacion { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(100, ErrorMessage = "El título no puede exceder los 100 caracteres")]
        public string Title { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string Description { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        // Puede ser nulo porque inicialmente no está enviada
        public DateTime? SendDate { get; set; }

        [Required]
        public int IdBuilding { get; set; }

        // Relación con el Consorcio (Navegación)
        [ForeignKey("IdConsorcio")]
        public virtual Building Building { get; set; }
    }
}