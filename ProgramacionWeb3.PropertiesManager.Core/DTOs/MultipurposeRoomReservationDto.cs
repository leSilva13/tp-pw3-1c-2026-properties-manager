using ProgramacionWeb3.PropertiesManager.Core.Common.Enums;

namespace ProgramacionWeb3.PropertiesManager.Core.DTOs
{
    public class MultipurposeRoomReservationDto
    {
        public int Id { get; set; }

        public DateOnly ReservationDate { get; set; }

        public MultipurposeRoomShiftEnum Shift { get; set; }

        public string Comments { get; set; }

        public bool WasWellDelivered { get; set; }

        public virtual MultipurposeRoomDto MultipurposeRoomDto { get; set; }

        public int UnitId { get; set; }
    }
}
