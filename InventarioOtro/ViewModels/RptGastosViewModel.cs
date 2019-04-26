using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventarioOtro.ViewModels
{
    public class RptGastosViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Inicio { get; set; }
        [Required]
        [DataType(DataType.Date)]
       [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Fin { get; set; }
    }
}