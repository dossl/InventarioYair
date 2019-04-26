using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using InventarioOtro.Models;

namespace InventarioOtro.ViewModels
{
    public class ProductoViewModel
    {
        public Producto Producto { get; set; }
        public IEnumerable<CategoriaProducto> TipoCategoriaProductos { get; set; }

    }
}