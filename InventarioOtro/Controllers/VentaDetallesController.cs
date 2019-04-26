using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
    public class VentaDetallesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: VentaDetalles
        public async Task<ActionResult> Index(Venta venta, string searchString, string categoriaId, int? page, int? paso, string tipVenta)
        {
            
            /*Seguridad s = new Seguridad();
            if (!s.ValidarSeguridad())
            {
                return View("Error");
            }*/

            if (paso == null || paso == 0)
            {
                paso =50;
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
            VentaDetallesViewModel ip = new VentaDetallesViewModel()
            {
                ListCategoriaProductos = db.CategoriaProductoes.ToList(),

            };
            var lista = new List<Producto>();
            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.Search = searchString;
                if (!String.IsNullOrWhiteSpace(categoriaId))
                {
                    ViewBag.CategoriaId = categoriaId;

                    lista = await db.Productos.Where(p => p.Carticulo.ToLower().Contains(searchString.ToLower()) && p.CategoriaProductoId.ToString().Equals(categoriaId) && p.Lactivo).Include(p => p.CategoriaProducto).ToListAsync();

                }
                else
                {
                    ViewBag.CategoriaId = "";
                    lista = await db.Productos.Where(p => p.Carticulo.ToLower().Contains(searchString.ToLower()) && p.Lactivo).Include(p => p.CategoriaProducto).ToListAsync();
                }


            }
            else
            {
                ViewBag.Search = "";
                if (!String.IsNullOrEmpty(categoriaId))
                {
                    ViewBag.CategoriaId = categoriaId;
                    lista = await db.Productos.Where(p => p.CategoriaProductoId.ToString().Equals(categoriaId) && p.Lactivo).Include(p => p.CategoriaProducto).ToListAsync();
                }
                else
                {
                    ViewBag.CategoriaId = "";
                    lista = await db.Productos.Where(p=> p.Lactivo).Include(p => p.CategoriaProducto).ToListAsync();
                }


            }
            if (lista.Count <= ViewBag.Paso)
            {
                ViewBag.Page = 1;
            }
            ip.ListProductos = lista.ToPagedList((int)ViewBag.Page, (int)ViewBag.Paso);

            ViewBag.Min = "1".Equals(tipVenta);
                
            return View(ip);
        }

        // GET: VentaDetalles/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VentaDetalle ventaDetalle = await db.VentaDetalles.FindAsync(id);
            if (ventaDetalle == null)
            {
                return HttpNotFound();
            }
            return View(ventaDetalle);
        }

        // GET: VentaDetalles/Create
        public ActionResult Create()
        {
            ViewBag.ProductoId = new SelectList(db.Productos, "ID", "Carticulo");
            ViewBag.VentaId = new SelectList(db.Ventas, "ID", "Usuario");
            return View();
        }
        [HttpPost]
        public ActionResult Save([Bind(Include = "ListProductos")]VentaDetallesViewModel vd)
        {

            return View("Create");
        }

        // POST: VentaDetalles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,ProductoId,VentaId,Cantidad,Garantia,Descuento")] VentaDetalle ventaDetalle)
        {
            if (ModelState.IsValid)
            {
                db.VentaDetalles.Add(ventaDetalle);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProductoId = new SelectList(db.Productos, "ID", "Carticulo", ventaDetalle.ProductoId);
            ViewBag.VentaId = new SelectList(db.Ventas, "ID", "Usuario", ventaDetalle.VentaId);
            return View(ventaDetalle);
        }

        // GET: VentaDetalles/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VentaDetalle ventaDetalle = await db.VentaDetalles.FindAsync(id);
            if (ventaDetalle == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductoId = new SelectList(db.Productos, "ID", "Carticulo", ventaDetalle.ProductoId);
            ViewBag.VentaId = new SelectList(db.Ventas, "ID", "Usuario", ventaDetalle.VentaId);
            return View(ventaDetalle);
        }

        // POST: VentaDetalles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,ProductoId,VentaId,Cantidad,Garantia,Descuento")] VentaDetalle ventaDetalle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ventaDetalle).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProductoId = new SelectList(db.Productos, "ID", "Carticulo", ventaDetalle.ProductoId);
            ViewBag.VentaId = new SelectList(db.Ventas, "ID", "Usuario", ventaDetalle.VentaId);
            return View(ventaDetalle);
        }

        // GET: VentaDetalles/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VentaDetalle ventaDetalle = await db.VentaDetalles.FindAsync(id);
            if (ventaDetalle == null)
            {
                return HttpNotFound();
            }
            return View(ventaDetalle);
        }

        // POST: VentaDetalles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            VentaDetalle ventaDetalle = await db.VentaDetalles.FindAsync(id);
            db.VentaDetalles.Remove(ventaDetalle);
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

        public ActionResult Indext()
        {
           
            Venta v = (Venta)TempData["venta"];
            VentaDetallesViewModel model= new VentaDetallesViewModel();
            
            return RedirectToAction("Index",v);
        }
    }
}
