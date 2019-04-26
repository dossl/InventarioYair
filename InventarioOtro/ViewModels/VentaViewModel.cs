using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventarioOtro.Models;
using PagedList;

namespace InventarioOtro.ViewModels
{
    public class VentaViewModel
    {
        public Venta Venta { get; set; }
        public List<string>  ListSrcFotos { get; set; }
        public string NombreMensajero { get; set; }
        public string ObjJson { get; set; }
        public Devolucion Devolucion { get; set; }

        


    }
}