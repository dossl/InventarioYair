using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventarioOtro.Models;

namespace InventarioOtro.ViewModels
{
    public class DetailVentaViewModel
    {
        public Venta Venta { get; set; }
        public List<String> ListImgSrc { get; set; }

        public string NombreMensajero { get; set; }
    }
}