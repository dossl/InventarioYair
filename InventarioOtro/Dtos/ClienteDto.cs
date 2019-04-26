using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventarioOtro.Dtos
{
    public class ClienteDto
    {
        public int ID { get; set; }
        [StringLength(100)]
        public string Clastname { get; set; }
        [Required]
        [StringLength(100)]
        public string Cfirstname { get; set; }
        [Required]
        [StringLength(11)]
        public string Ccidentidad { get; set; }
        public string Cnumtelefono { get; set; }
        public bool Lactivo { get; set; }
        public string Cdireccion { get; set; }
        public string Cnotas { get; set; }
    }
}