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
    public class MovimientoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Movimientoes
        public async Task<ActionResult> Index(string searchString1, string searchString, string categoriaId, int? page, int? paso, string sortOrder)
        {
            /*Seguridad s = new Seguridad();
            if (!s.ValidarSeguridad())
            {
                return View("Error");
            }*/

            if ("3".Equals(categoriaId)&&!string.IsNullOrEmpty(searchString1))
            {
                searchString = searchString1;
            }
            else
            {
                if ("3".Equals(categoriaId) && string.IsNullOrEmpty(searchString1))
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
               IndexMovimientoViewModel viewModel = new IndexMovimientoViewModel();
                var lista = new List<Movimiento>();
                if (!String.IsNullOrEmpty(searchString))
                {
                    ViewBag.Search = searchString;
                    if (!String.IsNullOrWhiteSpace(categoriaId))
                    {
                        ViewBag.CategoriaId = categoriaId;

                        if ("1".Equals(categoriaId))
                        {
                            lista =  db.Movimientos.Where(p => p.Nombre.ToLower().Contains(searchString.ToLower())).ToList();
                        }
                        else
                        {
                            if ("2".Equals(categoriaId))
                            {
                                lista = db.Movimientos.Where(p => p.Descripcion.ToLower().Contains(searchString.ToLower())).ToList();
                        }
                            else
                            {

                                if ("3".Equals(categoriaId))
                                {
                                    int valor = int.Parse(searchString);
                                    lista = db.Movimientos.Where(p => p.TipoMovimiento.Equals(valor)).ToList();
                                }
                               
                            }
                        }


                    }
                    else
                    {
                        ViewBag.CategoriaId = "";
                        lista = db.Movimientos.Where(p => p.Nombre.ToLower().Contains(searchString.ToLower())).ToList();
                    }


                }
                else
                {
                    ViewBag.Search = "";
                    if (!String.IsNullOrEmpty(categoriaId))
                    {
                        ViewBag.CategoriaId = categoriaId;
                        lista = db.Movimientos.ToList();
                    }
                    else
                    {
                        ViewBag.CategoriaId = "";
                        lista = db.Movimientos.ToList();
                    }


                }
                if (lista.Count <= ViewBag.Paso)
                {
                    ViewBag.Page = 1;
                }
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.NameSortParm = sortOrder == "nombre" ? "nombre_desc" : "nombre";
            ViewBag.MontoSortParm = sortOrder == "monto" ? "monto_desc" : "monto";
            ViewBag.TipSortParm = sortOrder == "tip" ? "tip_desc" : "tip";
            ViewBag.UserSortParm = sortOrder == "user" ? "user_desc" : "user";
           

            switch (sortOrder)
            {
                case "nombre_desc":
                    lista = lista.OrderByDescending(l => l.Nombre).ToList();
                    break;
                case "nombre":
                    lista = lista.OrderBy(l => l.Nombre).ToList(); 
                    break;
                
                case "date_desc":
                    lista = lista.OrderBy(l => l.FechaMovimiento).ToList();
                    break;
                case "monto":
                    lista = lista.OrderBy(l => l.Monto).ToList();
                    break;
                case "monto_desc":
                    lista = lista.OrderByDescending(l => l.Monto).ToList();
                    break;
                case "tip":
                    lista = lista.OrderBy(l => l.TipoMovimiento).ToList();
                    break;
                case "tip_desc":
                    lista = lista.OrderByDescending(l => l.TipoMovimiento).ToList();
                    break;
                case "user":
                    lista = lista.OrderBy(l => l.Usuario).ToList();
                    break;
                case "user_desc":
                    lista = lista.OrderByDescending(l => l.Usuario).ToList();
                    break;
               
                default:
                    lista = lista.OrderByDescending(l => l.FechaMovimiento).ToList();
                    break;
            }

            viewModel.ListMovimiento = lista.ToPagedList((int)ViewBag.Page, (int)ViewBag.Paso);
                
            
            return View(viewModel);
        }

        // GET: Movimientoes/Details/5
        public async Task<ActionResult> Details(int? id)
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
            Movimiento movimiento = await db.Movimientos.FindAsync(id);
            if (movimiento == null)
            {
                return HttpNotFound();
            }
            return View(movimiento);
        }

        // GET: Movimientoes/Create
        public ActionResult Create()
        {
            /*Seguridad s = new Seguridad();
            if (!s.ValidarSeguridad())
            {
                return View("Error");
            }*/
            return View();
        }

        // POST: Movimientoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Nombre,Monto,TipoMovimiento,Usuario,FechaMovimiento,Descripcion")] Movimiento movimiento)
        {
            Finanzas f = db.Finanzas.First();
            bool seguir = true;

            if (movimiento.TipoMovimiento == 2 || movimiento.TipoMovimiento == 3)
            {
                seguir = f.ValorEfectivo > movimiento.Monto;
                ViewBag.CreditoInsuficiente = !seguir;
            }
            if (ModelState.IsValid && seguir)
            {
               
                if (movimiento.TipoMovimiento == 1)
                {
                    f.ValorEfectivo = f.ValorEfectivo + movimiento.Monto;
                    f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
                    db.Entry(f).State = EntityState.Modified;
                   // await db.SaveChangesAsync();
                }
                else
                {
                    f.ValorEfectivo = f.ValorEfectivo - movimiento.Monto;
                    f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
                    db.Entry(f).State = EntityState.Modified;
                   // await db.SaveChangesAsync();
                }
                movimiento.IdProducto = 1;
                movimiento.Usuario = User.Identity.GetUserName();
                movimiento.FechaMovimiento = movimiento.FechaMovimiento.AddHours(DateTime.Now.Hour)
                    .AddMinutes(DateTime.Now.Minute)
                    .AddSeconds(DateTime.Now.Second)
                    .AddMilliseconds(DateTime.Now.Millisecond);
                db.Movimientos.Add(movimiento);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(movimiento);
        }

        // GET: Movimientoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            Movimiento movimiento = await db.Movimientos.FindAsync(id);

            if (movimiento == null)
            {
                return HttpNotFound();
            }
            ViewBag.MontoAnterior = movimiento.Monto;
            ViewBag.TipoAnterior = movimiento.TipoMovimiento;
            return View(movimiento);
        }

        // POST: Movimientoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Nombre,Monto,TipoMovimiento,Usuario,FechaMovimiento,Descripcion")] Movimiento movimiento,string montoAnterior, string tipoAnterior)
        {
            Finanzas f = db.Finanzas.First();
            bool seguir = true;
            Restablecer_Movimiento(f,decimal.Parse(montoAnterior), Int32.Parse(tipoAnterior));
           
            if (movimiento.TipoMovimiento == 2 || movimiento.TipoMovimiento == 3)
            {
                seguir = f.ValorEfectivo > movimiento.Monto;
                ViewBag.CreditoInsuficiente = !seguir;
            }
            if (ModelState.IsValid && seguir)
            {
                if (movimiento.TipoMovimiento == 1)
                {
                    f.ValorEfectivo = f.ValorEfectivo + movimiento.Monto;
                    f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
                    db.Entry(f).State = EntityState.Modified;
                    // await db.SaveChangesAsync();
                }
                else
                {
                    f.ValorEfectivo = f.ValorEfectivo - movimiento.Monto;
                    f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
                    db.Entry(f).State = EntityState.Modified;
                    // await db.SaveChangesAsync();
                }
                movimiento.IdProducto = 1;
                movimiento.Usuario = User.Identity.GetUserName();
                movimiento.FechaMovimiento = movimiento.FechaMovimiento.AddHours(DateTime.Now.Hour)
                   .AddMinutes(DateTime.Now.Minute)
                   .AddSeconds(DateTime.Now.Second)
                   .AddMilliseconds(DateTime.Now.Millisecond);
                db.Entry(f).State = EntityState.Modified;
                db.Entry(movimiento).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MontoAnterior = movimiento.Monto;
            ViewBag.TipoAnterior = movimiento.TipoMovimiento;
            return View(movimiento);
        }

        // GET: Movimientoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
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
            Movimiento movimiento = await db.Movimientos.FindAsync(id);
            if (movimiento == null)
            {
                return HttpNotFound();
            }
            return View(movimiento);
        }

        // POST: Movimientoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Movimiento movimiento = await db.Movimientos.FindAsync(id);
            Finanzas f = db.Finanzas.First();
            if(movimiento.IdProducto==1)
            Restablecer_Movimiento( f, movimiento.Monto,movimiento.TipoMovimiento);
            db.Movimientos.Remove(movimiento);
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

        private void Restablecer_Movimiento( Finanzas f, decimal montoanterior, int tipoant)
        {
          
            if (tipoant == 2 || tipoant == 3)
            {
                 f.ValorEfectivo = f.ValorEfectivo + montoanterior;
                f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
               
            }
            else
            {
                f.ValorEfectivo = f.ValorEfectivo - montoanterior;
                f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
               
            }
            

        }
    }

}
