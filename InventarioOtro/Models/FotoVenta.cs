using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventarioOtro.Models
{
    public class FotoVenta
    {
        public int ID { get; set; }
        [MaxLength(Int32.MaxValue)]
        public byte[] Bfotos { get; set; }


        public string Title { get; set; }

        public string Description { get; set; }


        public byte[] ImageContent { get; set; }

        public string ImageMimeType { get; set; }

        public string ImageName { get; set; }



        public string FileMimeType { get; set; }

        public string FileName { get; set; }
        public int VentaId { get; set; }
        public Venta Venta { get; set; }
    }
}