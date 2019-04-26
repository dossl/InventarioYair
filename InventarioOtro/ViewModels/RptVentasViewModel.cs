using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventarioOtro.ViewModels
{
    public class RptVentasViewModel
    {
      
        public string Cliente { get; set; }
        public string Producto { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Inicio { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Fin { get; set; }
    }
}