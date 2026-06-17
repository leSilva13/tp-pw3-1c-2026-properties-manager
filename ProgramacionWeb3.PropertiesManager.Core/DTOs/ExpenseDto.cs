namespace ProgramacionWeb3.PropertiesManager.Core.DTOs
{
    public class ExpenseDto
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public int ExpenseMonth { get; set; }

        public int ExpenseYear { get; set; }

        public bool IsGlobal { get; set; } = true;

        public DateTime BillingDate { get; set; }

        public string? ReceiptPath { get; set; }

        public DateTime CreationDate { get; set; }

        public int ExpenseTypeId { get; set; }

        public string? ExpenseTypeName { get; set; }

        public int BuildingId { get; set; }

        public string? BuildingName { get; set; }

        public List<int> UnitIds { get; set; } = new();
    }
}
