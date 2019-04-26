using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventarioOtro.Models
{
    public class Producto
    {
        public int ID { get; set; }

        [Display(Name = "Artículo")]
        [Required]
        [StringLength(50)]
        public string Carticulo { get; set; }

        [Display(Name = "Descripción")]
        [Required]
        [StringLength(255)]
        public string Cdescripcion { get; set; }

        [Display(Name = "Categoría")]
        [Required]
        public int CategoriaProductoId { get; set; }

        [Display(Name = "Cantidad")]
        [Required]
        public int Ncantidad { get; set; }
        
        [Display(Name = "Precio Costo")]
        public decimal Npreccosto { get; set; }

        [Display(Name = "Precio Mayorista")]
        public decimal Nprecmayoris { get; set; }

        [Display(Name = "Precio Minorista")]
        public decimal Nprecminoris { get; set; }

        [Display(Name = "Ganancia del Trabajador")]
        public decimal Ngananctrab { get; set; }

        [Display(Name = "Lote")]
        [Required]
        public string Clote { get; set; }

        [Display(Name = "Descuento")]
        public decimal Ndescuento { get; set; }

        [Display(Name = "Disponibilidad")]
        public int Ndisponibilidad { get; set; }

        [Display(Name = "Activo?")]
        public bool Lactivo { get; set; }
        [Display(Name = "Inversion?")]
        public bool Linversion { get; set; }

        public DateTime Dfechacreacion { get; set; }


        public CategoriaProducto CategoriaProducto { get; set; }
        public List<FotoProducto>ListaFotos { get; set; }
    }
}