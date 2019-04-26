using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using InventarioOtro.Models;
using InventarioOtro.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;

namespace InventarioOtro.Controllers
{
    [Authorize]
    public class ProductoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Productoes
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
            IndexProductoViewModel ip=new IndexProductoViewModel()
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
                    
                    lista = await db.Productos.Where(p => p.Carticulo.ToLower().Contains(searchString.ToLower()) && p.CategoriaProductoId.ToString().Equals(categoriaId)).Include(p => p.CategoriaProducto).ToListAsync();

                }
                else
                {
                    ViewBag.CategoriaId = "";
                    lista =  await db.Productos.Where(p => p.Carticulo.ToLower().Contains(searchString.ToLower())).Include(p => p.CategoriaProducto).ToListAsync();
                }
                
                
            }
            else
            {
                ViewBag.Search = "";
                if (!String.IsNullOrEmpty(categoriaId))
                {
                    ViewBag.CategoriaId = categoriaId;
                    lista = await db.Productos.Where(p =>  p.CategoriaProductoId.ToString().Equals(categoriaId)).Include(p => p.CategoriaProducto).ToListAsync();
                }
                else
                {
                    ViewBag.CategoriaId = "";
                    lista = await db.Productos.Include(p => p.CategoriaProducto).ToListAsync();
                }
                   
                
            }
            if (lista.Count<= ViewBag.Paso)
            {
                ViewBag.Page = 1;
            }
            ViewBag.CatSortParm = String.IsNullOrEmpty(sortOrder) ? "cat_desc" : ""; 
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.NameSortParm = sortOrder == "articulo" ? "articulo_desc" : "articulo";
            ViewBag.DisSortParm = sortOrder == "dis" ? "dis_desc" : "dis";
            ViewBag.LoteSortParm= sortOrder == "lote" ? "lote_desc" : "lote";
            ViewBag.ActiveSortParm = sortOrder == "act" ? "n_act" : "act";
            ViewBag.PrecMinSortParm = sortOrder == "precioM" ? "precioM_desc" : "precioM";
            ViewBag.PrecMaySortParm = sortOrder == "precioMa" ? "precioMa_desc" : "precioMa";
            ViewBag.PrecCoSortParm = sortOrder == "precioC" ? "precioC_desc" : "precioC";



            switch (sortOrder)
            {
                case "articulo_desc":
                    lista = lista.OrderByDescending(l => l.Carticulo ).ThenBy(l=>l.Dfechacreacion).ToList();
                    break;
                case "Date":
                    lista = lista.OrderBy(l => l.Dfechacreacion).ThenBy(l => l.Dfechacreacion).ToList();
                    break;
                case "date_desc":
                    lista = lista.OrderByDescending(l => l.Dfechacreacion).ThenBy(l => l.Dfechacreacion).ToList();
                    break;
                case "articulo":
                    lista = lista.OrderBy(l => l.Carticulo).ThenBy(l => l.Dfechacreacion).ToList();
                    break;
                case "cat_desc":
                    lista = lista.OrderByDescending(l => l.CategoriaProducto.Cnombre).ThenBy(l => l.Dfechacreacion).ToList();
                    break;
                case "dis":
                    lista = lista.OrderBy(l => l.Ndisponibilidad).ThenBy(l => l.Dfechacreacion).ToList();
                    break;
                case "dis_desc":
                    lista = lista.OrderByDescending(l => l.Ndisponibilidad).ThenBy(l => l.Dfechacreacion).ToList();
                    break;
                case "lote":
                    lista = lista.OrderBy(l => l.Clote).ThenBy(l => l.Dfechacreacion).ToList();
                    break;
                case "lote_desc":
                    lista = lista.OrderByDescending(l => l.Clote).ThenBy(l => l.Dfechacreacion).ToList();
                    break;
                case "act":
                    lista = lista.OrderBy(l => l.Lactivo).ThenBy(l => l.Dfechacreacion).ToList();
                    break;
                case "n_act":
                    lista = lista.OrderByDescending(l => l.Lactivo).ThenBy(l => l.Dfechacreacion).ToList();
                    break;
                case "precioM":
                    lista = lista.OrderBy(l => l.Nprecminoris).ThenBy(l => l.Dfechacreacion).ToList();
                    break;
                case "preciom_desc":
                    lista = lista.OrderByDescending(l => l.Nprecminoris).ThenBy(l => l.Dfechacreacion).ToList();
                    break;
                case "precioMa":
                    lista = lista.OrderBy(l => l.Nprecmayoris).ThenBy(l => l.Dfechacreacion).ToList();
                    break;
                case "precioMa_desc":
                    lista = lista.OrderByDescending(l => l.Nprecmayoris).ThenBy(l => l.Dfechacreacion).ToList();
                    break;
                case "precioC":
                    lista = lista.OrderBy(l => l.Npreccosto).ThenBy(l => l.Dfechacreacion).ToList();
                    break;
                case "precioC_desc":
                    lista = lista.OrderByDescending(l => l.Npreccosto).ThenBy(l => l.Dfechacreacion).ToList();
                    break;
                default:
                    lista = lista.OrderBy(l => l.CategoriaProducto.Cnombre).ThenBy(l => l.Dfechacreacion).ToList(); 
                    break;
            }
          
        ip.ListProductos= lista.ToPagedList((int)ViewBag.Page, (int)ViewBag.Paso);
            return View(ip);
        }

        // GET: Productoes/Details/5
        public async Task<ActionResult> Details(int? id, string m)
        {
            ViewBag.Mov = string.IsNullOrEmpty(m);
           /* Seguridad s = new Seguridad();
            if (!s.ValidarSeguridad())
            {
                return View("Error");
            }*/
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = await db.Productos.FindAsync(id);
            
            if (producto == null)
            {
                return HttpNotFound();
            }
            producto.CategoriaProducto = db.CategoriaProductoes.Find(producto.CategoriaProductoId);
            producto.ListaFotos = db.FotoProductos.Where(f => f.ProductoId == producto.ID).ToList();


            DetailProductoViewModel d = new DetailProductoViewModel()
            {
                Producto = producto
            };


            List<String> fotosList = new List<String>();
            foreach (var val in producto.ListaFotos)
            {
                fotosList.Add( "data:" + val.ImageMimeType + ";base64," + Convert.ToBase64String(val.ImageContent));
            }
            d.ListImgSrc = fotosList;
            return View(d);
        }

        // GET: Productoes/Create
        public ActionResult Create()
        {
            /*Seguridad s = new Seguridad();
            if (!s.ValidarSeguridad())
            {
                return View("Error");
            }*/
            ViewBag.Mensaje = false;
            var tiposCate = db.CategoriaProductoes.ToList();
            ProductoViewModel p = new ProductoViewModel()
            {
                TipoCategoriaProductos = tiposCate,
                Producto=new Producto()
            };
            p.Producto.Ndescuento = 0;
            p.Producto.Lactivo = true;
            p.Producto.Dfechacreacion = DateTime.Now;
            return View(p);
        }

        // POST: Productoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Carticulo,Cdescripcion,CategoriaProductoId,Ncantidad,Npreccosto,Nprecmayoris,Nprecminoris,Ngananctrab,Clote,Ndescuento,Ndisponibilidad,Lactivo,Linversion,Dfechacreacion")] Producto producto)
        {

            bool seguir = true;
            Finanzas f = db.Finanzas.First();
            if (producto.Linversion)
            {   //Actualizando Finanzas
                f.ValorProductos += producto.Npreccosto * producto.Ncantidad;
                f.ValorCapital = f.ValorEfectivo + f.ValorProductos+ f.Ganancias;
                db.Entry(f).State = EntityState.Modified;
                db.SaveChanges();
                //Registrando Movimientos
               /* Registrar_Movimiento(4, "Compra de producto: " + producto.Carticulo,
                    "Se compra el producto: " + producto.Carticulo +
                    " por inversión aumentando valor en productos en: " + producto.Npreccosto * producto.Ncantidad,
                    producto.Npreccosto * producto.Ncantidad, 0);*/
              
               
            }
            else
            {
               	//Restar el costo total al Total de efectivo. 
                  //  Total de Efectivo = Total de Efectivo -(∑ precio costos *cantidad productos).
                  f.ValorEfectivo-=producto.Npreccosto*producto.Ncantidad;
                if (f.ValorEfectivo<0)
                {
                    seguir = false;
                    ViewBag.Mensaje = true;
                }
                else
                {
                    //	Sumar el costo total al Dinero invertido
                    //    Dinero en inventario = Dinero en inventario+(∑ precio costos *cantidad productos).
                    f.ValorProductos+= producto.Npreccosto * producto.Ncantidad;
                    //Actualizando Finanzas
                    f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
                    db.Entry(f).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    //Registrando Movimientos
                    //Ingreso
                    /*Registrar_Movimiento(4, "Compra de producto: " + producto.Carticulo,
                        "Se compra el producto: " + producto.Carticulo +
                        " aumentando valor en productos en: " + producto.Npreccosto * producto.Ncantidad,
                        producto.Npreccosto * producto.Ncantidad, 0);
                    //Gasto
                    Registrar_Movimiento(4, "Disminuye efectivo por Compra de producto: " + producto.Carticulo,
                        "Se compra el producto: " + producto.Carticulo +
                        " disminuyendo valor en efectivo en: " + producto.Npreccosto * producto.Ncantidad,
                        producto.Npreccosto * producto.Ncantidad, 0);*/
                    
                }

                    

            }


            ProductoViewModel p = new ProductoViewModel()
            {
                Producto = producto
            };
            if (ModelState.IsValid && seguir)
            {
               
                producto.Dfechacreacion=DateTime.Now;
                db.Productos.Add(producto);
                await db.SaveChangesAsync();
                if (producto.Linversion)
                {
                    Registrar_Movimiento(4, "Compra de producto: " + producto.Carticulo+". Cant. "+producto.Ndisponibilidad,
                    "Se compra el producto: " + producto.Carticulo +
                    " por inversión aumentando valor en productos en: " + producto.Npreccosto * producto.Ncantidad,
                    producto.Npreccosto * producto.Ncantidad, 0, producto.ID);
                }
                else
                {
                    Registrar_Movimiento(4, "Compra de producto: " + producto.Carticulo + ". Cant. " + producto.Ndisponibilidad,
                        "Se compra el producto: " + producto.Carticulo +
                        " aumentando valor en productos en: " + producto.Npreccosto * producto.Ncantidad,
                        producto.Npreccosto * producto.Ncantidad, 0, producto.ID);
                    //Gasto
                    Registrar_Movimiento(4, "Disminuye efectivo por Compra de producto: " + producto.Carticulo + ". Cant. " + producto.Ndisponibilidad,
                        "Se compra el producto: " + producto.Carticulo +
                        " disminuyendo valor en efectivo en: " + producto.Npreccosto * producto.Ncantidad,
                        producto.Npreccosto * producto.Ncantidad, 0, producto.ID);
                }
                
                return RedirectToAction("Index");
            }
           
            p.TipoCategoriaProductos = db.CategoriaProductoes.ToList();
            return View(p);
        }

        // GET: Productoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
           /* Seguridad s = new Seguridad();
            if (!s.ValidarSeguridad())
            {
                return View("Error");
            }*/
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           ViewBag.ReadOnly = YaseVendioProducto(id);
            
            
            ViewBag.Mensaje = false;
            Producto producto = await db.Productos.FindAsync(id);
            if (producto!=null)
            {
                ViewBag.CantidadAnterior = producto.Ncantidad;
                ViewBag.PrecioCostoAnterior = producto.Npreccosto;
            }
           
            var tiposCate = db.CategoriaProductoes.ToList();
            ProductoViewModel p = new ProductoViewModel()
            {
                TipoCategoriaProductos = tiposCate,
                Producto = producto
            };
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(p);
        }

        // POST: Productoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Carticulo,Cdescripcion,CategoriaProductoId,Ncantidad,Npreccosto,Nprecmayoris,Nprecminoris,Ngananctrab,Clote,Ndescuento,Ndisponibilidad,Lactivo,Linversion,Dfechacreacion")] Producto producto, string nuevaCantidad, string cantidadAnterior, string precioCostoAnterior)
        {
            ViewBag.ReadOnly = YaseVendioProducto(producto.ID);

            //ViewBag.ReadOnly = false;
            ViewBag.Mensaje = false;
            var cantAnterior = decimal.Parse(cantidadAnterior);
            var precioAnterior = decimal.Parse(precioCostoAnterior);
            bool seguir ;
            if (!string.IsNullOrEmpty(nuevaCantidad))
            {

                if (Int32.Parse(nuevaCantidad) > 0)
                {
                    producto.Ncantidad += Int32.Parse(nuevaCantidad);
                    producto.Ndisponibilidad += Int32.Parse(nuevaCantidad);

                    Finanzas f = db.Finanzas.First();
                    if (producto.Linversion)
                    {
                        //Actualizando Finanzas
                        f.ValorProductos += producto.Npreccosto * Int32.Parse(nuevaCantidad);
                        f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
                        db.Entry(f).State = EntityState.Modified;
                        db.SaveChanges();
                        //Registrando Movimientos
                        //Ingreso
                        Registrar_Movimiento(4, "Ingresando producto: " + producto.Carticulo + ". Cant. " + nuevaCantidad,
                            "Se compra el producto: " + producto.Carticulo +
                            "por inversión aumentando valor en productos en: " + producto.Npreccosto * Int32.Parse(nuevaCantidad),
                            producto.Npreccosto * Int32.Parse(nuevaCantidad), 0,producto.ID);

                      
                    }
                    else
                    {
                        //Restar el costo total al Total de efectivo. 
                        //  Total de Efectivo = Total de Efectivo -(∑ precio costos *cantidad productos).
                        f.ValorEfectivo -= producto.Npreccosto * Int32.Parse(nuevaCantidad);
                        if (f.ValorEfectivo < 0)
                        {
                            seguir = false;
                            ViewBag.Mensaje = true;
                        }
                        else
                        {
                            //	Sumar el costo total al Dinero invertido
                            //    Dinero en inventario = Dinero en inventario+(∑ precio costos *cantidad productos).
                            f.ValorProductos += producto.Npreccosto * Int32.Parse(nuevaCantidad);
                            //Actualizando Finanzas
                            f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
                            db.Entry(f).State = EntityState.Modified;
                            await db.SaveChangesAsync();

                            //Registrando Movimientos
                            //Ingreso
                            Registrar_Movimiento(4, "Ingresando producto: " + producto.Carticulo + ". Cant. " + nuevaCantidad,
                                "Se compra el producto: " + producto.Carticulo +
                                " aumentando valor en productos en: " + producto.Npreccosto * Int32.Parse(nuevaCantidad),
                                producto.Npreccosto * Int32.Parse(nuevaCantidad), 0,producto.ID);

                            //Gastos
                            Registrar_Movimiento(4, "Disminuye Efectivo por compra de producto: " + producto.Carticulo + ". Cant. " + nuevaCantidad,
                                "Se compra el producto: " + producto.Carticulo +
                                " disminuyendo efectivo en: " + producto.Npreccosto * Int32.Parse(nuevaCantidad),
                                producto.Npreccosto * Int32.Parse(nuevaCantidad), 0,producto.ID);
                          
                        }

                    }
                }
                else
                {
                   
                    producto.Ncantidad += Int32.Parse(nuevaCantidad);
                    producto.Ndisponibilidad += Int32.Parse(nuevaCantidad);

                    Finanzas f = db.Finanzas.First();
                    if (producto.Linversion)
                    {
                        //Actualizando Finanzas
                        f.ValorProductos += producto.Npreccosto * Int32.Parse(nuevaCantidad);
                        f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
                        db.Entry(f).State = EntityState.Modified;
                        db.SaveChanges();
                        //Registrando Movimientos
                        //Gasto
                        Registrar_Movimiento(4, "Reduciendo cantidad del producto: " + producto.Carticulo + ". Cant. " + nuevaCantidad,
                            "Se ha disminuido la cantidad del producto: " + producto.Carticulo +
                            " comprado por inversión disminuyendo el valor en productos en: " +( producto.Npreccosto * Int32.Parse(nuevaCantidad)),
                           producto.Npreccosto * Int32.Parse(nuevaCantidad), 0,producto.ID);
                        
                    }
                    else
                    {
                        //Restar el costo total al Total de efectivo. 
                        //  Total de Efectivo = Total de Efectivo -(∑ precio costos *cantidad productos).
                        f.ValorEfectivo -= producto.Npreccosto * Int32.Parse(nuevaCantidad);
                        if (f.ValorEfectivo < 0)
                        {
                            seguir = false;
                            ViewBag.Mensaje = true;
                        }
                        else
                        {
                            //	Sumar el costo total al Dinero invertido
                            //    Dinero en inventario = Dinero en inventario+(∑ precio costos *cantidad productos).
                            f.ValorProductos += producto.Npreccosto * Int32.Parse(nuevaCantidad);
                            //Actualizando Finanzas
                            f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
                            db.Entry(f).State = EntityState.Modified;
                            await db.SaveChangesAsync();

                            //Registrando Movimientos

                            Registrar_Movimiento(4, "Reduciendo cantidad del producto: " + producto.Carticulo + ". Cant. " + nuevaCantidad,
                                "Se ha disminuido la cantidad del producto: " + producto.Carticulo +
                                " disminuyendo el valor en productos en: " + ( producto.Npreccosto * Int32.Parse(nuevaCantidad)),
                               producto.Npreccosto * Int32.Parse(nuevaCantidad), 0,producto.ID);

                            Registrar_Movimiento(4, "Aumento de efectivo por dismución de cantidad del producto: " + producto.Carticulo + ". Cant. " + nuevaCantidad,
                                "Se ha aumentado la cantidad del producto: " + producto.Carticulo +
                                " aumentando el valor en efectivo en: " + ( producto.Npreccosto * Int32.Parse(nuevaCantidad)),
                                producto.Npreccosto * Int32.Parse(nuevaCantidad), 0,producto.ID);
                          
                        }
                    }
                }

                //----------------------------MODIFICANDO CUANDO PRECIO DISTINTO----------------------------------------
                seguir = await ModificarPrecioProducto(producto, cantAnterior, precioAnterior);
                //----------------------------------------------------------------------------------------------------

            }
            else
            {
                seguir = await ModificarPrecioProducto(producto, cantAnterior, precioAnterior);
            }
          
            var tiposCate = db.CategoriaProductoes.ToList();
            ProductoViewModel p = new ProductoViewModel()
            {
                TipoCategoriaProductos = tiposCate,
                Producto = producto
            };

            if (ModelState.IsValid && seguir)
            {
                
                db.Entry(producto).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
          
                ViewBag.CantidadAnterior = producto.Ncantidad;
                ViewBag.PrecioCostoAnterior = producto.Npreccosto;
            
            return View(p);
        }

        private async Task<bool> ModificarPrecioProducto(Producto producto, decimal cantAnterior, decimal precioAnterior)
        {
            bool seguir=true;
            if (cantAnterior.Equals(producto.Ncantidad))
            {
                if (!precioAnterior.Equals(producto.Npreccosto))
                {
                    Finanzas f = db.Finanzas.First();
                    if (producto.Linversion)
                    {
                        //Actualizando Finanzas
                        f.ValorProductos -= precioAnterior*cantAnterior;
                        f.ValorProductos += producto.Npreccosto*producto.Ncantidad;
                        f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
                        db.Entry(f).State = EntityState.Modified;
                        db.SaveChanges();
                        //Registrando Movimientos
                        Registrar_Movimiento(4,
                            "Restauro movimiento en producto: " + producto.Carticulo + " comprado por inversión",
                            "Se ha disminuido el valor total en productos en: " + precioAnterior*cantAnterior +
                            ". Debido a que cambió el precio",
                            precioAnterior*cantAnterior, 0, producto.ID);

                        Registrar_Movimiento(4,
                            "Ingresando nuevo valor en producto: " + producto.Carticulo + " comprado por inversión",
                            "Se ha aumentado el valor total en productos en: " + producto.Npreccosto*producto.Ncantidad +
                            ". Debido a que cambió el precio",
                            producto.Npreccosto*producto.Ncantidad, 0, producto.ID);
                    }
                    else
                    {
                        //Restar el costo total al Total de efectivo. 
                        //  Total de Efectivo = Total de Efectivo -(∑ precio costos *cantidad productos).
                        f.ValorEfectivo += precioAnterior*cantAnterior;
                        f.ValorEfectivo -= producto.Npreccosto*producto.Ncantidad;
                        if (f.ValorEfectivo < 0)
                        {
                            seguir = false;
                            ViewBag.Mensaje = true;
                        }
                        else
                        {
                            //	Sumar el costo total al Dinero invertido
                            //    Dinero en inventario = Dinero en inventario+(∑ precio costos *cantidad productos).
                            f.ValorProductos -= precioAnterior*cantAnterior;
                            f.ValorProductos += producto.Npreccosto*producto.Ncantidad;
                            //Actualizando Finanzas
                            f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
                            db.Entry(f).State = EntityState.Modified;
                            await db.SaveChangesAsync();

                            //Registrando Movimientos
                            Registrar_Movimiento(4, "Restauro movimiento en producto: " + producto.Carticulo,
                                "Se ha disminuido el valor total en productos en: " + precioAnterior*cantAnterior +
                                ". Debido a que cambió el precio",
                                precioAnterior*cantAnterior, 0, producto.ID);

                            Registrar_Movimiento(4, "Restaurando movimiento en producto: " + producto.Carticulo,
                                "Se ha aumentado el valor de efectivo en: " + producto.Npreccosto*producto.Ncantidad +
                                ". Debido a que cambió el precio",
                                producto.Npreccosto*producto.Ncantidad, 0, producto.ID);

                            Registrar_Movimiento(4, "Ingresando nuevo valor en producto: " + producto.Carticulo,
                                "Se ha aumentado el valor total en productos en: " + producto.Npreccosto*producto.Ncantidad +
                                ". Debido a que cambió el precio",
                                producto.Npreccosto*producto.Ncantidad, 0, producto.ID);

                            Registrar_Movimiento(4, "Ingresando nuevo valor por compra de producto: " + producto.Carticulo,
                                "Se ha disminuido el valor en efectivo en: " + producto.Npreccosto*producto.Ncantidad +
                                ". Debido a que cambió el precio",
                                producto.Npreccosto*producto.Ncantidad, 0, producto.ID);

                            Registrar_Movimiento(4, "Restaurando movimiento en producto: " + producto.Carticulo,
                                "Se ha aumentado el valor de efectivo en: " + producto.Npreccosto*producto.Ncantidad +
                                ". Debido a que cambió el precio",
                                producto.Npreccosto*producto.Ncantidad, 0, producto.ID);
                        }
                    }
                }
            }
            else
            {
                if (precioAnterior.Equals(producto.Npreccosto))
                {
                    var diferencia = producto.Ncantidad - cantAnterior;
                    Finanzas f = db.Finanzas.First();
                    if (producto.Linversion)
                    {
                        //Actualizando Finanzas
                        f.ValorProductos += producto.Npreccosto*diferencia;
                        f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
                        db.Entry(f).State = EntityState.Modified;
                        db.SaveChanges();
                        //Registrando Movimientos
                        if (diferencia >= 0)
                        {
                            Registrar_Movimiento(4,
                                "Aumenta valor del producto: " + producto.Carticulo + " comprado por inversión",
                                "Se ha aumentado el valor total en productos en: " + producto.Npreccosto*diferencia,
                                producto.Npreccosto*diferencia, 0, producto.ID);
                        }
                        else
                        {
                            Registrar_Movimiento(4,
                                "Disminuye valor del producto: " + producto.Carticulo + " comprado por inversión",
                                "Se ha disminuido el valor total en productos en: " + -1*producto.Npreccosto*diferencia,
                                -1*producto.Npreccosto*diferencia, 0, producto.ID);
                        }
                    }
                    else
                    {
                        //Restar el costo total al Total de efectivo. 
                        //  Total de Efectivo = Total de Efectivo -(∑ precio costos *cantidad productos).
                        f.ValorEfectivo -= producto.Npreccosto*diferencia;
                        if (f.ValorEfectivo < 0)
                        {
                            seguir = false;
                            ViewBag.Mensaje = true;
                        }
                        else
                        {
                            //	Sumar el costo total al Dinero invertido
                            //    Dinero en inventario = Dinero en inventario+(∑ precio costos *cantidad productos).
                            f.ValorProductos += producto.Npreccosto*diferencia;
                            //Actualizando Finanzas
                            f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
                            db.Entry(f).State = EntityState.Modified;
                            await db.SaveChangesAsync();

                            //Registrando Movimientos
                            if (diferencia >= 0)
                            {
                                Registrar_Movimiento(4, "Aumenta valor del producto: " + producto.Carticulo,
                                    "Se ha aumentado el valor total en productos en: " + producto.Npreccosto*diferencia,
                                    producto.Npreccosto*diferencia, 0, producto.ID);

                                Registrar_Movimiento(4, "Disminuye valor en efectivo por compra del producto: " + producto.Carticulo,
                                    "Se ha disminuido el valor en efectivo en: " + producto.Npreccosto*diferencia,
                                    producto.Npreccosto*diferencia, 0, producto.ID);
                            }
                            else
                            {
                                Registrar_Movimiento(4, "Disminuye valor del producto: " + producto.Carticulo,
                                    "Se ha disminuido el valor total en productos en: " + -1*producto.Npreccosto*diferencia,
                                    -1*producto.Npreccosto*diferencia, 0, producto.ID);

                                Registrar_Movimiento(4, "Aumenta valor en efectivo por disminución del producto: " + producto.Carticulo,
                                    "Se ha aumentado el efectivo en: " + -1*producto.Npreccosto*diferencia,
                                    -1*producto.Npreccosto*diferencia, 0, producto.ID);
                            }
                        }
                    }
                }
                else
                {
                    Finanzas f = db.Finanzas.First();
                    if (producto.Linversion)
                    {
                        //Actualizando Finanzas
                        var diferencia = producto.Ncantidad - cantAnterior;
                        f.ValorProductos -= precioAnterior*cantAnterior;
                        f.ValorProductos += producto.Npreccosto*producto.Ncantidad;
                        f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
                        db.Entry(f).State = EntityState.Modified;
                        db.SaveChanges();
                        //Registrando Movimientos
                        if (diferencia >= 0)
                        {
                            Registrar_Movimiento(4, "Aumenta el valor producto: " + producto.Carticulo + " por inversión",
                                "Se ha aumentado el valor total en productos en: " + producto.Npreccosto*producto.Ncantidad,
                                producto.Npreccosto*diferencia, 0, producto.ID);

                            Registrar_Movimiento(4,
                                "Restableciendo movimiento del producto: " + producto.Carticulo + " por inversión",
                                "Se ha disminuido el valor total en productos en: " + precioAnterior*cantAnterior,
                                precioAnterior*cantAnterior, 0, producto.ID);
                        }
                        else
                        {
                            Registrar_Movimiento(4, "Disminuye el valor en producto: " + producto.Carticulo + " por inversión",
                                "Se ha disminuido el valor total en productos en: " + producto.Npreccosto*producto.Ncantidad,
                                producto.Npreccosto*producto.Ncantidad, 0, producto.ID);

                            Registrar_Movimiento(4,
                                "Restableciendo movimiento del producto: " + producto.Carticulo + " por inversión",
                                "Se ha disminuido el valor total en productos en: " + precioAnterior*cantAnterior,
                                precioAnterior*cantAnterior, 0, producto.ID);
                        }
                    }
                    else
                    {
                        //Restar el costo total al Total de efectivo. 
                        //  Total de Efectivo = Total de Efectivo -(∑ precio costos *cantidad productos).
                        var diferencia = producto.Ncantidad - cantAnterior;
                        f.ValorEfectivo += precioAnterior*cantAnterior;
                        f.ValorEfectivo -= producto.Npreccosto*producto.Ncantidad;
                        if (f.ValorEfectivo < 0)
                        {
                            seguir = false;
                            ViewBag.Mensaje = true;
                        }
                        else
                        {
                            //	Sumar el costo total al Dinero invertido
                            //    Dinero en inventario = Dinero en inventario+(∑ precio costos *cantidad productos).
                            f.ValorProductos -= precioAnterior*cantAnterior;
                            f.ValorProductos += producto.Npreccosto*producto.Ncantidad;
                            ;
                            //Actualizando Finanzas
                            f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
                            db.Entry(f).State = EntityState.Modified;
                            await db.SaveChangesAsync();

                            //Registrando Movimientos
                            if (diferencia >= 0)
                            {
                                Registrar_Movimiento(4, "Aumenta el valor en producto: " + producto.Carticulo,
                                    "Se ha aumentado el valor total en productos en: " + producto.Npreccosto*producto.Ncantidad,
                                    producto.Npreccosto*producto.Npreccosto*producto.Ncantidad, 0, producto.ID);

                                Registrar_Movimiento(4, "Restableciendo movimiento del producto: " + producto.Carticulo,
                                    "Se ha disminuido el valor total en productos en: " + precioAnterior*cantAnterior,
                                    precioAnterior*cantAnterior, 0, producto.ID);

                                Registrar_Movimiento(4, "Disminuye el valor en efectivo por aumento del producto: " + producto.Carticulo,
                                    "Se ha disminuido el efectivo en: " + producto.Npreccosto*producto.Ncantidad,
                                    producto.Npreccosto*producto.Ncantidad, 0, producto.ID);

                                Registrar_Movimiento(4, "Restableciendo movimiento del producto: " + producto.Carticulo,
                                    "Se ha incrementado el efectivo en: " + precioAnterior*cantAnterior,
                                    precioAnterior*cantAnterior, 0, producto.ID);
                            }
                            else
                            {
                                Registrar_Movimiento(4, "Disminuye el valor en producto: " + producto.Carticulo,
                                    "Se ha disminuido el valor total en productos en: " + producto.Npreccosto*producto.Ncantidad,
                                    producto.Npreccosto*producto.Npreccosto*producto.Ncantidad, 0, producto.ID);

                                Registrar_Movimiento(4,
                                    "Restableciendo movimiento del producto modificado: " + producto.Carticulo,
                                    "Se ha disminuido el valor total en productos en: " + precioAnterior*cantAnterior,
                                    precioAnterior*cantAnterior, 0, producto.ID);

                                Registrar_Movimiento(4, "Aumenta el valor efectivo por aumento del producto: " + producto.Carticulo,
                                    "Se ha aumentado el efectivo en: " + producto.Npreccosto*producto.Ncantidad,
                                    producto.Npreccosto*producto.Ncantidad, 0, producto.ID);

                                Registrar_Movimiento(4,
                                    "Restableciendo movimiento del producto modificado: " + producto.Carticulo,
                                    "Se ha incrementado el efectivo en: " + precioAnterior*cantAnterior,
                                    precioAnterior*cantAnterior, 0, producto.ID);
                            }
                        }
                    }
                }
            }
            return seguir;
        }

        // GET: Productoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            ViewBag.Mensaje = YaseVendioProducto(id);
            /*Seguridad s = new Seguridad();
            if (!s.ValidarSeguridad())
            {
                return View("Error");
            }*/
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = await db.Productos.FindAsync(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Productoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
           
                Finanzas f = db.Finanzas.First();
                Producto producto = await db.Productos.FindAsync(id);

                if (producto.Linversion)
                {
                    decimal valorProd = producto.Ndisponibilidad * producto.Npreccosto;
                    f.ValorProductos -= valorProd;
                    Registrar_Movimiento(4, "Eliminando producto: " + producto.Carticulo +" Descrip.: "+producto.Cdescripcion,
                        "Se ha disminuido el valor en producto en: " + valorProd, valorProd, 0,0);
                }
                else
                {
                    decimal valorProd = producto.Ndisponibilidad * producto.Npreccosto;
                    f.ValorProductos -= valorProd;
                    f.ValorEfectivo += valorProd;
                    Registrar_Movimiento(4, "Eliminando producto: " + producto.Carticulo + " Descrip.: " + producto.Cdescripcion,
                        "Se ha disminuido el valor en producto en: " + valorProd, valorProd, 0, 0);
                    Registrar_Movimiento(4, "Eliminando producto: " + producto.Carticulo + " Descrip.: " + producto.Cdescripcion,
                        "Se ha aumentado el valor en producto en: " + valorProd, valorProd, 0, 0);
                }
            f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
            db.Entry(f).State = EntityState.Modified;
                db.SaveChanges();

                db.Productos.Remove(producto);
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


        public ActionResult ListUpload(int? Id)
        {
            var lista = new List<FotoProducto>();
             lista = db.FotoProductos.Where(f=> f.ProductoId==Id).Include(f=>f.Producto).ToList();
            ViewBag.ID = Id;
            return View(lista);
        }

        public ActionResult Upload(int? Id)
        {
            FotoProductoViewModel f = new FotoProductoViewModel()
            {
                Producto = db.Productos.Find(Id),
            };
           
           
            
            return View(f);
        }

        // POST: /Download/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public ActionResult CreateUpload([Bind(Include = "ID,Title,Description,ImageContent,ImageMimeType,ImageName,FileContent,FileMimeType,FileName")] FotoProducto foto, int? ProductoId)
        {
            if (ModelState.IsValid)
            {
                 
                var ImageName = "";
                var ImageMimeType = "";
                byte[] ImageContent = { };

                var FileName = "";
                var FileMimeType = "";
                byte[] FileContent = { };

                foreach (string upload in Request.Files)
                {
                    var mimeType = Request.Files[upload].ContentType;
                    var fileStream = Request.Files[upload].InputStream;
                    var fileName =Path.GetFileName(Request.Files[upload].FileName);
                    var fileLength = Request.Files[upload].ContentLength;
                    var fileData = new byte[fileLength];
                    fileStream.Read(fileData, 0, fileLength);

                    if (upload == "FileUpload1")
                    {
                        if (!string.IsNullOrEmpty(fileName) && fileData.Length > 0 && !string.IsNullOrEmpty(mimeType))
                        {
                            ImageName = fileName;
                            ImageContent = fileData;
                            ImageMimeType = mimeType;
                        }


                    }
                    else if (upload == "FileUpload2")
                    {
                        if (!string.IsNullOrEmpty(fileName) && fileData.Length > 0 && !string.IsNullOrEmpty(mimeType))
                        {
                            FileName = fileName;
                            FileContent = fileData;
                            FileMimeType = mimeType;
                        }
                        else
                        {
                           // Errors = Resource.ResourceStore_enUS.selectFile;
                        }
                    }
                }

                    var carga = new FotoProducto();

                    carga.Title = foto.Title;
                    carga.Description = foto.Description;
                    carga.ImageName = ImageName;
                    carga.ImageContent = ImageContent;
                    carga.ImageMimeType = ImageMimeType;
                    carga.FileName = FileName;
                    carga.Bfotos = FileContent;
                    carga.FileMimeType = FileMimeType;
                    carga.ProductoId = (int)ProductoId;

                    db.FotoProductos.Add(carga);

                    db.SaveChanges();
                    return RedirectToAction("ListUpload", new { Id = ProductoId });
                
            }

            return View("Upload");
        }


        public FileContentResult GetFile(int id)
        {
            byte[] fileContent = null;
            string mimeType = ""; string fileName = "";

            var download = db.FotoProductos.Find(id);

            fileContent = download.Bfotos;
            mimeType = download.FileMimeType;
            fileName = download.FileName;
            return File(fileContent, mimeType, fileName);
        }


        // GET: Productoes/Delete/5
        public async Task<ActionResult> DeleteF(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FotoProducto foto = await db.FotoProductos.FindAsync(id);
            foto.FileName = "data:" + foto.ImageMimeType + ";base64," + Convert.ToBase64String(foto.ImageContent);
            if (foto == null)
            {
                return HttpNotFound();
            }
            return View(foto);
        }

        // POST: Productoes/Delete/5
        [HttpPost, ActionName("DeleteF")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteFConfirmed(int id)
        {
            FotoProducto foto = await db.FotoProductos.FindAsync(id);
            db.FotoProductos.Remove(foto);
            await db.SaveChangesAsync();
            return RedirectToAction("ListUpload", new { Id = foto.ProductoId });
        }


        private void Registrar_Movimiento( int tipo, string nombre, string descripcion, decimal monto, int idProd, int idProCompra)
        {
            if (monto < 0) monto = monto*-1;
            Movimiento m = new Movimiento()
            {
                TipoMovimiento = tipo,
                Descripcion = descripcion,
                FechaMovimiento = DateTime.Now,
                Monto = monto,
                Nombre = nombre,
                IdProducto = idProd,
                IdVenta = 0,
                IdProCompra = idProCompra,
                Usuario = User.Identity.GetUserName()
            };
            db.Movimientos.Add(m);
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException p)
            {
                var a=p.Message;
            }
            catch (Exception e)
            {
                var a = e.Message;
            }
            
        }

        private bool YaseVendioProducto(int? id)
        {
            bool sevendio = false;
            List<Venta> todasVentas = db.Ventas.ToList();
            int i = 0;
            while (i < todasVentas.Count && !sevendio)
            {
                var venta = todasVentas[i];
                sevendio = db.VentaDetalles.Any(vd => vd.VentaId == venta.ID && vd.ProductoId == id);
                i++;
            }
            
            return sevendio;
        }

        // GET: Productoes/Create
        public ActionResult ReCreate(int id)
        {
            /*Seguridad s = new Seguridad();
            if (!s.ValidarSeguridad())
            {
                return View("Error");
            }*/
            Producto existente = db.Productos.Include(e => e.CategoriaProducto).First(ss=>ss.ID==id);
            ViewBag.Mensaje = false;
            var tiposCate = db.CategoriaProductoes.ToList();
            ProductoViewModel p = new ProductoViewModel()
            {
                TipoCategoriaProductos = tiposCate,
                Producto = new Producto()
            };
            p.Producto.Carticulo = existente.Carticulo;
            p.Producto.Cdescripcion = existente.Cdescripcion;
            //p.Producto.Clote = existente.Clote;
            p.Producto.Npreccosto = existente.Npreccosto;
            p.Producto.Ngananctrab = existente.Ngananctrab;
            p.Producto.Nprecminoris = existente.Nprecminoris;
            p.Producto.Nprecmayoris = existente.Nprecmayoris;
            p.Producto.Ndisponibilidad = existente.Ncantidad;
            p.Producto.Ncantidad = existente.Ncantidad;
            p.Producto.CategoriaProducto = existente.CategoriaProducto;
            p.Producto.CategoriaProductoId = existente.CategoriaProductoId;
            p.Producto.Ndescuento = 0;
            p.Producto.Lactivo = true;
            p.Producto.Dfechacreacion = DateTime.Now;
          
            return View("Create",p);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReCreate([Bind(Include = "ID,Carticulo,Cdescripcion,CategoriaProductoId,Ncantidad,Npreccosto,Nprecmayoris,Nprecminoris,Ngananctrab,Clote,Ndescuento,Ndisponibilidad,Lactivo,Linversion,Dfechacreacion")] Producto producto)
        {

            bool seguir = true;
            Finanzas f = db.Finanzas.First();
            if (producto.Linversion)
            {   //Actualizando Finanzas
                f.ValorProductos += producto.Npreccosto * producto.Ncantidad;
                f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
                db.Entry(f).State = EntityState.Modified;
                db.SaveChanges();
                //Registrando Movimientos
               /* Registrar_Movimiento(4, "Compra de producto: " + producto.Carticulo,
                    "Se compra el producto: " + producto.Carticulo +
                    " por Inversión aumentando valor en productos en: " + producto.Npreccosto * producto.Ncantidad,
                    producto.Npreccosto * producto.Ncantidad, 0);*/


            }
            else
            {
                //Restar el costo total al Total de efectivo. 
                //  Total de Efectivo = Total de Efectivo -(∑ precio costos *cantidad productos).
                f.ValorEfectivo -= producto.Npreccosto * producto.Ncantidad;
                if (f.ValorEfectivo < 0)
                {
                    seguir = false;
                    ViewBag.Mensaje = true;
                }
                else
                {
                    //	Sumar el costo total al Dinero invertido
                    //    Dinero en inventario = Dinero en inventario+(∑ precio costos *cantidad productos).
                    f.ValorProductos += producto.Npreccosto * producto.Ncantidad;
                    //Actualizando Finanzas
                    f.ValorCapital = f.ValorEfectivo + f.ValorProductos + f.Ganancias;
                    db.Entry(f).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    //Registrando Movimientos
                    //Ingreso
                    /*Registrar_Movimiento(4, "Compra de producto: " + producto.Carticulo,
                        "Se compra el producto: " + producto.Carticulo +
                        " aumentando valor en productos en: " + producto.Npreccosto * producto.Ncantidad,
                        producto.Npreccosto * producto.Ncantidad, 0);
                    //Gasto
                    Registrar_Movimiento(4, "Disminuye efectivo por Compra de producto: " + producto.Carticulo,
                        "Se compra el producto: " + producto.Carticulo +
                        " disminuyendo valor en efectivo en: " + producto.Npreccosto * producto.Ncantidad,
                        producto.Npreccosto * producto.Ncantidad, 0);*/

                }



            }


            ProductoViewModel p = new ProductoViewModel()
            {
                Producto = producto
            };
            if (ModelState.IsValid && seguir)
            {

                producto.Dfechacreacion = DateTime.Now;
                db.Productos.Add(producto);
                await db.SaveChangesAsync();
                if (producto.Linversion)
                {
                    Registrar_Movimiento(4, "Compra de producto: " + producto.Carticulo,
                    "Se compra el producto: " + producto.Carticulo +
                    " por inversión aumentando valor en productos en: " + producto.Npreccosto * producto.Ncantidad,
                    producto.Npreccosto * producto.Ncantidad, 0, producto.ID);
                }
                else
                {
                    Registrar_Movimiento(4, "Compra de producto: " + producto.Carticulo,
                        "Se compra el producto: " + producto.Carticulo +
                        " aumentando valor en productos en: " + producto.Npreccosto * producto.Ncantidad,
                        producto.Npreccosto * producto.Ncantidad, 0, producto.ID);
                    //Gasto
                    Registrar_Movimiento(4, "Disminuye efectivo por Compra de producto: " + producto.Carticulo,
                        "Se compra el producto: " + producto.Carticulo +
                        " disminuyendo valor en efectivo en: " + producto.Npreccosto * producto.Ncantidad,
                        producto.Npreccosto * producto.Ncantidad, 0, producto.ID);
                }
               
                return RedirectToAction("Index");
            }

            p.TipoCategoriaProductos = db.CategoriaProductoes.ToList();
            return View("Create",p);
        }
    }
}
