using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using InventarioOtro.Models;
using InventarioOtro.ViewModels;
using Microsoft.AspNet.Identity;
using PagedList;

namespace InventarioOtro.Controllers
{
    public class VentasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Ventas
        public async Task<ActionResult> Index(string searchString2, string searchString1, string searchString,
            string categoriaId, int? page, int? paso, string sortOrder)
        {

            if (TempData["Validar"] == null)
            {
                ViewBag.Validar = false;
            }
            else
            {
                ViewBag.Validar = TempData["Validar"];
                TempData["Validar"] = false;
                
            }
            /*Seguridad s = new Seguridad();
            if (!s.ValidarSeguridad())
            {
                return View("Error");
            }*/
            if ("5".Equals(categoriaId) && !string.IsNullOrEmpty(searchString2))
            {
                searchString = searchString2;
            }
            else
            if ("2".Equals(categoriaId) && !string.IsNullOrEmpty(searchString1))
            {
                searchString = searchString1;
            }
            else
            {
                if ("2".Equals(categoriaId) && string.IsNullOrEmpty(searchString1))
                {
                    searchString = "";
                }
            }

            if (paso == null || paso == 0)
            {
                paso = 50;
                ViewBag.Paso = paso;
            }
            else
            {
                ViewBag.Paso = paso;
            }
            if (page == null || page == 0)
            {
                page = 1;
                ViewBag.Page = page;
            }
            else
            {
                ViewBag.Page = page;
            }
            IndexVentaViewModel viewModel = new IndexVentaViewModel();
            var lista = new List<Venta>();
            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.Search = searchString;
                if (!String.IsNullOrWhiteSpace(categoriaId))
                {
                    ViewBag.CategoriaId = categoriaId;
                    switch (categoriaId)
                    {
                        case "1":
                            {
                                lista = db.Ventas.Include(v => v.Vendedor).Include(p => p.Cliente).Where(p => p.Cliente.Cfirstname.ToLower().Contains(searchString.ToLower()) || p.Cliente.Clastname.ToLower().Contains(searchString.ToLower())).ToList();
                                break;
                            }
                        case "2":
                            {
                                int valor = int.Parse(searchString);
                                lista = db.Ventas.Include(v => v.Vendedor).Include(p => p.Cliente).Where(p => p.TipoVenta.Equals(valor)).ToList();
                                break;
                            }
                        case "3":
                            {
                                decimal valor = decimal.Parse(searchString);
                                lista = db.Ventas.Include(v => v.Vendedor).Include(p => p.Cliente).Where(p => p.PrecioTotal == valor).ToList();
                                break;
                            }
                        case "4":
                            {
                                decimal valor = decimal.Parse(searchString);
                                lista = db.Ventas.Include(v => v.Vendedor).Include(p => p.Cliente).Where(p => p.CantTotalProductos == valor).ToList();
                                break;
                            }
                        case "5":
                            {
                                decimal valor = decimal.Parse(searchString);
                                lista = db.Ventas.Include(v => v.Vendedor).Include(p => p.Cliente).Where(p => p.Estado == valor).ToList();
                                break;
                            }
                    }

                }
                else
                {
                    ViewBag.CategoriaId = "";
                    lista = db.Ventas.Include(v => v.Vendedor).Include(p => p.Cliente).ToList();
                }


            }
            else
            {
                ViewBag.Search = "";
                if (!String.IsNullOrEmpty(categoriaId))
                {
                    ViewBag.CategoriaId = categoriaId;
                    lista = db.Ventas.Include(v => v.Vendedor).Include(p => p.Cliente).ToList();
                }
                else
                {
                    ViewBag.CategoriaId = "";
                    lista = db.Ventas.Include(v => v.Vendedor).Include(p => p.Cliente).ToList();
                }


            }
            if (lista.Count <= ViewBag.Paso)
            {
                ViewBag.Page = 1;
            }
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.NameSortParm = sortOrder == "cliente" ? "cliente_desc" : "cliente";
            ViewBag.MontoSortParm = sortOrder == "monto" ? "monto_desc" : "monto";
            ViewBag.TipSortParm = sortOrder == "tip" ? "tip_desc" : "tip";
            ViewBag.EstadoSortParm = sortOrder == "estado" ? "estado_desc" : "estado";
            ViewBag.VenSortParm = sortOrder == "v" ? "v_desc" : "v";
            ViewBag.CanSortParm = sortOrder == "c" ? "c_desc" : "c";


            switch (sortOrder)
            {
                case "cliente_desc":
                    lista = lista.OrderByDescending(l => l.Cliente.Cfirstname).ThenByDescending(l=>l.Cliente.Clastname).ToList();
                    break;
                case "cliente":
                    lista = lista.OrderBy(l => l.Cliente.Cfirstname).ThenBy(l => l.Cliente.Clastname).ToList();
                    break;

                case "date_desc":
                    lista = lista.OrderBy(l => l.FechaVenta).ToList();
                    break;
                case "monto":
                    lista = lista.OrderBy(l => l.PrecioTotal).ToList();
                    break;
                case "monto_desc":
                    lista = lista.OrderByDescending(l => l.PrecioTotal).ToList();
                    break;
                case "tip":
                    lista = lista.OrderBy(l => l.TipoVenta).ToList();
                    break;
                case "tip_desc":
                    lista = lista.OrderByDescending(l => l.TipoVenta).ToList();
                    break;
                case "estado":
                    lista = lista.OrderBy(l => l.Estado).ToList();
                    break;
                case "estado_desc":
                    lista = lista.OrderByDescending(l => l.Estado).ToList();
                    break;
                case "v":
                    lista = lista.OrderBy(l => l.Vendedor.Nombre).ToList();
                    break;
                case "v_desc":
                    lista = lista.OrderByDescending(l => l.Vendedor.Nombre).ToList();
                    break;
                case "c":
                    lista = lista.OrderBy(l => l.CantTotalProductos).ToList();
                    break;
                case "c_desc":
                    lista = lista.OrderByDescending(l => l.CantTotalProductos).ToList();
                    break;
                default:
                    lista = lista.OrderByDescending(l => l.FechaVenta).ToList();
                    break;
            }

            viewModel.ListVentas = lista.ToPagedList((int)ViewBag.Page, (int)ViewBag.Paso);


