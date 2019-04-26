using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventarioOtro.Models;
using PagedList;

namespace InventarioOtro.ViewModels
{
    public class IndexProductoViewModel
    {
        public IPagedList<Producto> ListProductos { get; set; }
        public IEnumerable<CategoriaProducto> ListCategoriaProductos { get; set; }
    }
}