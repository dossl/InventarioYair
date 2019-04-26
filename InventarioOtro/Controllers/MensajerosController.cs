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

using PagedList;

namespace InventarioOtro.Controllers
{
    [Authorize]
    public class MensajerosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Mensajeros
        public async Task<ActionResult> Index(string searchString, string categoriaId, int? page, int? paso)
        {
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
            IndexMensajeroViewModel ip = new IndexMensajeroViewModel();

            var lista = new List<Mensajero>();
            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.Search = searchString;
                if (!String.IsNullOrWhiteSpace(categoriaId))
                {
                    ViewBag.CategoriaId = categoriaId;

                    lista = await db.Mensajeros.Where(p => p.Nombre.ToLower().Contains(searchString.ToLower())).ToListAsync();

                }
                else
                {
                    ViewBag.CategoriaId = "";
                    lista = await db.Mensajeros.Where(p => p.Nombre.ToLower().Contains(searchString.ToLower())).ToListAsync();

                }


            }
            else
            {
                ViewBag.Search = "";
                if (!String.IsNullOrEmpty(categoriaId))
                {
                    ViewBag.CategoriaId = categoriaId;
                    lista = await db.Mensajeros.ToListAsync();
                }
                else
                {
                    ViewBag.CategoriaId = "";
                    lista = await db.Mensajeros.ToListAsync();
                }


            }
            if (lista.Count <= ViewBag.Paso)
            {
                ViewBag.Page = 1;
            }
            ip.ListMensajeros = lista.ToPagedList((int)ViewBag.Page, (int)ViewBag.Paso);
            return View(ip);
        }

        // GET: Mensajeros/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mensajero mensajero = await db.Mensajeros.FindAsync(id);
            if (mensajero == null)
            {
                return HttpNotFound();
            }
            return View(mensajero);
        }

        // GET: Mensajeros/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mensajeros/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Nombre")] Mensajero mensajero)
        {
            var existe = db.Mensajeros.Any(c => c.Nombre.ToLower().Equals(mensajero.Nombre.ToLower()));
            if (ModelState.IsValid && !existe)
            {
                db.Mensajeros.Add(mensajero);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(mensajero);
        }

        // GET: Mensajeros/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mensajero mensajero = await db.Mensajeros.FindAsync(id);
            if (mensajero == null)
            {
                return HttpNotFound();
            }
            return View(mensajero);
        }

        // POST: Mensajeros/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Nombre")] Mensajero mensajero)
        {
            var existe = db.Mensajeros.Any(c => c.Nombre.ToLower().Equals(mensajero.Nombre.ToLower()));
            if (ModelState.IsValid && !existe)
            {
                db.Entry(mensajero).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(mensajero);
        }

        // GET: Mensajeros/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mensajero mensajero = await db.Mensajeros.FindAsync(id);
            if (mensajero == null)
            {
                return HttpNotFound();
            }
            return View(mensajero);
        }

        // POST: Mensajeros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Mensajero mensajero = await db.Mensajeros.FindAsync(id);
            db.Mensajeros.Remove(mensajero);
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
    }
}
