using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgramacionWeb3.PropertiesManager.Core.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public DateTime? LastLoginDate { get; set; } // el "?" se usa para indicar que el campo puede ser nulo ejemplo el usuario aún no inicio sesión

        [Required]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; } // virtual permite a EF usar lazy loading. Sin lazy loading, para acceder a Role.Name hay que cargar la relación explícitamente con Include(u => u.Role).

    }
}
