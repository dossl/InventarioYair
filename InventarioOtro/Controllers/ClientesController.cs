using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using InventarioOtro.Models;
using InventarioOtro.ViewModels;

using PagedList;

namespace InventarioOtro.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

       

        // GET: Clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Clastname,Cfirstname,Ccidentidad,Cnumtelefono,Lactivo,Cdireccion,Cnotas")] Cliente cliente)
        {
            var existe = db.Clientes.Any(c => c.Ccidentidad == cliente.Ccidentidad);
            if (ModelState.IsValid && !existe)
            {
                db.Clientes.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Clastname,Cfirstname,Ccidentidad,Cnumtelefono,Lactivo,Cdireccion,Cnotas")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
            db.Clientes.Remove(cliente);
            db.SaveChanges();
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

        // Buscador
        public ActionResult Index( string searchString, string categoriaId, int? page, int? paso)
        {
            var listcriterio = new List<SelectListItem>();
            listcriterio.Add(new SelectListItem()
            {
                Value = "Cfirstname",
                Text = "Nombre"
            });
            listcriterio.Add(new SelectListItem()
            {
                Value = "Clastname",
                Text = "Apellidos"
            });
            listcriterio.Add(new SelectListItem()
            {
                Value = "Ccidentidad",
                Text = "No. de identidad"
            });
            listcriterio.Add(new SelectListItem()
            {
                Value = "Cnumtelefono",
                Text = "No. de teléfono"
            });
            listcriterio.Add(new SelectListItem()
            {
                Value = "Cdireccion",
                Text = "Dirección"
            });
            List<Cliente> clientes = new List<Cliente>();


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
            BuscadorClienteViewModel viewModel = new BuscadorClienteViewModel()
            {
                Listcriterio = listcriterio,

            };
            var lista = new List<Cliente>();
            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.Search = searchString;
                if (!String.IsNullOrWhiteSpace(categoriaId))
                {
                    ViewBag.CategoriaId = categoriaId;

                    if ("Cfirstname".Equals(categoriaId))
                    {
                        lista = db.Clientes.Where(p => p.Cfirstname.ToLower().Contains(searchString.ToLower())).ToList();
                    }
                    else
                    {
                        if ("Clastname".Equals(categoriaId))
                        {
                            lista = db.Clientes.Where(p => p.Clastname.ToLower().Contains(searchString.ToLower())).ToList();
                        }
                        else
                        {
                            
                            if ("Ccidentidad".Equals(categoriaId))
                            {
                                lista = db.Clientes.Where(p => p.Ccidentidad.ToLower().Contains(searchString.ToLower())).ToList();
                            }
                            else
                            {
                                if ("Cnumtelefono".Equals(categoriaId))
                                {
                                    lista = db.Clientes.Where(p => p.Cnumtelefono.ToLower().Contains(searchString.ToLower())).ToList();
                                }
                                else
                                {
                                    if ("Cdireccion".Equals(categoriaId))
                                    {
                                        lista = db.Clientes.Where(p => p.Cdireccion.ToLower().Contains(searchString.ToLower())).ToList();
                                    }
                                }
                            }
                        }
                    }
                    

                }
                else
                {
                    ViewBag.CategoriaId = "";
                    lista = db.Clientes.Where(p => p.Cfirstname.ToLower().Contains(searchString.ToLower())).ToList();
                }


            }
            else
            {
                ViewBag.Search = "";
                if (!String.IsNullOrEmpty(categoriaId))
                {
                    ViewBag.CategoriaId = categoriaId;
                    lista = db.Clientes.ToList();
                }
                else
                {
                    ViewBag.CategoriaId = "";
                    lista = db.Clientes.ToList();
                }


            }
            if (lista.Count <= ViewBag.Paso)
            {
                ViewBag.Page = 1;
            }
            viewModel.ListClientes = lista.ToPagedList((int)ViewBag.Page, (int)ViewBag.Paso);


            return View(viewModel);
        }

    }
}
