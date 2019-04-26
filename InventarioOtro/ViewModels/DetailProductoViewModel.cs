using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventarioOtro.Models;

namespace InventarioOtro.ViewModels
{
    public class DetailProductoViewModel
    {
        public Producto Producto { get; set; }
        public List<String> ListImgSrc { get; set; }
    }
}