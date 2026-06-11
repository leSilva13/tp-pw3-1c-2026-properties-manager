namespace ProgramacionWeb3.PropertiesManager.Core.DTOs
{
    public class BuildingDto
    {
        public int Id { get; set; }

        public int ExpensesDueDate { get; set; }

        public decimal TotalSurfaceArea { get; set; }

        public bool IsActive { get; set; }

        public int AdministratorUserId { get; set; }

        public AddressDto AddressDto { get; set; }
    }
}
