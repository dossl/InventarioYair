using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventarioOtro.Models
{
    public class Devolucion
    {
        public int? Id { get; set; }
        public string Notas { get; set; }
        public int VentaId { get; set; }
        public decimal DescuentoSalario { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public Venta Venta { get; set; }
    }
}