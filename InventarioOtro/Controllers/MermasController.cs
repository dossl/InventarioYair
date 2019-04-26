using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using InventarioOtro.Models;
using InventarioOtro.ViewModels;
using Microsoft.AspNet.Identity;
using PagedList;

namespace InventarioOtro.Controllers
{
    [Authorize]
    public class MermasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Mermas
        public async Task<ActionResult> Index(string searchString, string categoriaId, int? page, int? paso, string sortOrder)
        {
            /*Seguridad s = new Seguridad();
            if (!s.ValidarSeguridad())
            {
                return View("Error");
            }*/
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
            //var a = db.ApplicationRole.ToList();
            IndexMermaViewModel ip = new IndexMermaViewModel()
            {
                ListCategoriaProductos = db.CategoriaProductoes.ToList(),

            };
            var lista = new List<Merma>();
            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.Search = searchString;
                if (!String.IsNullOrWhiteSpace(categoriaId))
                {
                    ViewBag.CategoriaId = categoriaId;

                    lista = await db.Mermas.Include(p => p.Producto).Include(d => d.Producto.CategoriaProducto)
                        .Where(m => m.Producto.Carticulo.ToLower().Contains(searchString.ToLower()) &&
                                    m.Producto.CategoriaProductoId.ToString().Equals(categoriaId))
                        .ToListAsync();

                }
                else
                {
                    ViewBag.CategoriaId = "";
                    lista = await db.Mermas.Include(p => p.Producto).Include(d => d.Producto.CategoriaProducto)
                        .Where(m => m.Producto.Carticulo.ToLower().Contains(searchString.ToLower())).ToListAsync();
                }


            }
            else
            {
                ViewBag.Search = "";
                if (!String.IsNullOrEmpty(categoriaId))
                {
                    ViewBag.CategoriaId = categoriaId;
                    lista = await db.Mermas.Include(p => p.Producto).Include(d => d.Producto.CategoriaProducto)
                        .Where(m => m.Producto.CategoriaProductoId.ToString().Equals(categoriaId)).ToListAsync();
                }
                else
                {
                    ViewBag.CategoriaId = "";
                    lista = await db.Mermas.Include(p => p.Producto).Include(d => d.Producto.CategoriaProducto)
                        .ToListAsync();
                }


            }
            if (lista.Count <= ViewBag.Paso)
            {
                ViewBag.Page = 1;
            }

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "articulo_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.CatSortParm = sortOrder == "cat" ? "cat_desc" : "cat";
            ViewBag.DisSortParm = sortOrder == "dis" ? "dis_desc" : "dis";
            ViewBag.LoteSortParm = sortOrder == "lote" ? "lote_desc" : "lote";
            ViewBag.PrecioSortParm= sortOrder == "precio" ? "precio_desc" : "precio";
            ViewBag.CostoSortParm = sortOrder == "costo" ? "costo_desc" : "costo";


            switch (sortOrder)
            {
                case "articulo_desc":
                    lista = lista.OrderByDescending(l => l.Producto.Carticulo).ToList();
                    break;
                case "Date":
                    lista = lista.OrderBy(l => l.Producto.Dfechacreacion).ToList();
                    break;
                case "date_desc":
                    lista = lista.OrderByDescending(l => l.Producto.Dfechacreacion).ToList();
                    break;
                case "cat":
                    lista = lista.OrderBy(l => l.Producto.CategoriaProducto.Cnombre).ToList();
                    break;
                case "cat_desc":
                    lista = lista.OrderByDescending(l => l.Producto.CategoriaProducto.Cnombre).ToList();
                    break;
                case "dis":
                    lista = lista.OrderBy(l => l.Cantidad).ToList();
                    break;
                case "dis_desc":
                    lista = lista.OrderByDescending(l => l.Cantidad).ToList();
                    break;
                case "lote":
                    lista = lista.OrderBy(l => l.Producto.Clote).ToList();
                    break;
                case "lote_desc":
                    lista = lista.OrderByDescending(l => l.Producto.Clote).ToList();
                    break;
                case "precio":
                    lista = lista.OrderBy(l => l.Precio).ToList();
                    break;
                case "precio_desc":
                    lista = lista.OrderByDescending(l => l.Precio).ToList();
                    break;
                case "costo":
                    lista = lista.OrderBy(l => l.Cantidad*l.Producto.Npreccosto).ToList();
                    break;
                case "costo_desc":
                    lista = lista.OrderByDescending(l => l.Cantidad * l.Producto.Npreccosto).ToList();
                    break;
                default:
                    lista = lista.OrderBy(l => l.Producto.Carticulo).ToList();
                    break;
            }

