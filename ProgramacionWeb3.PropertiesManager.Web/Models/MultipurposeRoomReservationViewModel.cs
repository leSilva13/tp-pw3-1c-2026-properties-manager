using ProgramacionWeb3.PropertiesManager.Core.Common.Enums;

namespace ProgramacionWeb3.PropertiesManager.Web.Models
{
    public class MultipurposeRoomReservationViewModel
    {
        public int Id { get; set; }

        public DateOnly ReservationDate { get; set; }

        public MultipurposeRoomShiftEnum Shift { get; set; }

        public string Comments { get; set; }

        public bool WasWellDelivered { get; set; }

        public int MultipurposeRoomId { get; set; }

        public MultipurposeRoomViewModel MultipurposeRoomViewModel { get; set; }

        public int UnitId { get; set; }
    }
}
