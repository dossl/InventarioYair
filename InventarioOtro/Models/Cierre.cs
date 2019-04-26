using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace InventarioOtro.Models
{
    public class Cierre
    {
        public int Id { get; set; }
        [Display(Name="Ganancias")]
        public decimal GananciasPeriodo { get; set; }
        [Display(Name = "Retiro de efectivo")]
        public decimal GananciaExtraida { get; set; }
        [Display(Name="Cantidad Vendida")]
        public int CantidadArticulos { get; set; }
        [Display(Name = "Total de Ventas")]
        public decimal ValorTotalVentas { get; set; }
        [Display(Name = "Cantidad Devuelta")]
        public int CantArticulosDev { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Fecha { get; set; }
    }
}