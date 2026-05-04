using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Entities
{
    public class UnitBill
    {
        [Key]
        public int Id { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        public bool IsPaid { get; set; } = false;

        public string? ReceiptPath { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public int UnitId { get; set; }

        [ForeignKey("UnitId")]
        public virtual Unit Unit { get; set; }
    }
}
