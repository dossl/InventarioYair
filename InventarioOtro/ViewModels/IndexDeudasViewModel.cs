﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventarioOtro.Models;
using PagedList;

namespace InventarioOtro.ViewModels
{
    public class IndexDeudasViewModel
    {
        public IPagedList<Deuda> ListDeudas { get; set; }
    }
}