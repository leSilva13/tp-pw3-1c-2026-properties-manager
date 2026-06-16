namespace ProgramacionWeb3.PropertiesManager.Core.DTOs
{
    public class UnitDto
    {
        public int Id { get; set; }

        public int BuildingId { get; set; }
        
        public string BuildingName { get; set; }

        public string Floor { get; set; }

        public string ApartmentNumber { get; set; }

        public decimal AreaInMeters { get; set; }

        public int? OwnerId { get; set; }
        public string? OwnerFullName { get; set; }
        public string? OwnerEmail { get; set; }
        public OwnerDto? NuevoOwner { get; set; }

        public BuildingDto? BuildingDto { get; set; }
    }
}