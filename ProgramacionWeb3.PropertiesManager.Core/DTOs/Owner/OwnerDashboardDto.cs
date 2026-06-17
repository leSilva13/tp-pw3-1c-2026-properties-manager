namespace ProgramacionWeb3.PropertiesManager.Core.DTOs.Owner
{
    public class OwnerDashboardDto
    {
        public IEnumerable<BuildingDto> BuildingDtos { get; set; }

        public IEnumerable<MultipurposeRoomReservationDto> MultipurposeRoomReservationDtos { get; set; }

        public IEnumerable<ExpenseDto> ExpenseDtos { get; set; }
    }
}
