using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventarioOtro.Models;

namespace InventarioOtro.ViewModels
{
    public class FotoVentaViewModel
    {
        public FotoVenta FotoVenta{ get; set; }
        public Venta Venta { get; set; }
    }
}