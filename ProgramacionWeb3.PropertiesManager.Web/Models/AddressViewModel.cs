namespace ProgramacionWeb3.PropertiesManager.Web.Models
{
    public class AddressViewModel
    {
        public int Id { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }

        public int ProvinceId { get; set; }

        public int LocalityId { get; set; }

        public string Latitude { get; set; } = string.Empty;

        public string Longitude { get; set; } = string.Empty;
    }
}
