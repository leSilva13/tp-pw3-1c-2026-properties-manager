using System.ComponentModel.DataAnnotations;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
