namespace ProgramacionWeb3.PropertiesManager.Core.DTOs
{
    public class BaseResponseDto
    {
        public int Id { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }
    }
}
