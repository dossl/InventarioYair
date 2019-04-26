using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Web;
using System.Web.Mvc;
using InventarioOtro.Models;
using InventarioOtro.ViewModels;
using Microsoft.AspNet.Identity;
using PagedList;

namespace InventarioOtro.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PagoSalariosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PagoSalarios
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
            IndexPagoViewModel viewModel = new IndexPagoViewModel();
            var lista = new List<PagoSalario>();
            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.Search = searchString;
                if (!String.IsNullOrWhiteSpace(categoriaId))
                {
                    ViewBag.CategoriaId = categoriaId;

                    if ("1".Equals(categoriaId))
                    {
                        decimal valor = decimal.Parse(searchString);
                        lista = db.PagoSalarios.Where(p => p.Monto== valor).ToList();
                    }
                    else
                    {
                        if ("2".Equals(categoriaId))
                        {
                            lista = db.PagoSalarios.Where(p => p.Trabajador.ToLower().Contains(searchString.ToLower())).ToList();
                        }
                        else
                        {

                            if ("3".Equals(categoriaId))
                            {
                                DateTime valor = DateTime.Parse(searchString);
                                lista = db.PagoSalarios.Where(p => p.FechaPago.Equals(valor)).ToList();
                            }

                        }
                    }


                }
                else
                {
                    ViewBag.CategoriaId = "";
                    lista = db.PagoSalarios.Where(p => p.Trabajador.ToLower().Contains(searchString.ToLower())).ToList();
                }


            }
            else
            {
                ViewBag.Search = "";
                if (!String.IsNullOrEmpty(categoriaId))
                {
                    ViewBag.CategoriaId = categoriaId;
                    lista = db.PagoSalarios.ToList();
                }
                else
                {
                    ViewBag.CategoriaId = "";
                    lista = db.PagoSalarios.ToList();
                }


            }
            if (lista.Count <= ViewBag.Paso)
            {
                ViewBag.Page = 1;
            }
            viewModel.ListPagos = lista.OrderBy(c => c.FechaPago).ToPagedList((int)ViewBag.Page, (int)ViewBag.Paso);


            return View(viewModel);
        }

        // GET: PagoSalarios/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PagoSalario pagoSalario = await db.PagoSalarios.FindAsync(id);
            if (pagoSalario == null)
            {
                return HttpNotFound();
            }
            return View(pagoSalario);
        }

        // GET: PagoSalarios/Create
        public ActionResult Create()
        {
            List<PagoSalario> ultimo =new List<PagoSalario>();
            if (db.PagoSalarios.Any())
            {
                
                ultimo = db.PagoSalarios.ToList();
                int c = ultimo.Count;
                ViewBag.Inicio = ultimo[c-1].FechaPago.ToString().Substring(0, 10);
                ViewBag.Readonly = true;
            }
            else
            {
                ViewBag.Inicio = DateTime.Today.ToString().Substring(0, 10);
                ViewBag.Readonly = false;
            }
            
            ViewBag.Fin = DateTime.Today.ToString().Substring(0, 10);

            return View();
        }

        // POST: PagoSalarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Trabajador,Monto,Estado,FechaPago,DiasDescontado")] PagoSalario pagoSalario, string inicio, string fin)
        {
            if (ModelState.IsValid)
            {
                DateTime Inicio = DateTime.Parse(inicio);
                DateTime Fin= DateTime.Parse(fin);
                pagoSalario.Monto -= pagoSalario.DiasDescontado*3;
                pagoSalario.CantidadLaborada = (Fin - Inicio).Days;
                
                Finanzas f = db.Finanzas.First();
                Registrar_Movimiento(4, "Salario", "Salario pagado al trabajador: " + pagoSalario.Trabajador+" y disminuye el efectivo en "+ pagoSalario.Monto,
                    pagoSalario.Monto, 0);
                f.ValorEfectivo -= pagoSalario.Monto;
                f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
                db.Entry(f).State = EntityState.Modified;
                db.SaveChanges();
                pagoSalario.FechaPago=DateTime.Now;
                
                db.PagoSalarios.Add(pagoSalario);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(pagoSalario);
        }

        // GET: PagoSalarios/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PagoSalario pagoSalario = await db.PagoSalarios.FindAsync(id);
            if (pagoSalario == null)
            {
                return HttpNotFound();
            }
            return View(pagoSalario);
        }

        // POST: PagoSalarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Trabajador,Monto,Estado,FechaPago")] PagoSalario pagoSalario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pagoSalario).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pagoSalario);
        }

        // GET: PagoSalarios/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PagoSalario pagoSalario = await db.PagoSalarios.FindAsync(id);
            if (pagoSalario == null)
            {
                return HttpNotFound();
            }
            return View(pagoSalario);
        }

        // POST: PagoSalarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {

            PagoSalario pagoSalario = await db.PagoSalarios.FindAsync(id);
            Finanzas f = db.Finanzas.First();
            Registrar_Movimiento(4, "Salario (Eliminado)", "Se elimina un pago de Salario pagado al trabajador: " + pagoSalario.Trabajador+" y aumenta el efectivo en "+pagoSalario.Monto,
                pagoSalario.Monto, 0);
            f.ValorEfectivo += pagoSalario.Monto;
            f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
            db.Entry(f).State = EntityState.Modified;

            db.PagoSalarios.Remove(pagoSalario);
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

        public class Rango
        {
            public DateTime Inicio { get; set; }
            public DateTime Fin { get; set; }
            public int DiasDescontados { get; set; }
           
        }
        [HttpPost]
        public JsonResult Calcular([Bind(Include = "Inicio,Fin,DiasDescontados")] Rango rango)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    if (rango.Fin < rango.Inicio)
                    {
                        return Json(new { Success = 0, ex = "La fecha de fin tiene que ser mayor que la fecha de inicio" });
                    }
                    int cantLaborado =( rango.Fin - rango.Inicio).Days;
                    decimal salario = (cantLaborado-rango.DiasDescontados) * 3;

                    List<Venta> ventasRango = db.Ventas.Where(v => v.FechaVenta > rango.Inicio 
                                                                && v.FechaVenta<=rango.Fin 
                                                                && v.Estado > 1).ToList();
                    List<Devolucion> devolucionesRango = db.Devoluciones.Where(v => v.Fecha > rango.Inicio
                                                                                    && v.Fecha <= rango.Fin).ToList();
                    foreach (var venta in ventasRango)
                    {
                        if(venta.TipoVenta!=3)
                            salario += venta.GanaTrab;
                      
                    }

                    foreach (var devolucion in devolucionesRango)
                    {
                        salario -= devolucion.DescuentoSalario;
                    }
                  

                    return Json(new { Success = 1, ID = salario, ex = "" });
                }
                catch (Exception e)
                {
                    return Json(new { Success = 0, ex = "No se pudo realizar la el calculo debido a un error" });
                }
            }

            return Json(new { Success = 0, ex = "No se pudo realizar la calculo debido a un error" });
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
