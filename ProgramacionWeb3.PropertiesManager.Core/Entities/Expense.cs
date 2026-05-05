using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProgramacionWeb3.PropertiesManager.Core.Entities
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Required]
        public int ExpenseMonth { get; set; }

        [Required]
        public int ExpenseYear { get; set; }

        public bool IsGlobal { get; set; } = true;

        [Required]
        public DateTime BillingDate { get; set; }

        public string? ReceiptPath { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;
        
        [Required]
        public int ExpenseTypeId { get; set; }

        [ForeignKey("ExpenseTypeId")]
        public virtual ExpenseType ExpenseType { get; set; }

        [Required]
        public int BuildingId { get; set; }

        [ForeignKey("BuildingId")]
        public virtual Building Building { get; set; }

        public virtual ICollection<ExpenseDistribution> ExpenseDistributions { get; set; }
    }
}
