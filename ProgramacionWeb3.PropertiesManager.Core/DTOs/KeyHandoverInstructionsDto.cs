namespace ProgramacionWeb3.PropertiesManager.Core.DTOs
{
    public class KeyHandoverInstructionsDto
    {
        public bool HasPreviousReservation { get; set; }

        public string PreviousOwnerEmail { get; set; }

        public string PreviousOwnerName { get; set; }

        public string PreviousUnitNumber { get; set; }

        public string PickUpInstructionMessage { get; set; }

        public bool HasNextReservation { get; set; }

        public string NextOwnerEmail { get; set; }

        public string NextOwnerName { get; set; }

        public string NextUnitNumber { get; set; }

        public string DropOffInstructionMessage { get; set; }
    }
}
