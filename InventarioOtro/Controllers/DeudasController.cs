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
    public class DeudasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Deudas
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
            IndexDeudasViewModel viewModel = new IndexDeudasViewModel();
            var lista = new List<Deuda>();
            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.Search = searchString;
                if (!String.IsNullOrWhiteSpace(categoriaId))
                {
                    ViewBag.CategoriaId = categoriaId;

                    if ("1".Equals(categoriaId))
                    {
                        decimal valor = decimal.Parse(searchString);
                        lista = db.Deudas.Include(v=>v.Venta)
                                         .Include(c=>c.Venta.Cliente)
                                         .Where(p => p.ValorInicial == valor).ToList();
                    }
                    else
                    {
                        if ("2".Equals(categoriaId))
                        {
                            lista = db.Deudas.Include(v => v.Venta)
                                         .Include(c => c.Venta.Cliente)
                                         .Where(p => p.Venta.Cliente.Cfirstname.ToLower().Contains(searchString.ToLower()) || p.Venta.Cliente.Clastname.ToLower().Contains(searchString.ToLower())).ToList();
                        }
                        else
                        {

                            if ("3".Equals(categoriaId))
                            {
                                DateTime valor = DateTime.Parse(searchString);
                                lista = db.Deudas.Include(v => v.Venta)
                                         .Include(c => c.Venta.Cliente)
                                         .Where(p => p.FechaCreacion.Equals(valor)).ToList();
                            }

                        }
                    }


                }
                else
                {
                    ViewBag.CategoriaId = "";
                    lista = db.Deudas.Include(v => v.Venta)
                                         .Include(c => c.Venta.Cliente).ToList();
                }


            }
            else
            {
                ViewBag.Search = "";
                if (!String.IsNullOrEmpty(categoriaId))
                {
                    ViewBag.CategoriaId = categoriaId;
                    lista = db.Deudas.Include(v => v.Venta)
                                         .Include(c => c.Venta.Cliente).ToList();
                }
                else
                {
                    ViewBag.CategoriaId = "";
                    lista = db.Deudas.Include(v => v.Venta)
                                          .Include(c => c.Venta.Cliente).ToList();
                }


            }
            if (lista.Count <= ViewBag.Paso)
            {
                ViewBag.Page = 1;
            }
            viewModel.ListDeudas = lista.OrderByDescending(c => c.FechaCreacion).ToPagedList((int)ViewBag.Page, (int)ViewBag.Paso);


            return View(viewModel);
        }

        // GET: Deudas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deuda deuda =   await db.Deudas.Include(v => v.Venta)
                                         .Include(c => c.Venta.Cliente)
                                         .FirstAsync(d => d.Id==id);
            if (deuda == null)
            {
                return HttpNotFound();
            }
            return View(deuda);
        }

        // GET: Deudas/Create
        public ActionResult Create()
        {
            ViewBag.VentaId = new SelectList(db.Ventas, "ID", "MensajeroId");
            return View();
        }

        // POST: Deudas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,VentaId,ValorInicial,ValorActual,CostoProductoAPagar,GanaTrabAPagar,FechaCreacion,UltimoPago")] Deuda deuda)
        {
            if (ModelState.IsValid)
            {
                db.Deudas.Add(deuda);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.VentaId = new SelectList(db.Ventas, "ID", "MensajeroId", deuda.VentaId);
            return View(deuda);
        }

        // GET: Deudas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deuda deuda = await db.Deudas.Include(v => v.Venta)
                                         .Include(c => c.Venta.Cliente)
                                         .FirstAsync(d => d.Id == id);
            if (deuda == null)
            {
                return HttpNotFound();
            }
            ViewBag.VentaId = new SelectList(db.Ventas, "ID", "MensajeroId", deuda.VentaId);
            return View(deuda);
        }

        // POST: Deudas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,VentaId,ValorInicial,ValorActual,CostoProductoAPagar,GanaTrabAPagar,FechaCreacion,UltimoPago")] Deuda deuda, string valorPagado)
        {
            if (ModelState.IsValid)
            {
                deuda = db.Deudas.Find(deuda.Id);
                decimal pagado = decimal.Parse(valorPagado);
                deuda.UltimoPago=DateTime.Today;
                deuda.ValorActual -= pagado;
                decimal resto = 0;
                Finanzas f = db.Finanzas.First();
                if (deuda.CostoProductoAPagar > 0)
                {
                    if (pagado >= deuda.CostoProductoAPagar)
                    {
                        resto = pagado - deuda.CostoProductoAPagar;
                        Registrar_Movimiento(4, "Disminuye valor en Productos por actualizacion de deuda",
                            "Se actualiza una  deuda disminuyendo valor en productos en: " + deuda.CostoProductoAPagar,
                            deuda.CostoProductoAPagar, 0);
                        f.ValorProductos -= deuda.CostoProductoAPagar;

                        Registrar_Movimiento(4, "Aumenta el efectivo por actualizacion de deuda",
                            "Se actualiza deuda y aumenta el efectivo en: " + deuda.CostoProductoAPagar,
                            deuda.CostoProductoAPagar, 0);
                        f.ValorEfectivo += deuda.CostoProductoAPagar;
                        deuda.CostoProductoAPagar = 0;
                        if (resto >= deuda.GanaTrabAPagar)
                        {
                            resto -= deuda.GanaTrabAPagar;
                            Registrar_Movimiento(4, "Aumenta el efectivo actualizacion de deuda",
                                "Se actualiza una deuda y aumenta el efectivo en: " + deuda.GanaTrabAPagar,
                                deuda.GanaTrabAPagar, 0);
                            f.ValorEfectivo += deuda.GanaTrabAPagar;
                            deuda.GanaTrabAPagar = 0;
                            if (resto > 0)
                            {
                                f.Ganancias += resto;
                                Registrar_Movimiento(7, "Aumenta ganancia por actualizacion de deuda",
                                    "Se actualiza una venta con deuda a finalizada y aumenta la ganancia en: " + (resto),
                                    resto, 0);
                            }

                        }
                        else
                        {
                            deuda.GanaTrabAPagar -= resto;
                            Registrar_Movimiento(4, "Aumenta el efectivo actualizacion de deuda",
                                "Se actualiza una deuda y aumenta el efectivo en: " + resto,
                                resto, 0);
                            f.ValorEfectivo += resto;
                        }
                    }
                    else
                    {
                        Registrar_Movimiento(4, "Disminuye valor en Productos por actualizacion de deuda",
                            "Se actualiza una  deuda disminuyendo valor en productos en: " + pagado,
                            pagado, 0);
                        f.ValorProductos -= pagado;

                        Registrar_Movimiento(4, "Aumenta el efectivo por actualizacion de deuda",
                            "Se actualiza deuda y aumenta el efectivo en: " + pagado,
                            pagado, 0);
                        f.ValorEfectivo += pagado;
                        deuda.CostoProductoAPagar -= pagado;
                    }
                }
                else
                {
                    if (deuda.GanaTrabAPagar > 0)
                    {
                        if (pagado >= deuda.GanaTrabAPagar)
                        {
                            resto = pagado- deuda.GanaTrabAPagar;
                            Registrar_Movimiento(4, "Aumenta el efectivo actualizacion de deuda",
                                "Se actualiza una deuda y aumenta el efectivo en: " + deuda.GanaTrabAPagar,
                                deuda.GanaTrabAPagar, 0);
                            f.ValorEfectivo += deuda.GanaTrabAPagar;
                            deuda.GanaTrabAPagar=0;
                            if (resto > 0)
                            {
                                f.Ganancias += resto;
                                Registrar_Movimiento(4, "Aumenta ganancia por actualizacion de deuda",
                                    "Se actualiza una venta con deuda a finalizada y aumenta la ganancia en: " + (resto),
                                    resto, 0);
                            }

                        }
                        else
                        {
                            deuda.GanaTrabAPagar -= pagado;
                            Registrar_Movimiento(4, "Aumenta el efectivo actualizacion de deuda",
                                "Se actualiza una deuda y aumenta el efectivo en: " + pagado,
                                pagado, 0);
                            f.ValorEfectivo += pagado;
                        }
                    }
                    else
                    {
                        if (pagado > 0)
                        {

                            Registrar_Movimiento(4, "Aumenta el efectivo actualizacion de deuda",
                               "Se actualiza una deuda y aumenta el efectivo en: " + pagado,
                               pagado, 0);
                            f.Ganancias += pagado;
                        }
                    }
                }

                f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
                db.Entry(f).State = EntityState.Modified;
                db.Entry(deuda).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            
            return View(deuda);
        }

        // GET: Deudas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deuda deuda = await db.Deudas.Include(v => v.Venta)
                                        .Include(c => c.Venta.Cliente)
                                        .FirstAsync(d => d.Id == id);
            if (deuda == null)
            {
                return HttpNotFound();
            }
            return View(deuda);
        }

        // POST: Deudas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Deuda deuda = await db.Deudas.Include(v => v.Venta)
                                        .Include(c => c.Venta.Cliente)
                                        .FirstAsync(d => d.Id == id);
            Finanzas f = db.Finanzas.First();
            Venta venta = deuda.Venta;
            venta.Estado = 1;
            decimal precioTotal = deuda.Venta.PrecioTotal;
            decimal valorDeuda=deuda.ValorActual;
            decimal revertir = precioTotal - valorDeuda;

            if (revertir>=venta.CostoTotal)
            {
                Registrar_Movimiento(4, "Aumenta valor en Productos por eliminación deuda",
                           "Se elimina una deuda aumentando valor en productos en: " + venta.CostoTotal+". La venta pasa a pedido",
                           venta.CostoTotal, 0);
                f.ValorProductos += venta.CostoTotal;

                Registrar_Movimiento(4, "Disminuye el efectivo por eliminación de deuda",
                    "Se elimina una deuda y disminuye el efectivo en: " + venta.CostoTotal + ". La venta pasa a pedido",
                    venta.CostoTotal, 0);
                f.ValorEfectivo -= venta.CostoTotal;

                revertir -= venta.CostoTotal;
                if (revertir >= venta.GanaTrab)
                {
                    f.ValorEfectivo -= venta.GanaTrab;
                    Registrar_Movimiento(4, "Disminuye el efectivo de salario de trabajador(eliminación deuda)",
                        "Se elimina deuda y disminuye el efectivo destinado a salario en: " + venta.GanaTrab,
                        venta.GanaTrab, 0);
                    revertir -= venta.GanaTrab;

                    if (revertir > 0)
                    {
                        f.Ganancias -= revertir;
                        Registrar_Movimiento(4, "Disminuye ganancia por eliminación de deuda",
                            "Se cambia estado de una venta a pedido, se elimina deuda y disminuye la ganancia en: " +
                            revertir,
                            revertir, 0);
                    }
                }
                else
                {
                    f.ValorEfectivo -= revertir;
                    Registrar_Movimiento(4, "Disminuye el efectivo (eliminación deuda)",
                        "Se elimina deuda y disminuye el efectivo en: " + revertir,
                        revertir, 0);
                }


            }
            else
            {
                Registrar_Movimiento(4, "Aumenta valor en Productos por eliminación deuda",
                           "Se elimina una deuda aumentando valor en productos en: " + revertir + ". La venta pasa a pedido",
                           revertir, 0);
                f.ValorProductos += revertir;

                Registrar_Movimiento(4, "Disminuye el efectivo por eliminación de deuda",
                    "Se elimina una deuda y disminuye el efectivo en: " + revertir + ". La venta pasa a pedido",
                    revertir, 0);
                f.ValorEfectivo -= revertir;
            }
            f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
            db.Entry(f).State = EntityState.Modified;
            db.Entry(venta).State = EntityState.Modified;
            db.Deudas.Remove(deuda);
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
