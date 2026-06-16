namespace ProgramacionWeb3.PropertiesManager.Core.DTOs
{
    public class AddressDto
    {
        public int Id { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }

        public int ProvinceId { get; set; }

        public string ProvinceName { get; set; }

        public int LocalityId { get; set; }

        public decimal Latitude { get; set; }
        
        public decimal Longitude { get; set; }
    }
}
