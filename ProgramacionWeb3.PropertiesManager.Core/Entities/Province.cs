using System.ComponentModel.DataAnnotations;

namespace ProgramacionWeb3.PropertiesManager.Core.Entities
{
    public class Province
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
