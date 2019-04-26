using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace InventarioOtro.Models
{
    public class CategoriaProducto
    {
        public int ID { get; set; }
        [Required (ErrorMessage = "Este campo es obligatorio")]
        [Display (Name = "Nombre de la categoría")]
        public string Cnombre { get; set; }


    }
}