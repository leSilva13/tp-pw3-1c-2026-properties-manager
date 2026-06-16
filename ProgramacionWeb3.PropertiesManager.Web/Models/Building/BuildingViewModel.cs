using ProgramacionWeb3.PropertiesManager.Web.Models.Unit;

namespace ProgramacionWeb3.PropertiesManager.Web.Models.Building
{
    public class BuildingViewModel
    {
        public string FullAddress { get; set; }

        public int ExpensesDueDate { get; set; }

        public IEnumerable<UnitViewModel> UnitViewModels { get; set; }

        public IEnumerable<MultipurposeRoomViewModel> MultipurposeRoomViewModels { get; set; }
    }
}
