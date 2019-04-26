using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventarioOtro.Models
{
    public class Deuda
    {
        public int Id { get; set; }
        public int VentaId { get; set; }
        [Display(Name = "Valor Inicial")]
        public decimal ValorInicial { get; set; }
        [Display(Name = "Valor Actual")]
        [RangeAttribute(0,Double.MaxValue,ErrorMessage = "El valor debe estar entre 0 y el Valor Actual de la deuda")]
        public decimal ValorActual { get; set; }
        public decimal CostoProductoAPagar { get; set; }
        public decimal GanaTrabAPagar { get; set; }
        [Display(Name = "Fecha de Creación")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaCreacion { get; set; }
        [Display(Name = "Último pago")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime UltimoPago { get; set; }
        public Venta Venta { get; set; }
    }
}