using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InventarioOtro.Models;

namespace InventarioOtro.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            Finanzas f = db.Finanzas.First();
            ViewBag.Ganancia = f.Ganancias;
            ViewBag.ValorProducto = f.ValorProductos;
            ViewBag.Capital = f.ValorCapital;
            ViewBag.Efectivo = f.ValorEfectivo;
            ViewBag.Ventas = CalcularTotalVentasDesdeUltimoCierre();
            ViewBag.Gastos = CalcularTotalGastosDesdeUltimoCierre();
            ViewBag.Deudas = CalcularDeudasVigentes();
            ViewBag.Merma = CalcularMermas();
            try
            {
            
              ViewBag.Ult = db.Cierres.OrderByDescending(c => c.Fecha).First().Fecha.ToString("dd/MM/yyyy");
            }
            catch (Exception)
            {
                ViewBag.Ult = "No realizado";

            }
             
            return View();
        }

        private decimal CalcularTotalVentasDesdeUltimoCierre()
        {
            decimal valor = 0;
            List<Venta> ventasNoCerradas = db.Ventas.Where(v => v.Estado==2).ToList();
            foreach (var venta in ventasNoCerradas)
            {
                valor += venta.PrecioTotal-venta.Descuento;
            }
            return valor;
        }

        private decimal CalcularTotalGastosDesdeUltimoCierre()
        {
            decimal valor = 0;
            DateTime? ultimoCierre = null;
            List<Movimiento> gastos = new List<Movimiento>();
            try
            {
                
                ultimoCierre = db.Cierres.OrderByDescending(c => c.Fecha).First().Fecha;
                gastos = db.Movimientos.Where(g =>(( g.IdProducto == 1 && g.TipoMovimiento == 2) ) && g.FechaMovimiento >= ultimoCierre).ToList();
            }
            catch (Exception)
            {
                gastos = db.Movimientos.Where(g => (g.IdProducto == 1 && g.TipoMovimiento == 2) ).ToList();

            }
           
            foreach (var gast in gastos)
            {
                valor += gast.Monto;
            }
            List<Venta> ventasNoCerradas = db.Ventas.Where(v => v.Estado == 2).ToList();
            
            foreach (var venta in ventasNoCerradas)
            {
                
                valor += venta.GanaTrab;
            }

            //AGREGANDO VALOR DE GASTOS EN SALARIO DESDE EL ULTIMO CIERRE
            List<PagoSalario> salarios = new List<PagoSalario>();
            try
            {

                ultimoCierre = db.Cierres.OrderByDescending(c => c.Fecha).First().Fecha;
                salarios = db.PagoSalarios.Where(p => p.FechaPago >= ultimoCierre).ToList();
            }
            catch (Exception)
            {
                salarios = db.PagoSalarios.Where(p => p.FechaPago >= ultimoCierre).ToList();

            }
            foreach (var sal in salarios)
            {
                valor += ((sal.CantidadLaborada-sal.DiasDescontado)*3);
            }
            return valor;
        }
        private decimal CalcularDeudasVigentes()
        {
            decimal valor = 0;
            List<Deuda> deudasVigentes = db.Deudas.Where(d => d.ValorActual>0).ToList();
            foreach (var d in deudasVigentes)
            {
                valor += d.ValorActual;
            }
            return valor;
        }
        private decimal CalcularMermas()
        {
            decimal valor = 0;
            List<Merma> mermas = db.Mermas.Include(v=>v.Producto).ToList();
           
            foreach (var d in mermas)
            {
                valor += d.Producto.Npreccosto*d.Cantidad;
            }
            return valor;
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}