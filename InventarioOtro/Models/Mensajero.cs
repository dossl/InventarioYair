﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventarioOtro.Models
{
    public class Mensajero
    {
        public int ID { get; set; }
        [Required]
        public string Nombre { get; set; }
    }
}