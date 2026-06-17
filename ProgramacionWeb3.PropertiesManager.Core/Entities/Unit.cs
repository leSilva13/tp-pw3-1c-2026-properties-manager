using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgramacionWeb3.PropertiesManager.Core.Entities
{
    public class Unit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Floor { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        [StringLength(10)]
        public string ApartmentNumber { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")] 
        public decimal AreaInMeters { get; set; } 

        public DateTime CreationDate { get; set; } = DateTime.Now;

        [Required]
        public int BuildingId { get; set; }

        [ForeignKey("BuildingId")]
        public virtual Building Building { get; set; }

        public int? OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public virtual Owner? Owner { get; set; }

        public virtual ICollection<ExpenseDistribution> ExpenseDistributions { get; set; }

        public virtual ICollection<MultipurposeRoomReservation> MultipurposeRoomReservations { get; set; }
    }
}
