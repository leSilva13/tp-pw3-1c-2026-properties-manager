using ProgramacionWeb3.PropertiesManager.Core.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgramacionWeb3.PropertiesManager.Core.Entities
{
    public class MultipurposeRoomReservation
    {
        [Key]
        public int Id { get; set; }

        public DateOnly ReservationDate { get; set; }

        public MultipurposeRoomShiftEnum Shift { get; set; }

        public string Comments { get; set; }

        public bool WasWellDelivered { get; set; }

        [Required]
        public int MultipurposeRoomId { get; set; }

        [ForeignKey("MultipurposeRoomId")]
        public virtual MultipurposeRoom MultipurposeRoom { get; set; }

        [Required]
        public int UnitId { get; set; }

        [ForeignKey("UnitId")]
        public virtual Unit Unit { get; set; }
    }
}
