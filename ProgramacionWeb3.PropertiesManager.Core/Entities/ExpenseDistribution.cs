using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgramacionWeb3.PropertiesManager.Core.Entities
{
    public class ExpenseDistribution
    {
        [Key]
        public int Id { get; set; }

        public int ExpenseId { get; set; }
        [ForeignKey("ExpenseId")]
        public virtual Expense Expense { get; set; }

        public int UnitId { get; set; }
        [ForeignKey("UnitId")]
        public virtual Unit Unit { get; set; }

        //TODO ???
        //[Column(TypeName = "decimal(18, 2)")]
        //public decimal? SpecificAmount { get; set; }
    }
}
