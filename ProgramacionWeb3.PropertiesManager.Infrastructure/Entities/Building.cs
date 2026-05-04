using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Entities
{
    public class Building
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ExpensesDueDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalSurfaceArea { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public DateTime CreationDate { get; set; } = DateTime.Now;

        [Required]
        public int AddressId { get; set; }

        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User AdministratorUser { get; set; }

        public virtual ICollection<Unit> Units { get; set; }
    }
}
