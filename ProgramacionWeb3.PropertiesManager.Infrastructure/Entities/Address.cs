using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Entities
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Street { get; set; }

        [Required]
        [StringLength(50)]
        public string Number { get; set; }

        [Column(TypeName = "decimal(18, 10)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(18, 10)")]
        public decimal Longitude { get; set; }

        [Required]
        public int ProvinceId { get; set; }

        [ForeignKey("ProvinceId")]
        public virtual Province Province { get; set; }

        //TODO ??
        //[Required]
        //public int LocalityId { get; set; }

        //[ForeignKey("LocalityId")]
        //public virtual Locality Locality { get; set; }
    }
}
