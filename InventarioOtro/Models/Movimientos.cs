using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventarioOtro.Models
{
    public class Movimiento
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Nombre del Movimiento")]
        public string Nombre { get; set; }
        [Required]
        [Display(Name = "Monto")]
        [Range(0, int.MaxValue, ErrorMessage = "Debe ingresar un valor numérico mayor que 0.")]
        public decimal Monto { get; set; }
        [Required]
        [Display(Name = "Tipo de movimiento")]
        public int TipoMovimiento { get; set; }//1- Ingreso 2- Gasto 3-Extracion 4-Interno 5-Comision 6-Merma 7-Ganancia
        public string Usuario  { get; set; }
        [Required]
        [Display(Name = "Fecha")]
        [DataType(DataType.DateTime,ErrorMessage = "Debe ser una fecha valida"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime FechaMovimiento { get; set; }
        
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        public int IdProducto{ get; set; }
        public int IdProCompra { get; set; }
        public int IdVenta { get; set; }
    }
}