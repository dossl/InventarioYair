using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventarioOtro.Models
{
    public class Finanzas
    {
        public int ID { get; set; }
        public decimal ValorCapital { get; set; }
        public decimal ValorProductos { get; set; }
        public decimal ValorEfectivo { get; set; }
        public decimal Ganancias { get; set; }
    }
}