using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventarioOtro.ViewModels
{
    public class RptMensajeriaViewModel
    {
        
        public string MensajeroId { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        [DataType(DataType.DateTime, ErrorMessage = "deivis")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Inicio { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Fin { get; set; }
    }
}