using Microsoft.AspNetCore.Mvc.Rendering;
using ProgramacionWeb3.PropertiesManager.Core.Entities;

namespace ProgramacionWeb3.PropertiesManager.Web.Models
{
    public class BuildingFormViewModel
    {
        public Building Building { get; set; } = new Building();
        //public Building Building { get; set; } = new Building { Address = new Address() }

        public int SelectedProvinceId { get; set; }
        //public int SelectedLocalityId { get; set; }

        public SelectList? Provinces { get; set; }
        //public SelectList? Localities { get; set; }
    }
}
