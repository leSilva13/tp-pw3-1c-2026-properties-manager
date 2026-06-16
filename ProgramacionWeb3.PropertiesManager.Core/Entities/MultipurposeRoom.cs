using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgramacionWeb3.PropertiesManager.Core.Entities
{
    public class MultipurposeRoom
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Floor { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public DateTime CreationDate { get; set; } = DateTime.Now;

        [Required]
        public int BuildingId { get; set; }

        [ForeignKey("BuildingId")]
        public virtual Building Building { get; set; }
    }
}
