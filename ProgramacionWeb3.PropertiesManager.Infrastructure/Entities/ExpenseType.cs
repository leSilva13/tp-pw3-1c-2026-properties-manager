using System.ComponentModel.DataAnnotations;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Entities
{
    public class ExpenseType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
