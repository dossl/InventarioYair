using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventarioOtro.Models
{
    public class Merma
    {
        public int Id { get; set; }
        [Display(Name = "Producto")]
        [Required (ErrorMessage = "Este campo es obligatorio")]
        public int ProductoId { get; set; }
        [Required]
        [Range(0,Int32.MaxValue,ErrorMessage = "El valor deber ser mayor que 0")]
        public int Cantidad { get; set; }
        [Required]
        [Range(0.05,Double.MaxValue, ErrorMessage = "El precio deber ser mayor que 0")]
        public decimal Precio  { get; set; }        
        public Producto Producto { get; set; }

    }
}