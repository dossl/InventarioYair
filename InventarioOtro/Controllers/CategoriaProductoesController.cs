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
    public class CategoriaProductoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CategoriaProductoes
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
            BuscadorCategoriaProductoViewModel ip = new BuscadorCategoriaProductoViewModel();
           
            var lista = new List<CategoriaProducto>();
            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.Search = searchString;
                if (!String.IsNullOrWhiteSpace(categoriaId))
                {
                    ViewBag.CategoriaId = categoriaId;

                    lista = await db.CategoriaProductoes.Where(p => p.Cnombre.ToLower().Contains(searchString.ToLower())).ToListAsync();

                }
                else
                {
                    ViewBag.CategoriaId = "";
                    lista = await db.CategoriaProductoes.Where(p => p.Cnombre.ToLower().Contains(searchString.ToLower())).ToListAsync();

                }


            }
            else
            {
                ViewBag.Search = "";
                if (!String.IsNullOrEmpty(categoriaId))
                {
                    ViewBag.CategoriaId = categoriaId;
                    lista = await db.CategoriaProductoes.ToListAsync();
                }
                else
                {
                    ViewBag.CategoriaId = "";
                    lista = await db.CategoriaProductoes.ToListAsync();
                }


            }
            if (lista.Count <= ViewBag.Paso)
            {
                ViewBag.Page = 1;
            }
            ip.ListCategoriaProductos = lista.ToPagedList((int)ViewBag.Page, (int)ViewBag.Paso);
            return View(ip);
        }

        // GET: CategoriaProductoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriaProducto categoriaProducto = await db.CategoriaProductoes.FindAsync(id);
            if (categoriaProducto == null)
            {
                return HttpNotFound();
            }
            return View(categoriaProducto);
        }

        // GET: CategoriaProductoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoriaProductoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Cnombre")] CategoriaProducto categoriaProducto)
        {
            var existe = db.CategoriaProductoes.Any(c => c.Cnombre.ToLower().Equals(categoriaProducto.Cnombre.ToLower()));
            if (ModelState.IsValid && !existe)
            {
                db.CategoriaProductoes.Add(categoriaProducto);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(categoriaProducto);
        }

        // GET: CategoriaProductoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriaProducto categoriaProducto = await db.CategoriaProductoes.FindAsync(id);
            if (categoriaProducto == null)
            {
                return HttpNotFound();
            }
            return View(categoriaProducto);
        }

        // POST: CategoriaProductoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Cnombre")] CategoriaProducto categoriaProducto)
        {
            var existe = db.CategoriaProductoes.Any(c => c.Cnombre.ToLower().Equals(categoriaProducto.Cnombre.ToLower()));
            if (ModelState.IsValid && !existe)
            {
                db.Entry(categoriaProducto).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            if (existe)
                ViewBag.Mensaje = true;
            return View(categoriaProducto);
        }

        // GET: CategoriaProductoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriaProducto categoriaProducto = await db.CategoriaProductoes.FindAsync(id);
            if (categoriaProducto == null)
            {
                return HttpNotFound();
            }
            ViewBag.Mensaje = false;
            return View(categoriaProducto);
        }

        // POST: CategoriaProductoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CategoriaProducto categoriaProducto = await db.CategoriaProductoes.FindAsync(id);
            if (!db.Productos.Any(p=>p.CategoriaProductoId==id))
            {
                
                db.CategoriaProductoes.Remove(categoriaProducto);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            
            ViewBag.Mensaje = true;
            return View(categoriaProducto);
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
