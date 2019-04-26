using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventarioOtro.Models
{
    public class Venta
    {
        public int? ID { get; set; }
        [Required]
        [Display(Name = "Fecha de Venta")]
        [DataType(DataType.DateTime,ErrorMessage = "El Valor debe ser una fecha válida")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}")]
        public DateTime FechaVenta { get; set; }
        [Display(Name = "Mensajero")]
        public string MensajeroId { get; set; }
        [Display(Name = "Vendedor")]
        [Required]
        public int VendedorId  { get; set; }
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }
        [Display(Name = "Ganancia del Mensajero")]
        public decimal GanaMensajero { get; set; }
        [Display(Name = "Descuento")]
        public decimal Descuento { get; set; }
        [Display(Name = "Costo Total en Productos")]
        public decimal CostoTotal { get; set; }
        [Display(Name = "Ganancia")]
        public decimal GanaTotal { get; set; }
        [Display(Name = "Precio Total")]
        public decimal PrecioTotal { get; set; }
        [Display(Name = "Cant. Total de Productos")]
        public int CantTotalProductos { get; set; }
        public int Estado { get; set; }
        [Display(Name = "Cliente")]
        public Cliente Cliente { get; set; }
        [Display(Name = "Vendedor")]
        public Vendedor Vendedor { get; set; }
        public string  Usuario { get; set; }
        public int TipoVenta { get; set; } //1- minorista 2-mayorista
        public List<VentaDetalle> DetallesVenta{ get; set; }
        public List<FotoVenta> ListFotoVentas { get; set; }
        public decimal GanaTrab { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public bool CalcSalarioTrab { get; set; }



    }
}