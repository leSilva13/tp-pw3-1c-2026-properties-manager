namespace ProgramacionWeb3.PropertiesManager.Core.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public OwnerDto? OwnerDto { get; set; }

        public int RoleId { get; set; }
    }
}
