using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InventarioOtro.Models;
using PagedList;

namespace InventarioOtro.ViewModels
{
    public class IndexMovimientoViewModel
    {
        public IPagedList<Movimiento> ListMovimiento { get; set; }

        

    }
}