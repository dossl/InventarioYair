using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventarioOtro.Models;
using InventarioOtro.ViewModels;
using Microsoft.AspNet.Identity;
using PagedList;

namespace InventarioOtro.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CierresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cierres
        public async Task<ActionResult> Index(string searchString1, string searchString, string categoriaId, int? page, int? paso)
        {
            /*Seguridad s = new Seguridad();
            if (!s.ValidarSeguridad())
            {
                return View("Error");
            }*/

            /*if ("3".Equals(categoriaId) && !string.IsNullOrEmpty(searchString1))
            {
                searchString = searchString1;
            }
            else
            {
                if ("3".Equals(categoriaId) && string.IsNullOrEmpty(searchString1))
                {
                    searchString = "";
                }
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
            IndexCierreViewModel viewModel = new IndexCierreViewModel();
            var lista = new List<Cierre>();
            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.Search = searchString;
                if (!String.IsNullOrWhiteSpace(categoriaId))
                {
                    ViewBag.CategoriaId = categoriaId;

                    if ("1".Equals(categoriaId))
                    {
                        decimal valor = decimal.Parse(searchString);
                        //lista = db.Cierres.Where(p => p.Monto == valor).ToList();
                    }
                    else
                    {
                        if ("2".Equals(categoriaId))
                        {
                           // lista = db.PagoSalarios.Where(p => p.Trabajador.ToLower().Contains(searchString.ToLower())).ToList();
                        }
                        else
                        {

                            if ("3".Equals(categoriaId))
                            {
                                DateTime valor = DateTime.Parse(searchString);
                                lista = db.Cierres.Where(p => p.Fecha.Equals(valor)).ToList();
                            }

                        }
                    }


                }
                else
                {
                    ViewBag.CategoriaId = "";
                    //lista = db.Cierres.Where(p => p.Trabajador.ToLower().Contains(searchString.ToLower())).ToList();
                    lista = db.Cierres.ToList();
                }


            }
            else
            {
                ViewBag.Search = "";
                if (!String.IsNullOrEmpty(categoriaId))
                {
                    ViewBag.CategoriaId = categoriaId;
                    lista = db.Cierres.ToList();
                }
                else
                {
                    ViewBag.CategoriaId = "";
                    lista = db.Cierres.ToList();
                }


            }
            if (lista.Count <= ViewBag.Paso)
            {
                ViewBag.Page = 1;
            }
            viewModel.ListCierres = lista.OrderBy(c => c.Fecha).ToPagedList((int)ViewBag.Page, (int)ViewBag.Paso);


            return View(viewModel);
        }

        // GET: Cierres/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cierre cierre = await db.Cierres.FindAsync(id);
            if (cierre == null)
            {
                return HttpNotFound();
            }
            return View(cierre);
        }

        // GET: Cierres/Create
        public ActionResult Create()
        {
            Cierre cierre= new Cierre();
            cierre.Fecha=DateTime.Today;
            //Ventas Finalizada
            List<Venta> listaVentas = db.Ventas.Where(v => v.Estado == 2).Include(r => r.DetallesVenta).ToList();
            Cierre ultimoCierre = new Cierre();
            List<Devolucion> devoluciones = new List<Devolucion>();
            Finanzas f = db.Finanzas.First();
            if (!db.Cierres.Any())
            {
                devoluciones = db.Devoluciones.ToList();
            }
            else
            {
               
                ultimoCierre = db.Cierres.OrderByDescending(c=>c.Fecha).First(); 
                devoluciones = db.Devoluciones.Where(d => d.Fecha > ultimoCierre.Fecha).ToList();
            }
            foreach (var devolucion in devoluciones)
            {
                cierre.CantArticulosDev += devolucion.Cantidad;
            }
            cierre.GananciasPeriodo = f.Ganancias;
            foreach (Venta venta in listaVentas)
            {
                
                cierre.CantidadArticulos += venta.CantTotalProductos;
                foreach (VentaDetalle ventaDetalle in venta.DetallesVenta)
                {
                    if (venta.TipoVenta == 3)
                    {
                        Merma mer = db.Mermas.First(s => s.ProductoId == ventaDetalle.ProductoId);
                        cierre.ValorTotalVentas += ventaDetalle.Cantidad * mer.Precio;
                        
                    }
                    else
                    {
                        Producto pr = db.Productos.First(s => s.ID == ventaDetalle.ProductoId);
                        if (venta.TipoVenta == 1)
                        {
                            cierre.ValorTotalVentas += ventaDetalle.Cantidad * pr.Nprecminoris;
                        }
                        else
                        {
                            cierre.ValorTotalVentas += ventaDetalle.Cantidad * pr.Nprecmayoris;
                        }
                    }
                    

                }
                cierre.ValorTotalVentas -= venta.Descuento;

            }
            cierre.GananciaExtraida = cierre.GananciasPeriodo;
            return View(cierre);
        }

        // POST: Cierres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,GananciasPeriodo,CantidadArticulos,ValorTotalVentas,CantArticulosDev,Fecha,GananciaExtraida")] Cierre cierre)
        {
            
            if (ModelState.IsValid)
            {
                Finanzas f = db.Finanzas.First();
                decimal aEfectivo = cierre.GananciasPeriodo - cierre.GananciaExtraida;
                f.ValorEfectivo += aEfectivo;
                Registrar_Movimiento(4, "Aumenta efectivo por cierre del "+cierre.Fecha.ToString("dd/MM/yyyy"),
                        "Se realiza cierre el dia "+ cierre.Fecha.ToString("dd/MM/yyyy") + " y aumenta el efectivo en: " + aEfectivo,
                        aEfectivo, 0);
                f.Ganancias = 0;
                Registrar_Movimiento(7, "Ganancia a 0 por cierre del día: "+ cierre.Fecha.ToString("dd/MM/yyyy"),
                       "Se realiza cierre el dia " + cierre.Fecha.ToString("dd/MM/yyyy") + " y la ganancia se coloca a 0",
                        0, 0);
                f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
                List<Venta> listaVentas = db.Ventas.Where(v => v.Estado == 2).Include(r => r.DetallesVenta).ToList();
                foreach (Venta venta in listaVentas)
                {
                    venta.Estado = 4;
                    db.Entry(venta).State = EntityState.Modified;
                    db.SaveChanges();
                }
                db.Entry(f).State = EntityState.Modified;
                db.SaveChanges();
                db.Cierres.Add(cierre);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cierre);
        }

        // GET: Cierres/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cierre cierre = await db.Cierres.FindAsync(id);
            if (cierre == null)
            {
                return HttpNotFound();
            }
            return View(cierre);
        }

        // POST: Cierres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,GananciasPeriodo,CantidadArticulos,ValorTotalVentas,CantArticulosDev,Fecha")] Cierre cierre)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cierre).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cierre);
        }

        // GET: Cierres/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cierre cierre = await db.Cierres.FindAsync(id);
            if (cierre == null)
            {
                return HttpNotFound();
            }
            return View(cierre);
        }

        // POST: Cierres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Cierre cierre = await db.Cierres.FindAsync(id);
            db.Cierres.Remove(cierre);
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