            return View(viewModel);

        }

        // GET: Ventas/Details/5
        public async Task<ActionResult> Details(int? id, string l)
        {
            ViewBag.Mov = string.IsNullOrEmpty(l);
            /*Seguridad s = new Seguridad();
            if (!s.ValidarSeguridad())
            {
                return View("Error");
            }*/
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venta venta = await db.Ventas.Include(v => v.Cliente).Include(c => c.Vendedor).Where(t => t.ID == id).FirstAsync();
            if (venta == null)
            {
                return HttpNotFound();
            }

            venta.ListFotoVentas = db.FotoVentas.Where(f => f.VentaId == venta.ID).ToList();
            venta.DetallesVenta = db.VentaDetalles.Where(f => f.VentaId == venta.ID).Include(p => p.Producto).ToList();

            DetailVentaViewModel d = new DetailVentaViewModel()
            {
                Venta = venta,


            };

            if (string.IsNullOrEmpty(venta.MensajeroId) || "0".Equals(venta.MensajeroId))
            {
                d.NombreMensajero = "Venta Local";
            }
            else
            {
                int valor = int.Parse(venta.MensajeroId);
                d.NombreMensajero = db.Mensajeros.First(m => m.ID == valor).Nombre;
            }


            List<String> fotosList = new List<String>();
            foreach (var val in venta.ListFotoVentas)
            {
                fotosList.Add("data:" + val.ImageMimeType + ";base64," + Convert.ToBase64String(val.ImageContent));
            }
            d.ListImgSrc = fotosList;
            return View(d);
        }

        // GET: Ventas/Create
        public ActionResult Create(VentaViewModel model)
        {
            /*Seguridad s = new Seguridad();
            if (!s.ValidarSeguridad())
            {
                return View("Error");
            }*/
            ViewBag.ClienteId = new SelectList(db.Clientes, "ID", "Clastname");
            ViewBag.VendedorId = new SelectList(db.Vendedores, "ID", "Nombre");

            if (model.Venta == null)
            {
                ViewBag.Nombre = "";
                Venta ve = new Venta()
                {
                    TipoVenta = 1,
                    DetallesVenta = new List<VentaDetalle>(),
                    FechaVenta = DateTime.Now,
                    CalcSalarioTrab = true
                };
                model.Venta = ve;
                model.ListSrcFotos = new List<string>();
                ViewBag.Min = true;
            }
            else
            {
                ViewBag.Min = 1 == model.Venta.TipoVenta;

                if (model.Venta.ClienteId > 0)
                {
                    Cliente cliente = db.Clientes.First(c => c.ID == model.Venta.ClienteId);
                    ViewBag.Nombre = cliente.Cfirstname + " " + cliente.Clastname;
                }
                else
                {
                    ViewBag.Nombre = " ";
                }
                ViewBag.Min = 1 == model.Venta.TipoVenta;
            }




            return View(model);
        }

        // POST: Ventas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Create([Bind(Include = "ID,FechaVenta,MensajeroId,VendedorId,ClienteId,GanaMensajero,Descuento,CostoTotal,GanaTotal,PrecioTotal,CantTotalProductos,Usuario,Estado,TipoVenta,DetallesVenta,Cliente,Vendedor,ListFotoVentas,CalcSalarioTrab")] Venta venta)
        {

            if (ModelState.IsValid)
            {
                decimal salarioTrabajador = 0;
                if (venta.TipoVenta != 2)
                {
                    if(venta.CalcSalarioTrab)
                    foreach (var detalle in venta.DetallesVenta)
                    {
                        Producto pr = db.Productos.First(p => p.ID == detalle.ProductoId);
                        salarioTrabajador += detalle.Cantidad * pr.Ngananctrab;
                    }
                }
               
                venta.GanaTrab = salarioTrabajador;
                venta.GanaTotal = venta.PrecioTotal - venta.CostoTotal - venta.GanaTrab - venta.Descuento;
                venta.Usuario = User.Identity.Name;
                venta.FechaVenta=DateTime.Now;
                // var a=ModelState[cade].Errors.ToList();
                db.Ventas.Add(venta);
                db.SaveChanges();
                //REBAJAR Disponibilidad
                List<VentaDetalle> lista = venta.DetallesVenta;
                foreach (var ventaDetalle in lista)
                {
                    Producto prod = new Producto();
                    prod = db.Productos.First(p => p.ID == ventaDetalle.ProductoId);
                    prod.Ndisponibilidad -= ventaDetalle.Cantidad;
                    db.Entry(prod).State = EntityState.Modified;
                    db.SaveChanges();
                }

                //Realizar Movimientos en dependecia del estado de la venta



                return Json(new { Success = 1, ID = venta.ID, ex = "" });
            }

            return Json(new { Success = 0, ex = "No se pudo realizar la venta debido a un error" });



        }

        private class VentaSer
        {
            public int ID { get; set; }
            public DateTime FechaVenta { get; set; }

            public string MensajeroId { get; set; }

            public int VendedorId { get; set; }

            public int ClienteId { get; set; }

            public decimal GanaMensajero { get; set; }

            public decimal Descuento { get; set; }

            public decimal CostoTotal { get; set; }

            public decimal GanaTotal { get; set; }

            public decimal PrecioTotal { get; set; }

            public int CantTotalProductos { get; set; }
            public int Estado { get; set; }

            public string Usuario { get; set; }
            public int TipoVenta { get; set; } //1- minorista 2-mayorista
            public string Descripcion { get; set; }
            public bool CalcSalarioTrab { get; set; }
            public List<DetalleVentaSer> DetallesVenta { get; set; }




        }

        private class DetalleVentaSer
        {
            public int ID { get; set; }
            public int ProductoId { get; set; }
            public int VentaId { get; set; }
            public int Cantidad { get; set; }
            public int Garantia { get; set; }
            public decimal Descuento { get; set; }
            public decimal Precio { get; set; }
            public string Lote { get; set; }
            public string ProductoN { get; set; }
            public decimal Costo { get; set; }
            public int Disponibilidad { get; set; }

        }
        // GET: Ventas/Edit/5
        public ActionResult Edit(int? id)
        {
           /*Seguridad s = new Seguridad();
            if (!s.ValidarSeguridad())
            {
                return View("Error");
            }*/
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Venta venta = db.Ventas
                .Include(vv => vv.Cliente)
                .Include(p => p.Vendedor)

                .Include(f => f.ListFotoVentas)
                .First(a => a.ID == id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            ViewBag.Validar = false;
            bool tieneDeudaActiva = db.Deudas.Any(c => c.VentaId == id && c.ValorActual>0);
            if (tieneDeudaActiva && venta.TipoVenta == 2 && venta.Estado == 2)
            {
                ViewBag.Validar = true;
                TempData["Validar"] = true;
                return RedirectToAction("Index");
            }
            ViewBag.ClienteId = new SelectList(db.Clientes, "ID", "Clastname", venta.ClienteId);
            ViewBag.VendedorId = new SelectList(db.Vendedores, "ID", "Nombre", venta.VendedorId);
            ViewBag.Nombre = venta.Cliente.Cfirstname + " " + venta.Cliente.Clastname;
            venta.DetallesVenta = db.VentaDetalles.Include(p => p.Producto).Where(de => de.VentaId == venta.ID).ToList();
            VentaViewModel v = new VentaViewModel()
            {
                Venta = venta
            };

            if (string.IsNullOrEmpty(venta.MensajeroId) || "0".Equals(venta.MensajeroId))
            {
                v.NombreMensajero = "";
            }
            else
            {
                int valor = int.Parse(venta.MensajeroId);
                v.NombreMensajero = db.Mensajeros.First(m => m.ID == valor).Nombre;
            }


            List<String> fotosList = new List<String>();
            foreach (var val in venta.ListFotoVentas)
            {
                fotosList.Add("data:" + val.ImageMimeType + ";base64," + Convert.ToBase64String(val.ImageContent));
            }
            v.ListSrcFotos = fotosList;

            VentaSer ventaSer = new VentaSer()
            {
                ID = (int)venta.ID,
                Estado = venta.Estado,
                CantTotalProductos = venta.CantTotalProductos,
                ClienteId = venta.ClienteId,
                CostoTotal = venta.CostoTotal,
                Descuento = venta.Descuento,
                FechaVenta = venta.FechaVenta,
                GanaMensajero = venta.GanaMensajero,
                GanaTotal = venta.GanaTotal,
                MensajeroId = venta.MensajeroId,
                PrecioTotal = venta.PrecioTotal,
                VendedorId = venta.VendedorId,
                TipoVenta = venta.TipoVenta,
                Descripcion = venta.Descripcion,
                CalcSalarioTrab = venta.CalcSalarioTrab,
                DetallesVenta = new List<DetalleVentaSer>()
            };
            foreach (var detalle in venta.DetallesVenta)
            {
                DetalleVentaSer deta = new DetalleVentaSer();
                deta.ID = detalle.ID;
                deta.Cantidad = detalle.Cantidad;
                deta.Costo = detalle.Producto.Npreccosto;
                deta.Descuento = detalle.Producto.Ndescuento;
                deta.Disponibilidad = detalle.Producto.Ndisponibilidad;
                deta.Garantia = detalle.Garantia;
                deta.Lote = detalle.Producto.Clote;
                deta.Precio = venta.TipoVenta == 1 ? detalle.Producto.Nprecminoris : detalle.Producto.Nprecmayoris;
                deta.ProductoId = detalle.ProductoId;
                deta.ProductoN = detalle.Producto.Carticulo;
                deta.VentaId = detalle.VentaId;
                ventaSer.DetallesVenta.Add(deta);
            }
            v.ObjJson = new JavaScriptSerializer().Serialize(ventaSer);
            return View(v);
        }

        // POST: Ventas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Edit([Bind(Include = "ID,FechaVenta,MensajeroId,VendedorId,ClienteId,GanaMensajero,Descuento,CostoTotal,GanaTotal,PrecioTotal,CantTotalProductos,Usuario,Estado,TipoVenta,DetallesVenta,Cliente,Vendedor,ListFotoVentas, CalcSalarioTrab")] Venta venta)
        {

            if (ModelState.IsValid)
            {
                var ventaInDb = db.Ventas
                    .Include(d => d.DetallesVenta)
                    .SingleOrDefault(e => e.ID == venta.ID);
                if (ventaInDb == null) return Json(new { Success = 0, ex = "No se pudo editar la venta debido que no se encontro id anterior" }); ;
                if (venta.Estado == 1)
                {
                    EditarVentaPedido(venta, ventaInDb);
                }
                else
                {
                    EditarVentaFinalizada(venta, ventaInDb);
                }







                return Json(new { Success = 1, ID = venta.ID, ex = "" });
            }
            ViewBag.Validar = false;
            return Json(new { Success = 0, ex = "No se pudo editar la venta debido a un error" });

        }

        private void EditarVentaPedido(Venta venta, Venta ventaInDb)
        {
            /*Restauro disponibilidad anterior*/
            List<VentaDetalle> listaOld = ventaInDb.DetallesVenta;
            foreach (var ventaDetalle in listaOld)
            {
                Producto prod = new Producto();
                prod = db.Productos.First(p => p.ID == ventaDetalle.ProductoId);
                prod.Ndisponibilidad += ventaDetalle.Cantidad;
                db.Entry(prod).State = EntityState.Modified;
                db.SaveChanges();
            }
            ///////////////////////////////////////////////////////////////
            decimal salarioTrabajador = 0;
            if (venta.TipoVenta != 2)
            {
                if (venta.CalcSalarioTrab)
                foreach (var detalle in venta.DetallesVenta)
                {
                    Producto pr = db.Productos.First(p => p.ID == detalle.ProductoId);
                    salarioTrabajador += detalle.Cantidad * pr.Ngananctrab;
                }
            }
           
            venta.GanaTrab = salarioTrabajador;
            venta.GanaTotal = venta.PrecioTotal - venta.CostoTotal - venta.GanaTrab - venta.Descuento;
            venta.Usuario = User.Identity.Name;
            int count = ventaInDb.DetallesVenta.Count;
            for (int i = 0; i < count; i++)
            {
                db.VentaDetalles.Remove(ventaInDb.DetallesVenta[0]);

            }
            db.SaveChanges();
            ventaInDb.DetallesVenta = venta.DetallesVenta;
            ventaInDb.Estado = venta.Estado;
            ventaInDb.CantTotalProductos = venta.CantTotalProductos;
            ventaInDb.ClienteId = venta.ClienteId;
            ventaInDb.CostoTotal = venta.CostoTotal;
            ventaInDb.Descuento = venta.Descuento;
            //ventaInDb.FechaVenta = venta.FechaVenta;
            ventaInDb.GanaMensajero = venta.GanaMensajero;
            ventaInDb.GanaTotal = venta.GanaTotal;
            ventaInDb.MensajeroId = venta.MensajeroId;
            ventaInDb.PrecioTotal = venta.PrecioTotal;
            ventaInDb.VendedorId = venta.VendedorId;
            ventaInDb.TipoVenta = venta.TipoVenta;
            ventaInDb.GanaTrab = venta.GanaTrab;
            ventaInDb.Descripcion = venta.Descripcion;
            ventaInDb.CalcSalarioTrab = venta.CalcSalarioTrab;
            db.Entry(ventaInDb).State = EntityState.Modified;
            db.SaveChanges();
            //REBAJAR Disponibilidad
            List<VentaDetalle> lista = venta.DetallesVenta;
            foreach (var ventaDetalle in lista)
            {
                Producto prod = new Producto();
                prod = db.Productos.First(p => p.ID == ventaDetalle.ProductoId);
                prod.Ndisponibilidad -= ventaDetalle.Cantidad;
                db.Entry(prod).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        private void EditarVentaFinalizada(Venta venta, Venta ventaInDb)
        {
            Finanzas f = db.Finanzas.First();
            //REAJUSTANDO VALORES VIEJOS UQE SERAN MOIFICADOS
            f.Ganancias -= ventaInDb.GanaTotal;
           /* Registrar_Movimiento(7, "Disminuye ganancia por edición de venta",
                "Se modifica una venta y disminuye la ganancia en: " + ventaInDb.GanaTotal + (" (CORRECCION)"),
                ventaInDb.GanaTotal, 0);*/

            foreach (var deta in ventaInDb.DetallesVenta)
            {
                Producto p = db.Productos.First(r => r.ID == deta.ProductoId);

                /*Registrar_Movimiento(4, "Aumenta valor en Productos por edición de venta",
                    "Se edita una venta aumenta valor en productos en: " + p.Npreccosto * deta.Cantidad + " (CORRECCION)",
                    p.Npreccosto * deta.Cantidad, 0);*/
                f.ValorProductos += p.Npreccosto * deta.Cantidad;

                /*Registrar_Movimiento(4, "Disminuye el efectivo por edición de venta",
                    "Se edita una venta y disminuye el efectivo en: " + p.Npreccosto * deta.Cantidad + " (CORRECCION)",
                    p.Npreccosto * deta.Cantidad, 0);*/
                f.ValorEfectivo -= p.Npreccosto * deta.Cantidad;
            }
            /*Registrar_Movimiento(5, "Restaurando  el efectivo destinado a comisión por edición de venta",
                   "Se edita una venta y disminuye el efectivo destinado a comisión en: " + ventaInDb.GanaTrab + " (CORRECCION)",
                   ventaInDb.GanaTrab, 0);*/
            f.ValorEfectivo -= ventaInDb.GanaTrab;

            ////////////////////////////////////////////////////////////////////////////////////////////////

            /*Restauro disponibilidad anterior*/
            List<VentaDetalle> listaOld = ventaInDb.DetallesVenta;
            foreach (var ventaDetalle in listaOld)
            {
                Producto prod = new Producto();
                prod = db.Productos.First(p => p.ID == ventaDetalle.ProductoId);
                prod.Ndisponibilidad += ventaDetalle.Cantidad;
                db.Entry(prod).State = EntityState.Modified;
                db.SaveChanges();
            }
            ///////////////////////////////////////////////////////////////
            decimal salarioTrabajador = 0;
            if (venta.TipoVenta != 2)
            {
                if (venta.CalcSalarioTrab)
                foreach (var detalle in venta.DetallesVenta)
                {
                    Producto pr = db.Productos.First(p => p.ID == detalle.ProductoId);
                    salarioTrabajador += detalle.Cantidad*pr.Ngananctrab;
                }
            }
            venta.GanaTrab = salarioTrabajador;
            venta.GanaTotal = venta.PrecioTotal - venta.CostoTotal - venta.GanaTrab - venta.Descuento;
            venta.Usuario = User.Identity.Name;
            int count = ventaInDb.DetallesVenta.Count;
            for (int i = 0; i < count; i++)
            {
                db.VentaDetalles.Remove(ventaInDb.DetallesVenta[0]);
            }
            db.SaveChanges();
            ventaInDb.DetallesVenta = venta.DetallesVenta;
            ventaInDb.Estado = venta.Estado;
            ventaInDb.CantTotalProductos = venta.CantTotalProductos;
            ventaInDb.ClienteId = venta.ClienteId;
            ventaInDb.CostoTotal = venta.CostoTotal;
            ventaInDb.Descuento = venta.Descuento;
            //ventaInDb.FechaVenta = venta.FechaVenta;
            ventaInDb.GanaMensajero = venta.GanaMensajero;
            ventaInDb.GanaTotal = venta.GanaTotal;
            ventaInDb.MensajeroId = venta.MensajeroId;
            ventaInDb.PrecioTotal = venta.PrecioTotal;
            ventaInDb.VendedorId = venta.VendedorId;
            ventaInDb.TipoVenta = venta.TipoVenta;
            ventaInDb.GanaTrab = venta.GanaTrab;
            ventaInDb.Descripcion = venta.Descripcion;
            ventaInDb.CalcSalarioTrab = venta.CalcSalarioTrab;
            db.Entry(ventaInDb).State = EntityState.Modified;
            db.SaveChanges();
            //REBAJAR Disponibilidad
            List<VentaDetalle> lista = venta.DetallesVenta;
            foreach (var ventaDetalle in lista)
            {
                Producto prod = new Producto();
                prod = db.Productos.First(p => p.ID == ventaDetalle.ProductoId);
                prod.Ndisponibilidad -= ventaDetalle.Cantidad;
                db.Entry(prod).State = EntityState.Modified;
                db.SaveChanges();
            }
            //NUEVOS VALORES Movimiento
            f.Ganancias += venta.GanaTotal;
            Registrar_Movimiento(7, "Aumenta ganancia por edición de venta",
                "Se edita una venta y aumenta la ganancia en: " + venta.GanaTotal,
                venta.GanaTotal, 0,0);

            foreach (var deta in venta.DetallesVenta)
            {
                Producto p = db.Productos.First(r => r.ID == deta.ProductoId);

                Registrar_Movimiento(4, "Disminuye valor en Productos por edición de venta",
                    "Se edita una venta disminuyendo valor en productos en: " + p.Npreccosto * deta.Cantidad,
                    p.Npreccosto * deta.Cantidad, 0,venta.ID);
                f.ValorProductos -= p.Npreccosto * deta.Cantidad;

                Registrar_Movimiento(4, "Aumenta el efectivo por edición de venta",
                    "Se edita una venta y aumenta el efectivo en: " + p.Npreccosto * deta.Cantidad,
                    p.Npreccosto * deta.Cantidad, 0, venta.ID);
                f.ValorEfectivo += p.Npreccosto * deta.Cantidad;
            }
            Registrar_Movimiento(5, "Aumenta  el efectivo destinado a comisión por edición de venta",
                  "Se edita una venta y aumenta el efectivo destinado a comisión en: " + ventaInDb.GanaTrab + "",
                  ventaInDb.GanaTrab, 0,0);
            f.ValorEfectivo += ventaInDb.GanaTrab;


            f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
            db.Entry(f).State = EntityState.Modified;
            db.SaveChanges();
        }

        // GET: Ventas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {

           /* Seguridad s = new Seguridad();
            if (!s.ValidarSeguridad())
            {
                return View("Error");
            }*/
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Venta venta = await db.Ventas.Include(v => v.Cliente).Include(c => c.Vendedor).Where(t => t.ID == id).FirstAsync();
            if (venta == null)
            {
                return HttpNotFound();
            }
            ViewBag.Validar = false;
            bool tieneDeudaActiva = db.Deudas.Any(c => c.VentaId == id && c.ValorActual > 0);
            if (tieneDeudaActiva && venta.TipoVenta == 2 && venta.Estado == 2)
            {
                ViewBag.Validar = true;
                TempData["Validar"] = true;
                return RedirectToAction("Index");
            }
            venta.ListFotoVentas = db.FotoVentas.Where(f => f.VentaId == venta.ID).ToList();
            venta.DetallesVenta = db.VentaDetalles.Where(f => f.VentaId == venta.ID).Include(p => p.Producto).ToList();

            DeleteVentaViewModel d = new DeleteVentaViewModel()
            {
                Venta = venta,


            };

            if (string.IsNullOrEmpty(venta.MensajeroId) || "0".Equals(venta.MensajeroId))
            {
                d.NombreMensajero = "Venta Local";
            }
            else
            {
                int valor = int.Parse(venta.MensajeroId);
                d.NombreMensajero = db.Mensajeros.First(m => m.ID == valor).Nombre;
            }


            List<String> fotosList = new List<String>();
            foreach (var val in venta.ListFotoVentas)
            {
                fotosList.Add("data:" + val.ImageMimeType + ";base64," + Convert.ToBase64String(val.ImageContent));
            }
            d.ListImgSrc = fotosList;
            ViewBag.Mensaje = TempData["Mensaje"];
            if (ViewBag.Mensaje == null)
            {
                ViewBag.Mensaje = false;
            }
            return View(d);

        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ViewBag.Validar = false;
            Venta venta = await db.Ventas.FindAsync(id);

            if (venta != null && venta.Estado != 4)
            {
                List<VentaDetalle> list = db.VentaDetalles.Where(c => c.VentaId == id).Include(p => p.Producto).ToList();
                Finanzas f = db.Finanzas.First();
                if (venta.Estado != 1)
                {
                    
                    f.ValorEfectivo -= venta.GanaTrab;
                    Registrar_Movimiento(5, "Disminuye el efectivo destinado a comisión por eliminación de venta",
                          "Se elimina una venta y disminuye el efectivo destinado a comisión en: " + venta.GanaTrab,
                          venta.GanaTrab, 0,0);

                    f.Ganancias -= venta.GanaTotal;
                    Registrar_Movimiento(7, "Disminuye la ganancia por eliminación de venta",
                       "Se elimina una venta y disminuye la ganancia en: " + venta.GanaTotal,
                       venta.GanaTotal, 0,0);
                   
                }
                    
                foreach (var deta in list)
                {
                    Producto p = deta.Producto;
                    p.Ndisponibilidad += deta.Cantidad;
                    db.Entry(p).State = EntityState.Modified;
                    db.SaveChanges();

                    if (venta.Estado != 1)
                    {
                        Registrar_Movimiento(4, "Aumenta valor en Productos por eliminación de venta",
                            "Se elimina una venta y aumenta disponibilidad del producto: " + p.Carticulo +
                            ", aumentando valor en productos en: " + p.Npreccosto * deta.Cantidad,
                            p.Npreccosto * deta.Cantidad, 0,0);
                        f.ValorProductos += p.Npreccosto * deta.Cantidad;

                        Registrar_Movimiento(4, "Disminuye el efectivo por eliminación de venta",
                            "Se elimina una venta y disminuye el efectivo en: " + p.Npreccosto * deta.Cantidad,
                            p.Npreccosto * deta.Cantidad, 0,0);
                        f.ValorEfectivo -= p.Npreccosto * deta.Cantidad;

                      
                    }


                }
                f.ValorCapital = f.ValorProductos + f.ValorEfectivo + f.Ganancias;
                db.Entry(f).State = EntityState.Modified;
                db.SaveChanges();

                db.Ventas.Remove(venta);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            TempData["Mensaje"] = true;
            return RedirectToAction("Delete", id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Ventas/addCliente
        public ActionResult AddCliente()
        {

            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCliente([Bind(Include = "ID,Clastname,Cfirstname,Ccidentidad,Cnumtelefono,Lactivo,Cdireccion,Cnotas")] Cliente cliente)
        {
            ViewBag.Validar = false;
            var existe = db.Clientes.Any(c => c.Ccidentidad == cliente.Ccidentidad);
            if (ModelState.IsValid && !existe)
            {
                db.Clientes.Add(cliente);
                db.SaveChanges();
                ViewBag.Nombre = cliente.Cfirstname + " " + cliente.Clastname;
                ViewBag.ClienteId = new SelectList(db.Clientes, "ID", "Clastname");
                ViewBag.VendedorId = new SelectList(db.Vendedores, "ID", "Nombre");

                Venta ve = new Venta()
                {
                    TipoVenta = 1,
                    DetallesVenta = new List<VentaDetalle>(),
                    FechaVenta = DateTime.Now

                };

                VentaViewModel v = new VentaViewModel()
                {
                    Venta = ve,
                };
                return View("Create", v);
            }

            return View(cliente);
        }


        [HttpPost]
        public ActionResult AddVentaDetalle([Bind(Include = "ID,FechaVenta,MensajeroId,VendedorId,ClienteId,GanaMensajero,Descuento,CostoTotal,GanaTotal,PrecioTotal,CantTotalProductos,Usuario,Estado,TipoVenta,DetallesVenta")] Venta venta)
        {
            ViewBag.Validar = false;
            if (venta.DetallesVenta == null)
            {
                venta.DetallesVenta = new List<VentaDetalle>();
            }

            ViewBag.Min = 1 == venta.TipoVenta;

            if (venta.ClienteId > 0)
            {
                Cliente cliente = db.Clientes.First(c => c.ID == venta.ClienteId);
                ViewBag.Nombre = cliente.Cfirstname + " " + cliente.Clastname;
            }
            else
            {
                ViewBag.Nombre = " ";
            }

            ViewBag.ClienteId = new SelectList(db.Clientes, "ID", "Clastname");
            ViewBag.VendedorId = new SelectList(db.Vendedores, "ID", "Nombre");

            VentaViewModel v = new VentaViewModel()
            {
                ListSrcFotos = new List<string>(),
                Venta = venta
            };

            return Json(new { Success = 1, ex = new Exception("Unable to save").Message.ToString() });
        }


        public ActionResult ListUpload(int? Id)
        {
            ViewBag.Validar = false;
            var lista = new List<FotoVenta>();
            lista = db.FotoVentas.Where(f => f.VentaId == Id).Include(f => f.Venta).ToList();
            ViewBag.ID = Id;
            return View(lista);
        }

        public ActionResult Upload(int? Id)
        {
            ViewBag.Validar = false;
            FotoVentaViewModel f = new FotoVentaViewModel()
            {
                Venta = db.Ventas.Find(Id),
            };



            return View(f);
        }

        // POST: /Download/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public ActionResult CreateUpload([Bind(Include = "ID,Title,Description,ImageContent,ImageMimeType,ImageName,FileContent,FileMimeType,FileName")] FotoVenta foto, int? ventaId)
        {
            ViewBag.Validar = false;
            if (ModelState.IsValid)
            {

                var ImageName = "";
                var ImageMimeType = "";
                byte[] ImageContent = { };

                var FileName = "";
                var FileMimeType = "";
                byte[] FileContent = { };

                foreach (string upload in Request.Files)
                {
                    var mimeType = Request.Files[upload].ContentType;
                    var fileStream = Request.Files[upload].InputStream;
                    var fileName = Path.GetFileName(Request.Files[upload].FileName);
                    var fileLength = Request.Files[upload].ContentLength;
                    var fileData = new byte[fileLength];
                    fileStream.Read(fileData, 0, fileLength);

                    if (upload == "FileUpload1")
                    {
                        if (!string.IsNullOrEmpty(fileName) && fileData.Length > 0 && !string.IsNullOrEmpty(mimeType))
                        {
                            ImageName = fileName;
                            ImageContent = fileData;
                            ImageMimeType = mimeType;
                        }


                    }
                    else if (upload == "FileUpload2")
                    {
                        if (!string.IsNullOrEmpty(fileName) && fileData.Length > 0 && !string.IsNullOrEmpty(mimeType))
                        {
                            FileName = fileName;
                            FileContent = fileData;
                            FileMimeType = mimeType;
                        }
                        else
                        {
                            // Errors = Resource.ResourceStore_enUS.selectFile;
                        }
                    }
                }

                var carga = new FotoVenta();

                carga.Title = foto.Title;
                carga.Description = foto.Description;
                carga.ImageName = ImageName;
                carga.ImageContent = ImageContent;
                carga.ImageMimeType = ImageMimeType;
                carga.FileName = FileName;
                carga.Bfotos = FileContent;
                carga.FileMimeType = FileMimeType;
                carga.VentaId = (int)ventaId;

                db.FotoVentas.Add(carga);

                db.SaveChanges();
                return RedirectToAction("ListUpload", new { Id = ventaId });

            }

            return View("Upload");
        }


        public FileContentResult GetFile(int id)
        {
            byte[] fileContent = null;
            string mimeType = ""; string fileName = "";

            var download = db.FotoProductos.Find(id);

            fileContent = download.Bfotos;
            mimeType = download.FileMimeType;
            fileName = download.FileName;
            return File(fileContent, mimeType, fileName);
        }


        // GET: Productoes/Delete/5
        public async Task<ActionResult> DeleteF(int? id)
        {
            ViewBag.Validar = false;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FotoVenta foto = await db.FotoVentas.FindAsync(id);
            foto.FileName = "data:" + foto.ImageMimeType + ";base64," + Convert.ToBase64String(foto.ImageContent);
            if (foto == null)
            {
                return HttpNotFound();
            }
            return View(foto);
        }

        // POST: Productoes/Delete/5
        [HttpPost, ActionName("DeleteF")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteFConfirmed(int id)
        {
            ViewBag.Validar = false;
            FotoVenta foto = await db.FotoVentas.FindAsync(id);
            db.FotoVentas.Remove(foto);
            await db.SaveChangesAsync();
            return RedirectToAction("ListUpload", new { Id = foto.VentaId });
        }




        // GET: Productoes/Delete/5
        public async Task<ActionResult> CambiarEstado(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venta venta = await db.Ventas.Include(v => v.Cliente).Include(c => c.Vendedor).Where(t => t.ID == id).FirstAsync();
            if (venta == null)
            {
                return HttpNotFound();
            }

            ViewBag.Validar = false;
            bool tieneDeudaActiva = db.Deudas.Any(c => c.VentaId == id && c.ValorActual>0);
            if (tieneDeudaActiva && venta.TipoVenta == 2 && venta.Estado == 2)
            {
                ViewBag.Validar = true;
                TempData["Validar"] = true;
                return RedirectToAction("Index");
            }

            venta.ListFotoVentas = db.FotoVentas.Where(f => f.VentaId == venta.ID).ToList();
            venta.DetallesVenta = db.VentaDetalles.Where(f => f.VentaId == venta.ID).Include(p => p.Producto).ToList();

            DetailVentaViewModel d = new DetailVentaViewModel()
            {
                Venta = venta,


            };

            if (string.IsNullOrEmpty(venta.MensajeroId) || "0".Equals(venta.MensajeroId))
            {
                d.NombreMensajero = "Venta Local";
            }
            else
            {
                int valor = int.Parse(venta.MensajeroId);
                d.NombreMensajero = db.Mensajeros.First(m => m.ID == valor).Nombre;
            }

            return View(d);
        }
        [HttpPost]
        public ActionResult CambiarEstado([Bind(Include = "ID,FechaVenta,MensajeroId,VendedorId,ClienteId,GanaMensajero,Descuento,CostoTotal,GanaTotal,PrecioTotal,CantTotalProductos,Usuario,Estado,TipoVenta,DetallesVenta,Cliente,Vendedor,ListFotoVentas")] Venta venta, string valorPagado)
        {
            int estadonuevo = venta.Estado;
            
            venta = db.Ventas.Include(dd => dd.DetallesVenta).Include(v => v.Cliente).Include(c => c.Vendedor).First(t => t.ID == venta.ID);
            ViewBag.Validar = false;
            decimal realPagar = venta.PrecioTotal - venta.Descuento;
            decimal pagado = decimal.Parse(valorPagado);
            if (pagado < realPagar && venta.TipoVenta==2)
            {
                Deuda d = new Deuda()
                {
                    ValorInicial = venta.PrecioTotal - pagado - venta.Descuento,
                    ValorActual = venta.PrecioTotal - pagado - venta.Descuento,
                    FechaCreacion = DateTime.Now,
                    UltimoPago = DateTime.Now,
                    VentaId = (int)venta.ID
                };
                if (pagado >= venta.CostoTotal)
                {
                    d.CostoProductoAPagar = 0;
                    decimal resto = pagado - venta.CostoTotal;
                    if (resto >= venta.GanaTrab)
                    {
                        d.GanaTrabAPagar = 0;
                    }
                    else
                    {
                        d.GanaTrabAPagar = venta.GanaTrab-resto;
                    }
                }
                else
                {
                    d.CostoProductoAPagar = venta.CostoTotal- pagado ;
                    d.GanaTrabAPagar = venta.GanaTrab;
                }
                
                db.Deudas.Add(d);
                db.SaveChanges();
                CambiarEstado_ConDeuda(venta, pagado, estadonuevo);
            }
            else
            {
                CambiarEstado_SinDeuda(venta, estadonuevo);
            }




            return RedirectToAction("Index");
        }

        private void CambiarEstado_ConDeuda(Venta venta, decimal pagado, int estadonuevo)
        {
            
            Finanzas f = db.Finanzas.First();
            if (venta.Estado == 1 && estadonuevo == 2)
            {
                decimal precioCosto = venta.CostoTotal;
                if (pagado > precioCosto)
                {
                    foreach (var deta in venta.DetallesVenta)
                    {
                        Producto p = db.Productos.First(r => r.ID == deta.ProductoId);

                        Registrar_Movimiento(4, "Disminuye valor en Productos por finalización de venta con deuda",
                            "Se finaliza una venta con deuda disminuyendo valor en productos en: " + p.Npreccosto * deta.Cantidad,
                            p.Npreccosto * deta.Cantidad, 0, venta.ID);
                        f.ValorProductos -= p.Npreccosto * deta.Cantidad;

                        Registrar_Movimiento(4, "Aumenta el efectivo por finalización de venta con deuda",
                            "Se finaliza una venta con deuda y aumenta el efectivo en: " + p.Npreccosto * deta.Cantidad,
                            p.Npreccosto * deta.Cantidad, 0, venta.ID);
                        f.ValorEfectivo += p.Npreccosto * deta.Cantidad;
                    }
                    decimal resto = pagado - venta.CostoTotal;
                    if (resto >= venta.GanaTrab)
                    {
                        /*f.ValorEfectivo += venta.GanaTrab;*/
                        Registrar_Movimiento(5, "Aumenta el efectivo de salario(COMISIÓN) de trabajador(con deuda)",
                                "Se finaliza una venta(con deuda) y aumenta el efectivo destinado a salario(COMISIÓN) en: " + venta.GanaTrab,
                                 venta.GanaTrab, 0,0);
                        resto -= venta.GanaTrab;
                        if (resto > 0)
                        {
                            f.Ganancias += resto;
                            Registrar_Movimiento(7, "Aumenta ganancia por finalización de venta con deuda",
                           "Se cambia estado de una venta con deuda a finalizada y aumenta la ganancia en: " + (pagado - precioCosto),
                           pagado - precioCosto, 0,0);
                        }
                    }
                    else
                    {
                        f.ValorEfectivo += resto;
                        Registrar_Movimiento(4, "Aumenta el efectivo (con deuda)",
                                "Se finaliza una venta(con deuda) y aumenta el efectivo en: " + resto,
                                 resto, 0,0);
                       
                    }
                    
                  
                }
                else
                {
                    f.ValorEfectivo += pagado;
                    Registrar_Movimiento(4, "Aumenta el efectivo (con deuda)",
                            "Se finaliza una venta(con deuda) y aumenta el efectivo en: " + pagado,
                             pagado, 0, venta.ID);
                    Registrar_Movimiento(4, "Disminuye valor en Productos por finalización de venta con deuda",
                          "Se finaliza una venta con deuda disminuyendo valor en productos en: " + pagado,
                          pagado, 0, venta.ID);
                    f.ValorProductos -= pagado;
                }
             

            }

            f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
            db.Entry(f).State = EntityState.Modified;
            db.SaveChanges();
            venta.Estado = estadonuevo;
            db.Entry(venta).State = EntityState.Modified;
            db.SaveChanges();



            venta.ListFotoVentas = db.FotoVentas.Where(b => b.VentaId == venta.ID).ToList();
            //venta.DetallesVenta = db.VentaDetalles.Where(a => a.VentaId == venta.ID).Include(p => p.Producto).ToList();
            DetailVentaViewModel d = new DetailVentaViewModel()
            {
                Venta = venta,


            };

            if (string.IsNullOrEmpty(venta.MensajeroId) || "0".Equals(venta.MensajeroId))
            {
                d.NombreMensajero = "Venta Local";
            }
            else
            {
                int valor = int.Parse(venta.MensajeroId);
                d.NombreMensajero = db.Mensajeros.First(m => m.ID == valor).Nombre;
            }
        }

        private void CambiarEstado_SinDeuda(Venta venta, int estadonuevo)
        {
            
            Finanzas f = db.Finanzas.First();
            if (venta.Estado == 2 && estadonuevo == 1)
            {
                f.Ganancias -= venta.GanaTotal;
                Registrar_Movimiento(7, "Disminuye ganancia por cambio de estado de venta(CORRECCION)",
                    "Se cambia estado a una venta y disminuye la ganancia en: " + venta.GanaTotal,
                    venta.GanaTotal, 0,0);
                //f.ValorEfectivo -= venta.GanaTrab;
                Registrar_Movimiento(5, "Disminuye el efectivo de salario(COMISION) de trabajador por cambio de estado de venta(CORRECCION)",
                        "Se cambia estado de venta y disminuye el efectivo destinado a salario en: " + venta.GanaTrab,
                         venta.GanaTrab, 0,0);
                
                foreach (var deta in venta.DetallesVenta)
                {
                    Producto p = db.Productos.First(r => r.ID == deta.ProductoId);

                    Registrar_Movimiento(4, "Aumenta valor en Productos por cambio de estado en venta",
                        "Se cambia estado de venta aumentando valor en productos en: " + p.Npreccosto * deta.Cantidad,
                        p.Npreccosto * deta.Cantidad, 0, venta.ID);
                    f.ValorProductos += p.Npreccosto * deta.Cantidad;

                    Registrar_Movimiento(4, "Disminuye el efectivo por cambio de estado en venta",
                        "Se cambia estado de una venta y disminuye el efectivo en: " + p.Npreccosto * deta.Cantidad,
                        p.Npreccosto * deta.Cantidad, 0, venta.ID);
                    f.ValorEfectivo -= p.Npreccosto * deta.Cantidad;
                }




            }
            else
            {
                if (venta.Estado == 1 && estadonuevo == 2)
                {
                    f.Ganancias += venta.GanaTotal;
                    Registrar_Movimiento(7, "Aumenta ganancia por finalizacion de venta",
                        "Se cambia estado de una venta a finalizada y aumenta la ganancia en: " + venta.GanaTotal,
                        venta.GanaTotal, 0,0);

                    foreach (var deta in venta.DetallesVenta)
                    {
                        Producto p = db.Productos.First(r => r.ID == deta.ProductoId);

                        Registrar_Movimiento(4, "Disminuye valor en Productos por finalización de venta",
                            "Se finaliza una venta disminuyendo valor en productos en: " + p.Npreccosto * deta.Cantidad,
                            p.Npreccosto * deta.Cantidad, 0, venta.ID);
                        f.ValorProductos -= p.Npreccosto * deta.Cantidad;

                        Registrar_Movimiento(4, "Aumenta el efectivo por finalización de venta",
                            "Se finaliza una venta y aumenta el efectivo en: " + p.Npreccosto * deta.Cantidad,
                            p.Npreccosto * deta.Cantidad, 0, venta.ID);
                        f.ValorEfectivo += p.Npreccosto * deta.Cantidad;
                    }
                    //f.ValorEfectivo += venta.GanaTrab;
                    Registrar_Movimiento(5, "Aumenta el efectivo de salario(COMISION) de trabajador",
                            "Se finaliza una venta y aumenta el efectivo destinado a salario en: " + venta.GanaTrab,
                             venta.GanaTrab, 0,0);
                    

                }
            }
            f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
            db.Entry(f).State = EntityState.Modified;
            db.SaveChanges();
            venta.Estado = estadonuevo;
            db.Entry(venta).State = EntityState.Modified;
            db.SaveChanges();



            venta.ListFotoVentas = db.FotoVentas.Where(b => b.VentaId == venta.ID).ToList();
            //venta.DetallesVenta = db.VentaDetalles.Where(a => a.VentaId == venta.ID).Include(p => p.Producto).ToList();
            DetailVentaViewModel d = new DetailVentaViewModel()
            {
                Venta = venta,


            };

            if (string.IsNullOrEmpty(venta.MensajeroId) || "0".Equals(venta.MensajeroId))
            {
                d.NombreMensajero = "Venta Local";
            }
            else
            {
                int valor = int.Parse(venta.MensajeroId);
                d.NombreMensajero = db.Mensajeros.First(m => m.ID == valor).Nombre;
            }
        }
        private void Registrar_Movimiento(int tipo, string nombre, string descripcion, decimal monto, int idProd, int? idVenta)
        {
            if (monto < 0) monto = monto * -1;
            int idV = 0;
            if (idVenta != null)
                idV = idVenta.Value;
            Movimiento m = new Movimiento()
            {
                TipoMovimiento = tipo,
                Descripcion = descripcion,
                FechaMovimiento = DateTime.Now,
                Monto = monto,
                Nombre = nombre,
                IdProducto = idProd,
                IdProCompra = 0,
                IdVenta = idV,
                Usuario = User.Identity.GetUserName()
            };
            db.Movimientos.Add(m);
            db.SaveChanges();
        }


        public ActionResult Devolucion(int? id)
        {
            /*Seguridad s = new Seguridad();
            if (!s.ValidarSeguridad())
            {
                return View("Error");
            }*/
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Venta venta = db.Ventas
                .Include(vv => vv.Cliente)
                .Include(p => p.Vendedor)

                .Include(f => f.ListFotoVentas)
                .First(a => a.ID == id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            ViewBag.Validar = false;
            bool tieneDeudaActivada = db.Deudas.Any(c => c.VentaId == id && c.ValorActual>0);
            if (tieneDeudaActivada && venta.TipoVenta == 2 && venta.Estado == 2)
            {
                ViewBag.Validar = true;
                TempData["Validar"] = true;
                return RedirectToAction("Index");
            }
            ViewBag.ClienteId = new SelectList(db.Clientes, "ID", "Clastname", venta.ClienteId);
            ViewBag.VendedorId = new SelectList(db.Vendedores, "ID", "Nombre", venta.VendedorId);
            ViewBag.Nombre = venta.Cliente.Cfirstname + " " + venta.Cliente.Clastname;
            venta.DetallesVenta = db.VentaDetalles.Include(p => p.Producto).Where(de => de.VentaId == venta.ID).ToList();
            VentaViewModel v = new VentaViewModel()
            {
                Venta = venta
            };

            if (string.IsNullOrEmpty(venta.MensajeroId) || "0".Equals(venta.MensajeroId))
            {
                v.NombreMensajero = "";
            }
            else
            {
                int valor = int.Parse(venta.MensajeroId);
                v.NombreMensajero = db.Mensajeros.First(m => m.ID == valor).Nombre;
            }

            
            
            List<String> fotosList = new List<String>();
            foreach (var val in venta.ListFotoVentas)
            {
                fotosList.Add("data:" + val.ImageMimeType + ";base64," + Convert.ToBase64String(val.ImageContent));
            }
            v.ListSrcFotos = fotosList;

            VentaSer ventaSer = new VentaSer()
            {
                ID = (int)venta.ID,
                Estado = venta.Estado,
                CantTotalProductos = venta.CantTotalProductos,
                ClienteId = venta.ClienteId,
                CostoTotal = venta.CostoTotal,
                Descuento = venta.Descuento,
                FechaVenta = venta.FechaVenta,
                GanaMensajero = venta.GanaMensajero,
                GanaTotal = venta.GanaTotal,
                MensajeroId = venta.MensajeroId,
                PrecioTotal = venta.PrecioTotal,
                VendedorId = venta.VendedorId,
                TipoVenta = venta.TipoVenta,
                DetallesVenta = new List<DetalleVentaSer>()
            };
            foreach (var detalle in venta.DetallesVenta)
            {
                DetalleVentaSer deta = new DetalleVentaSer();
                deta.ID = detalle.ID;
                deta.Cantidad = detalle.Cantidad;
                deta.Costo = detalle.Producto.Npreccosto;
                deta.Descuento = detalle.Producto.Ndescuento;
                deta.Disponibilidad = detalle.Producto.Ndisponibilidad;
                deta.Garantia = detalle.Garantia;
                deta.Lote = detalle.Producto.Clote;
                deta.Precio = venta.TipoVenta == 1 ? detalle.Producto.Nprecminoris : detalle.Producto.Nprecmayoris;
                deta.ProductoId = detalle.ProductoId;
                deta.ProductoN = detalle.Producto.Carticulo;
                deta.VentaId = detalle.VentaId;
                ventaSer.DetallesVenta.Add(deta);
            }
            v.ObjJson = new JavaScriptSerializer().Serialize(ventaSer);
            v.Devolucion = new Devolucion()
            {
                Fecha = DateTime.Now,
                VentaId = (int)id
            };
            return View(v);
        }



        public class DevolucionSer
        {
            public int? Id { get; set; }
            public string Notas { get; set; }
            public int VentaId { get; set; }
            public decimal DescuentoSalario { get; set; }
            public int Cantidad { get; set; }
            public DateTime Fecha { get; set; }
            public List<DetalleDevolucionSer> Detalles { get; set; }

        }
        public class DetalleDevolucionSer
        {
            public int idProd { get; set; }
            public int cant { get; set; }


        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Devolucion([Bind(Include = "Id,Notas,VentaId,DescuentoSalario,Fecha,Cantidad,Detalles")] DevolucionSer devolucion)
        {
          ViewBag.Validar = false;
            /*foreach (var cade in ModelState.Keys.ToList())
            {
                var a = ModelState[cade].Errors.ToList();
            }*/
            if (ModelState.IsValid)
            {
                try
                {
                    Devolucion dev = new Devolucion()
                    {
                        VentaId = devolucion.VentaId,
                        Cantidad = devolucion.Cantidad,
                        DescuentoSalario = devolucion.DescuentoSalario,
                        Fecha = devolucion.Fecha,
                        Notas = devolucion.Notas
                    };
                    if (dev.Notas == null)
                    {
                        dev.Notas = "";
                    }
                    Venta venta = db.Ventas.First(v => v.ID == devolucion.VentaId);
                    venta.Estado = 3;
                    Finanzas f = db.Finanzas.First();
                    foreach (var detalle in devolucion.Detalles)
                    {
                        VentaDetalle vd =
                            db.VentaDetalles.First(d => d.ProductoId == detalle.idProd && d.VentaId == venta.ID);
                        vd.Cantidad -= detalle.cant;
                        venta.CantTotalProductos -= detalle.cant;
                        Producto p = db.Productos.First(d => d.ID == detalle.idProd);
                        p.Ndisponibilidad += detalle.cant;
                        db.Entry(p).State = EntityState.Modified;
                        db.SaveChanges();

                        f.ValorEfectivo -= detalle.cant * p.Npreccosto;
                        Registrar_Movimiento(4, "Disminución de efectivo por devolución", "Se devuelven " + detalle.cant + " del producto " + p.Carticulo + ". Disminuyendo efectivo en " + detalle.cant * p.Npreccosto, detalle.cant * p.Npreccosto, 0, venta.ID);

                        f.ValorProductos += detalle.cant * p.Npreccosto;
                        Registrar_Movimiento(4, "Aumento de cantidad de producto por devolución", "Se devuelven " + detalle.cant + " del producto " + p.Carticulo + ". Aumentando valor en producto en " + detalle.cant * p.Npreccosto, detalle.cant * p.Npreccosto, 0, venta.ID);


                        if (venta.TipoVenta == 1)
                        {
                            venta.PrecioTotal -= detalle.cant * p.Nprecminoris;
                            venta.GanaTotal -= (p.Nprecminoris * detalle.cant) - p.Ngananctrab-p.Npreccosto;
                            f.Ganancias -= (p.Nprecminoris * detalle.cant) - p.Ngananctrab - p.Npreccosto;
                            decimal valor = (p.Nprecminoris * detalle.cant) - p.Ngananctrab - p.Npreccosto;
                            Registrar_Movimiento(7, "Disminuye ganancia por devolución", "Se devuelven " + detalle.cant + " del producto " + p.Carticulo + ". Disminuyendo la ganancia en " + valor, (p.Nprecminoris * detalle.cant) - p.Ngananctrab-p.Npreccosto, 0,0);

                        }
                        else
                        {
                            venta.PrecioTotal -= detalle.cant * p.Nprecmayoris;
                            venta.GanaTotal -= (p.Nprecmayoris * detalle.cant) - p.Ngananctrab-p.Npreccosto;
                            f.Ganancias -= (p.Nprecmayoris * detalle.cant) - p.Ngananctrab-p.Npreccosto;
                            decimal valor = (p.Nprecmayoris * detalle.cant) - p.Ngananctrab-p.Npreccosto;
                            Registrar_Movimiento(7, "Disminuye ganancia por devolución", "Se devuelven " + detalle.cant + " del producto " + p.Carticulo + ". Disminuyendo la ganancia en " + valor, (p.Nprecmayoris * detalle.cant) - p.Ngananctrab- p.Npreccosto, 0,0);

                        }
                        f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
                        db.Entry(f).State = EntityState.Modified;
                        db.Entry(vd).State = EntityState.Modified;
                        db.SaveChanges();
                    }




                    db.Devoluciones.Add(dev);
                    db.Entry(venta).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { Success = 1, ID = devolucion.Id, ex = "" });
                }
                catch (Exception e)
                {
                    return Json(new { Success = 0, ex = "No se pudo realizar la devolución debido a un error: " + e.Message });
                }
            }

            return Json(new { Success = 0, ex = "No se pudo realizar la devolución debido a un error" });
            
        }
    }
}
