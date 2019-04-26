using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace InventarioOtro.Models
{
    public class Cliente
    {
        public int ID { get; set; }

        [Display(Name="Apellidos")]
        [StringLength(100)]
        public string Clastname { get; set; }

        [Display(Name = "Nombre(s)")]
        [Required]
        [StringLength(100)]
        public string Cfirstname { get; set; }

        [Display(Name = "No. de identidad")]
        [Required]
        [StringLength(11)]
        [MinLength(11,ErrorMessage = "El número de identidad tiene que tener 11 dígitos")]
        public string Ccidentidad { get; set; }

        [Display(Name = "No. de teléfono")]
        [Phone(ErrorMessage = "El campo no tiene el formato adecuado")]
        [MinLength(7, ErrorMessage = "El número de teléfono tiene que tener al menos 7 dígitos")]
        public string Cnumtelefono { get; set; }

        [Display(Name = "Activo?")]
        public bool Lactivo { get; set; }

        [Display(Name = "Dirección")]
        [StringLength(255)]
        public string Cdireccion { get; set; }

        [Display(Name = "Notas")]
        [StringLength(255)]
        public string Cnotas { get; set; }
    }
}