            ip.ListProductos = lista.ToPagedList((int) ViewBag.Page, (int) ViewBag.Paso);
            return View(ip);
        }

        // GET: Mermas/Details/5
            public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Merma merma = await db.Mermas.Include(m => m.Producto).Include(mp=>mp.Producto.CategoriaProducto).FirstAsync(m=>m.Id==id);
            
            if (merma == null)
            {
                return HttpNotFound();
            }
            
            return View(merma);
        }

        // GET: Mermas/Create
        public ActionResult Create()
        {
            ViewBag.Mensaje = false;
            ViewBag.ProductoId = new SelectList(db.Productos, "ID", "Carticulo");
            return View();
        }

        // POST: Mermas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ProductoId,Cantidad,Precio")] Merma merma)
        {

            bool seguir = !db.Mermas.Any(m => m.ProductoId == merma.ProductoId);
            
            if (ModelState.IsValid && seguir)
            {
                Finanzas f = db.Finanzas.First();
                Producto p = db.Productos.First(d => d.ID == merma.ProductoId);
                p.Ndisponibilidad -= merma.Cantidad;
                f.ValorProductos -= merma.Cantidad * p.Npreccosto;
                f.ValorCapital = f.ValorProductos + f.ValorEfectivo + f.Ganancias;
                Registrar_Movimiento(6,"Producto: "+p.Carticulo+" a Merma.","Se pasa "+merma.Cantidad+" unidades del poducto: "
                    +p.Carticulo+" a merma. Disminuye valor en producto en: "+ merma.Cantidad * p.Npreccosto, merma.Cantidad * p.Npreccosto,0);
                db.Entry(p).State = EntityState.Modified;
                db.Entry(f).State = EntityState.Modified;
                db.Mermas.Add(merma);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Mensaje = true;
            ViewBag.ProductoId = new SelectList(db.Productos, "ID", "Carticulo", merma.ProductoId);
            return View(merma);
        }

        // GET: Mermas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Merma merma = await db.Mermas.Include(m => m.Producto).Include(mp => mp.Producto.CategoriaProducto).FirstAsync(m => m.Id == id);
           
            if (merma == null)
            {
                return HttpNotFound();
            }
            ViewBag.Nombre = merma.Producto.Carticulo;
            ViewBag.CantAnt = merma.Cantidad;
            ViewBag.ProductoId = new SelectList(db.Productos, "ID", "Carticulo", merma.ProductoId);
            return View(merma);
        }

        // POST: Mermas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProductoId,Cantidad,Precio")] Merma merma, string cantAnt)
        {
            if (ModelState.IsValid)
            {
                Finanzas f = db.Finanzas.First();
                int disp = Int32.Parse(cantAnt.Trim())- merma.Cantidad;
                Producto p = db.Productos.First(d => d.ID == merma.ProductoId);
                p.Ndisponibilidad += disp;
                f.ValorProductos += disp * p.Npreccosto;
                f.ValorCapital = f.ValorProductos + f.ValorEfectivo + f.Ganancias;
                if (disp >= 0)
                {
                    int cantidadMovida = Int32.Parse(cantAnt.Trim()) - merma.Cantidad;
                    Registrar_Movimiento(6, "Producto: " + p.Carticulo + " a Merma(EDITAR).", "Se quita " + cantidadMovida + " unidades del poducto: "
                                                                                      + p.Carticulo + " a merma. Aumenta valor en producto en: " + cantidadMovida * p.Npreccosto, cantidadMovida * p.Npreccosto, 0);

                }
                else
                {
                    int cantidadMovida =  merma.Cantidad - Int32.Parse(cantAnt.Trim());
                    Registrar_Movimiento(6, "Producto: " + p.Carticulo + " a Merma(EDITAR).", "Se pasa " + merma.Cantidad + " unidades del poducto: "
                                                                                      + p.Carticulo + " a merma. Disminuye valor en producto en: " + cantidadMovida * p.Npreccosto, cantidadMovida * p.Npreccosto, 0);

                }
                db.Entry(p).State = EntityState.Modified;
                await db.SaveChangesAsync();
                db.Entry(merma).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProductoId = new SelectList(db.Productos, "ID", "Carticulo", merma.ProductoId);
            return View(merma);
        }

        // GET: Mermas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Merma merma = await db.Mermas.Include(m => m.Producto).Include(mp => mp.Producto.CategoriaProducto).FirstAsync(m => m.Id == id);

          
            if (merma == null)
            {
                return HttpNotFound();
            }

            return View(merma);
        }

        // POST: Mermas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Merma merma = await db.Mermas.FindAsync(id);
            if (merma == null)
            {
                return HttpNotFound();
            }
            Finanzas f = db.Finanzas.First();
            Producto p = db.Productos.First(d => d.ID == merma.ProductoId);
            p.Ndisponibilidad += merma.Cantidad;
            f.ValorProductos += merma.Cantidad * p.Npreccosto;
            f.ValorCapital = f.ValorProductos + f.ValorEfectivo + f.Ganancias;

            Registrar_Movimiento(6, "Producto: " + p.Carticulo + " a Merma (Eliminar).", "Se quita " + merma.Cantidad + " unidades del poducto: "
                                                                              + p.Carticulo + " a merma. Aumenta valor en producto en: " + merma.Cantidad * p.Npreccosto, merma.Cantidad * p.Npreccosto, 0);

            db.Entry(p).State = EntityState.Modified;
            await db.SaveChangesAsync();
            db.Mermas.Remove(merma);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
                return View("VentaM", v);
            }

            return View(cliente);
        }


        public ActionResult VentaM(VentaViewModel model)
        {
           /* Seguridad s = new Seguridad();
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
                    FechaVenta = DateTime.Now
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



            ViewBag.Mensaje = false;
         return View(model);

            
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult VentaM([Bind(Include = "ID,FechaVenta,MensajeroId,VendedorId,ClienteId,GanaMensajero,Descuento,CostoTotal,GanaTotal,PrecioTotal,CantTotalProductos,Usuario,Estado,TipoVenta,DetallesVenta,Cliente,Vendedor,ListFotoVentas")] Venta venta)
        {

            if (ModelState.IsValid)
            {
                decimal salarioTrabajador = 0;
               /* foreach (var detalle in venta.DetallesVenta)
                {
                    Producto pr = db.Productos.First(p => p.ID == detalle.ProductoId);
                    salarioTrabajador += detalle.Cantidad * pr.Ngananctrab;
                }*///ASUMO Q en merma no hay ganancia trabajador
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
                    Merma prod = new Merma();
                    prod = db.Mermas.First(p => p.ProductoId == ventaDetalle.ProductoId);
                    prod.Cantidad -= ventaDetalle.Cantidad;
                    db.Entry(prod).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException es)
                    {
                       var a= es.EntityValidationErrors;
                    }
                    
                }

                //Realizar Movimientos en dependecia del estado de la venta*/



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
            public string Descripcion { get; set; }

            public string Usuario { get; set; }
            public int TipoVenta { get; set; } //1- minorista 2-mayorista
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
        public ActionResult VentaEdit(int? id)
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
                CostoTotal =0 ,// venta.CostoTotal,
                Descuento = venta.Descuento,
                FechaVenta = venta.FechaVenta,
                GanaMensajero = venta.GanaMensajero,
                GanaTotal = venta.GanaTotal,
                MensajeroId = venta.MensajeroId,
                PrecioTotal = venta.PrecioTotal,
                VendedorId = venta.VendedorId,
                TipoVenta = venta.TipoVenta,
                Descripcion = venta.Descripcion,
                DetallesVenta = new List<DetalleVentaSer>()
            };
            foreach (var detalle in venta.DetallesVenta)
            {
                Merma merma = db.Mermas.First(r => r.ProductoId==detalle.ProductoId);
                DetalleVentaSer deta = new DetalleVentaSer();
                deta.ID = detalle.ID;
                deta.Cantidad = detalle.Cantidad;
                deta.Costo = 0;//detalle.Producto.Npreccosto;
                deta.Descuento = 0;//detalle.Producto.Ndescuento;
                deta.Disponibilidad = merma.Cantidad;//detalle.Producto.Ndisponibilidad;
                deta.Garantia = 0;//detalle.Garantia;
                deta.Lote = detalle.Producto.Clote;
                deta.Precio = merma.Precio; //venta.TipoVenta == 1 ? detalle.Producto.Nprecminoris : detalle.Producto.Nprecmayoris;
                deta.ProductoId = detalle.ProductoId;
                deta.ProductoN = detalle.Producto.Carticulo;
                deta.VentaId = detalle.VentaId;
                ventaSer.DetallesVenta.Add(deta);
                detalle.Producto.Nprecmayoris = merma.Precio;
            }

            v.ObjJson = new JavaScriptSerializer().Serialize(ventaSer);
            return View(v);
        }

        // POST: Ventas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult VentaEdit([Bind(Include = "ID,FechaVenta,MensajeroId,VendedorId,ClienteId,GanaMensajero,Descuento,CostoTotal,GanaTotal,PrecioTotal,CantTotalProductos,Usuario,Estado,TipoVenta,DetallesVenta,Cliente,Vendedor,ListFotoVentas")] Venta venta)
        {

            if (ModelState.IsValid)
            {
                var ventaInDb = db.Ventas
                    .Include(d => d.DetallesVenta)
                    .SingleOrDefault(e => e.ID == venta.ID);
                if (ventaInDb == null) return Json(new { Success = 0, ex = "No se pudo editar la venta debido que no se encontro id anterior" }); ;
                if (venta.Estado == 1)
                {
                    EditarVentaMermaPedido(venta, ventaInDb);
                }
                else
                {
                    EditarVentaMermaFinalizada(venta, ventaInDb);
                }







                return Json(new { Success = 1, ID = venta.ID, ex = "" });
            }

            return Json(new { Success = 0, ex = "No se pudo editar la venta debido a un error" });

        }

        private void EditarVentaMermaPedido(Venta venta, Venta ventaInDb)
        {
            /*Restauro disponibilidad anterior*/
            List<VentaDetalle> listaOld = ventaInDb.DetallesVenta;
            foreach (var ventaDetalle in listaOld)
            {
                Merma prod = new Merma();
                prod = db.Mermas.First(p => p.ProductoId == ventaDetalle.ProductoId);
                prod.Cantidad += ventaDetalle.Cantidad;
                db.Entry(prod).State = EntityState.Modified;
                db.SaveChanges();
            }
            ///////////////////////////////////////////////////////////////
            decimal salarioTrabajador = 0;
           /* foreach (var detalle in venta.DetallesVenta)
            {
                Producto pr = db.Productos.First(p => p.ID == detalle.ProductoId);
                salarioTrabajador += detalle.Cantidad * pr.Ngananctrab;
            }*/
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
           // ventaInDb.FechaVenta = venta.FechaVenta;
            ventaInDb.GanaMensajero = venta.GanaMensajero;
            ventaInDb.GanaTotal = venta.GanaTotal;
            ventaInDb.MensajeroId = venta.MensajeroId;
            ventaInDb.PrecioTotal = venta.PrecioTotal;
            ventaInDb.VendedorId = venta.VendedorId;
            ventaInDb.TipoVenta = venta.TipoVenta;
            ventaInDb.GanaTrab = venta.GanaTrab;
            ventaInDb.Descripcion = venta.Descripcion;
            db.Entry(ventaInDb).State = EntityState.Modified;
            db.SaveChanges();
            //REBAJAR Disponibilidad
            List<VentaDetalle> lista = venta.DetallesVenta;
            foreach (var ventaDetalle in lista)
            {
                Merma prod = new Merma();
                prod = db.Mermas.First(p => p.ProductoId == ventaDetalle.ProductoId);
                prod.Cantidad -= ventaDetalle.Cantidad;
                db.Entry(prod).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        private void EditarVentaMermaFinalizada(Venta venta, Venta ventaInDb)
        {
            Finanzas f = db.Finanzas.First();
            //REAJUSTANDO VALORES VIEJOS UQE SERAN MOIFICADOS
            f.Ganancias -= ventaInDb.GanaTotal;
           /* Registrar_Movimiento(7, "Disminuye ganancia por edición de venta",
                "Se modifica una venta y disminuye la ganancia en: " + ventaInDb.GanaTotal + (" (CORRECCION)"),
                ventaInDb.GanaTotal, 0);*/

            /*foreach (var deta in ventaInDb.DetallesVenta)
            {
                Producto p = db.Productos.First(r => r.ID == deta.ProductoId);

                Registrar_Movimiento(1, "Aumenta valor en Productos por edición de venta",
                    "Se edita una venta aumenta valor en productos en: " + p.Npreccosto * deta.Cantidad + " (CORRECCION)",
                    p.Npreccosto * deta.Cantidad, 0);
                f.ValorProductos += p.Npreccosto * deta.Cantidad;

                Registrar_Movimiento(2, "Disminuye el efectivo por edición de venta",
                    "Se edita una venta y disminuye el efectivo en: " + p.Npreccosto * deta.Cantidad + " (CORRECCION)",
                    p.Npreccosto * deta.Cantidad, 0);
                f.ValorEfectivo -= p.Npreccosto * deta.Cantidad;
            }*/
            ////////////////////////////////////////////////////////////////////////////////////////////////

            /*Restauro disponibilidad anterior*/
            List<VentaDetalle> listaOld = ventaInDb.DetallesVenta;
            foreach (var ventaDetalle in listaOld)
            {
                Merma prod = new Merma();
                prod = db.Mermas.First(p => p.ProductoId == ventaDetalle.ProductoId);
                prod.Cantidad += ventaDetalle.Cantidad;
                db.Entry(prod).State = EntityState.Modified;
                db.SaveChanges();
            }
            ///////////////////////////////////////////////////////////////
            decimal salarioTrabajador = 0;
           /* foreach (var detalle in venta.DetallesVenta)
            {
                Producto pr = db.Productos.First(p => p.ID == detalle.ProductoId);
                salarioTrabajador += detalle.Cantidad * pr.Ngananctrab;
            }*/
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
            db.Entry(ventaInDb).State = EntityState.Modified;
            db.SaveChanges();
            //REBAJAR Disponibilidad
            List<VentaDetalle> lista = venta.DetallesVenta;
            foreach (var ventaDetalle in lista)
            {
                Merma prod = new Merma();
                prod = db.Mermas.First(p => p.ProductoId == ventaDetalle.ProductoId);
                prod.Cantidad -= ventaDetalle.Cantidad;
                db.Entry(prod).State = EntityState.Modified;
                db.SaveChanges();
            }
            //NUEVOS VALORES Movimiento
            f.Ganancias += venta.GanaTotal;
            Registrar_Movimiento(7, "Aumenta ganancia por edición de venta",
                "Se edita una venta y aumenta la ganancia en: " + venta.GanaTotal,
                venta.GanaTotal, 0);

            /*foreach (var deta in venta.DetallesVenta)
            {
                Producto p = db.Productos.First(r => r.ID == deta.ProductoId);

                Registrar_Movimiento(2, "Disminuye valor en Productos por edición de venta",
                    "Se edita una venta disminuyendo valor en productos en: " + p.Npreccosto * deta.Cantidad,
                    p.Npreccosto * deta.Cantidad, 0);
                f.ValorProductos -= p.Npreccosto * deta.Cantidad;

                Registrar_Movimiento(1, "Aumenta el efectivo por edición de venta",
                    "Se edita una venta y aumenta el efectivo en: " + p.Npreccosto * deta.Cantidad,
                    p.Npreccosto * deta.Cantidad, 0);
                f.ValorEfectivo += p.Npreccosto * deta.Cantidad;
            }*/


            f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
            db.Entry(f).State = EntityState.Modified;
            db.SaveChanges();
        }

        // GET: Ventas/Delete/5
        public async Task<ActionResult> VentaDelete(int? id)
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
            Venta venta = await db.Ventas.Include(v => v.Cliente).Include(c => c.Vendedor).Where(t => t.ID == id).FirstAsync();
            if (venta == null)
            {
                return HttpNotFound();
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

            foreach (var detalle in venta.DetallesVenta)
            {
                Merma merma = db.Mermas.First(r => r.ProductoId == detalle.ProductoId);
              
                detalle.Producto.Nprecmayoris = merma.Precio;
            }




            return View(d);

        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("VentaDelete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VentaDeleteConfirmed(int id)
        {

            Venta venta = await db.Ventas.FindAsync(id);

            if (venta != null && venta.Estado != 4)
            {
                List<VentaDetalle> list = db.VentaDetalles.Where(c => c.VentaId == id).Include(p => p.Producto).ToList();
                Finanzas f = db.Finanzas.First();
                if (venta.Estado != 1)
                {
                    f.Ganancias -= venta.GanaTotal;
                    Registrar_Movimiento(7, "Disminuye la ganancia por eliminación de venta(Merma)",
                       "Se elimina una venta(Merma) y disminuye la ganancia en: " + venta.GanaTotal,
                       venta.GanaTotal, 0);
                }
                    
                foreach (var deta in list)
                {
                    Merma p = db.Mermas.First(m=>m.ProductoId==deta.ProductoId);
                    p.Cantidad += deta.Cantidad;
                    db.Entry(p).State = EntityState.Modified;
                    db.SaveChanges();

                   /* if (venta.Estado != 1)
                    {
                        Registrar_Movimiento(1, "Aumenta valor en Productos por eliminación de venta",
                            "Se elimina una venta y aumenta disponibilidad del producto: " + p.Carticulo +
                            ", aumentando valor en productos en: " + p.Npreccosto * deta.Cantidad,
                            p.Npreccosto * deta.Cantidad, 0);
                        f.ValorProductos += p.Npreccosto * deta.Cantidad;

                        Registrar_Movimiento(2, "Disminuye el efectivo por eliminación de venta",
                            "Se elimina una venta y disminuye el efectivo en: " + p.Npreccosto * deta.Cantidad,
                            p.Npreccosto * deta.Cantidad, 0);
                        f.ValorEfectivo -= p.Npreccosto * deta.Cantidad;
                    }*/


                }
                f.ValorCapital = f.ValorProductos + f.ValorEfectivo + f.Ganancias;
                db.Entry(f).State = EntityState.Modified;
                db.SaveChanges();

                db.Ventas.Remove(venta);
                await db.SaveChangesAsync();

                return RedirectToAction("Index","Ventas");
            }

            TempData["Mensaje"] = true;
            return RedirectToAction("VentaDelete", id);
        }

        // GET: Productoes/Delete/5
        public async Task<ActionResult> VentaCambiarEstado(int? id)
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
        public ActionResult VentaCambiarEstado([Bind(Include = "ID,FechaVenta,MensajeroId,VendedorId,ClienteId,GanaMensajero,Descuento,CostoTotal,GanaTotal,PrecioTotal,CantTotalProductos,Usuario,Estado,TipoVenta,DetallesVenta,Cliente,Vendedor,ListFotoVentas")] Venta venta)
        {


            int estadonuevo = venta.Estado;
            venta = db.Ventas.Include(dd => dd.DetallesVenta).Include(v => v.Cliente).Include(c => c.Vendedor).First(t => t.ID == venta.ID);
            Finanzas f = db.Finanzas.First();
            if (venta.Estado == 2 && estadonuevo == 1)
            {
                f.Ganancias -= venta.GanaTotal;
                Registrar_Movimiento(7, "Disminuye ganancia por cambio de estado de venta(MERMA)",
                    "Se cambia estado a una venta y disminuye la ganacia en: " + venta.GanaTotal,
                    venta.GanaTotal, 0);
                /*  foreach (var deta in venta.DetallesVenta)
                 {
                     //Producto p = db.Productos.First(r => r.ID == deta.ProductoId);
                     //No se hace movimiento por ser merma
                    Registrar_Movimiento(1, "Aumenta valor en Productos por cambio de estado en venta",
                         "Se cambia estado de venta aumentando valor en productos en: " + p.Npreccosto * deta.Cantidad,
                         p.Npreccosto * deta.Cantidad, 0);
                     f.ValorProductos += p.Npreccosto * deta.Cantidad;

                     Registrar_Movimiento(2, "Disminuye el efectivo por cambio de estado en venta",
                         "Se cambia estado de una venta y disminuye el efectivo en: " + p.Npreccosto * deta.Cantidad,
                         p.Npreccosto * deta.Cantidad, 0);
                     f.ValorEfectivo -= p.Npreccosto * deta.Cantidad;
            }*/




            }
            else
            {
                if (venta.Estado == 1 && estadonuevo == 2)
                {
                    f.Ganancias += venta.GanaTotal;
                    Registrar_Movimiento(7, "Aumenta ganancia por finalizacion de venta(MERMA)",
                        "Se cambia estado de una venta a finalizada y aumenta la ganancia en: " + venta.GanaTotal,
                        venta.GanaTotal, 0);

                   /* foreach (var deta in venta.DetallesVenta)
                    {
                        Producto p = db.Productos.First(r => r.ID == deta.ProductoId);

                        Registrar_Movimiento(2, "Disminuye valor en Productos por finalización de venta",
                            "Se finaliza una venta disminuyendo valor en productos en: " + p.Npreccosto * deta.Cantidad,
                            p.Npreccosto * deta.Cantidad, 0);
                        f.ValorProductos -= p.Npreccosto * deta.Cantidad;

                        Registrar_Movimiento(1, "Aumenta el efectivo por finalización de venta",
                            "Se finaliza una venta y aumenta el efectivo en: " + p.Npreccosto * deta.Cantidad,
                            p.Npreccosto * deta.Cantidad, 0);
                        f.ValorEfectivo += p.Npreccosto * deta.Cantidad;
                    }*/

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

            return RedirectToAction("Index","Ventas");
        }


        private void Registrar_Movimiento(int tipo, string nombre, string descripcion, decimal monto, int idProd)
        {
            if (monto < 0) monto = monto * -1;
            Movimiento m = new Movimiento()
            {
                TipoMovimiento = tipo,
                Descripcion = descripcion,
                FechaMovimiento = DateTime.Now,
                Monto = monto,
                Nombre = nombre,
                IdProducto = idProd,
                Usuario = User.Identity.GetUserName()
            };
            db.Movimientos.Add(m);
            db.SaveChanges();
        }
    }
}
