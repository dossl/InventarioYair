using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventarioOtro.Models;

namespace InventarioOtro.ViewModels
{
    public class FotoProductoViewModel
    {
        public FotoProducto FotoProducto { get; set; }
        public Producto Producto { get; set; }
    }
}