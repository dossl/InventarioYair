using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventarioOtro.Models;
using PagedList;

namespace InventarioOtro.ViewModels
{
    public class IndexMermaViewModel
    {
        public IPagedList<Merma> ListProductos { get; set; }
        public IEnumerable<CategoriaProducto> ListCategoriaProductos { get; set; }
    }
}