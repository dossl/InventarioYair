using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventarioOtro.Models
{
    public class PagoSalario
    {
        public int Id { get; set; }
        [Required (ErrorMessage = "Este campo es requerido")]
        public string Trabajador { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]
        public decimal Monto { get; set; }
        public int CantidadLaborada { get; set; }// dias entre el rango de fecha
        [Display(Name = "Días no laborados")]
        public int DiasDescontado { get; set; }
        //public int Estado { get; set; } //1-Calculado 2-Pagado
        [Display(Name = "Fecha de Pago")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaPago { get; set; }

    }
}