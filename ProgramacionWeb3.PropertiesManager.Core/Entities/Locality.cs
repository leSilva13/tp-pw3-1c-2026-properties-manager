using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProgramacionWeb3.PropertiesManager.Core.Entities
{
    public class Locality
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        // Relación con Provincia
        public int ProvinceId { get; set; }
        [ForeignKey("ProvinceId")]
        public virtual Province Province { get; set; }
    }
}
