namespace ProgramacionWeb3.PropertiesManager.Web.Models
{
    public class BuildingFormViewModel
    {
        public int Id { get; set; }

        public int AddressId { get; set; }

        public int ExpensesDueDate { get; set; }
        
        public decimal TotalSurfaceArea { get; set; }

        public bool IsActive { get; set; } = true;

        public AddressViewModel AddressViewModel { get; set; } 
    }
}
