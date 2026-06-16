namespace ProgramacionWeb3.PropertiesManager.Core.DTOs
{
    public class MultipurposeRoomDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Floor { get; set; }

        public bool IsActive { get; set; }

        public int BuildingId { get; set; }
    }
}
