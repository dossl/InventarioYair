using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventarioOtro.Models;
using PagedList;

namespace InventarioOtro.ViewModels
{
    public class IndexVendedorViewModel
    {
        public IPagedList<Vendedor> ListVendedores { get; set; }
    }
}