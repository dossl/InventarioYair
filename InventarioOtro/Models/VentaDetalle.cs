using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventarioOtro.Models
{
    public class VentaDetalle
    {
        public int ID { get; set; }
        public int ProductoId { get; set; }
        public int VentaId { get; set; }
        public int Cantidad { get; set; }
        public int Garantia { get; set; }
        public decimal Descuento { get; set; }
        public Producto Producto { get; set; }
        public Venta Venta { get; set; }
    }
}