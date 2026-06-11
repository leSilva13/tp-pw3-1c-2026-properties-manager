using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProgramacionWeb3.PropertiesManager.Web.Models
{
    public class UnitFormViewModel
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public string? BuildingName { get; set; }
        public string Floor { get; set; }
        public string ApartmentNumber { get; set; }
        public decimal AreaInMeters { get; set; }
        public int OwnerId { get; set; }

        public SelectList? Owners { get; set; }
    }
}