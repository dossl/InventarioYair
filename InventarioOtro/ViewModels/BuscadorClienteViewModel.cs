using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InventarioOtro.Models;
using PagedList;

namespace InventarioOtro.ViewModels
{
    public class BuscadorClienteViewModel
    {
        public IPagedList<Cliente> ListClientes { get; set; }
        public IEnumerable<SelectListItem> Listcriterio { get; set; }


    }
}