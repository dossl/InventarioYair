using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

using System.Web.Mvc;

using InventarioOtro.Models;
using InventarioOtro.ViewModels;
using PagedList;

namespace InventarioOtro.Controllers
{
    [Authorize]
    public class VendedorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Vendedors
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
            IndexVendedorViewModel ip = new IndexVendedorViewModel();

            var lista = new List<Vendedor>();
            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.Search = searchString;
                if (!String.IsNullOrWhiteSpace(categoriaId))
                {
                    ViewBag.CategoriaId = categoriaId;

                    lista = await db.Vendedores.Where(p => p.Nombre.ToLower().Contains(searchString.ToLower())).ToListAsync();

                }
                else
                {
                    ViewBag.CategoriaId = "";
                    lista = await db.Vendedores.Where(p => p.Nombre.ToLower().Contains(searchString.ToLower())).ToListAsync();

                }


            }
            else
            {
                ViewBag.Search = "";
                if (!String.IsNullOrEmpty(categoriaId))
                {
                    ViewBag.CategoriaId = categoriaId;
                    lista = await db.Vendedores.ToListAsync();
                }
                else
                {
                    ViewBag.CategoriaId = "";
                    lista = await db.Vendedores.ToListAsync();
                }


            }
            if (lista.Count <= ViewBag.Paso)
            {
                ViewBag.Page = 1;
            }
            ip.ListVendedores = lista.ToPagedList((int)ViewBag.Page, (int)ViewBag.Paso);
            return View(ip);
            
        }

        // GET: Vendedors/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendedor vendedor = await db.Vendedores.FindAsync(id);
            if (vendedor == null)
            {
                return HttpNotFound();
            }
            return View(vendedor);
        }

        // GET: Vendedors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vendedors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Nombre")] Vendedor vendedor)
        {
            var existe = db.Vendedores.Any(c => c.Nombre.ToLower().Equals(vendedor.Nombre.ToLower()));
            if (ModelState.IsValid && !existe)
            {
                db.Vendedores.Add(vendedor);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(vendedor);
        }

        // GET: Vendedors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendedor vendedor = await db.Vendedores.FindAsync(id);
            if (vendedor == null)
            {
                return HttpNotFound();
            }
            return View(vendedor);
        }

        // POST: Vendedors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Nombre")] Vendedor vendedor)
        {
            var existe = db.Vendedores.Any(c => c.Nombre.ToLower().Equals(vendedor.Nombre.ToLower()));
            if (ModelState.IsValid && !existe)
            {
                db.Entry(vendedor).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            if (existe)
                ViewBag.Mensaje = true;
            return View(vendedor);
        }

        // GET: Vendedors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendedor vendedor = await db.Vendedores.FindAsync(id);
            if (vendedor == null)
            {
                return HttpNotFound();
            }
            return View(vendedor);
        }

        // POST: Vendedors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Vendedor vendedor = await db.Vendedores.FindAsync(id);
            db.Vendedores.Remove(vendedor);
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
