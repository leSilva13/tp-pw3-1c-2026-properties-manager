namespace ProgramacionWeb3.PropertiesManager.Core.DTOs
{
    public class OwnerDto
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<UnitDto> UnitDtos { get; set; }
    }
}
