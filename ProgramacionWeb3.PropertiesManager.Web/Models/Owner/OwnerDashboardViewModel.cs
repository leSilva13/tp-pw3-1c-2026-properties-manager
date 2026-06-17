using ProgramacionWeb3.PropertiesManager.Web.Models.Building;
using ProgramacionWeb3.PropertiesManager.Web.Models.Expenses;

namespace ProgramacionWeb3.PropertiesManager.Web.Models.Owner
{
    public class OwnerDashboardViewModel
    {
        public IEnumerable<BuildingViewModel> BuildingViewModels { get; set; }

        public IEnumerable<MultipurposeRoomReservationViewModel> MultipurposeRoomReservationViewModels { get; set; }

        public IEnumerable<ExpenseViewModel> ExpenseViewModels { get; set; }
    }
}
