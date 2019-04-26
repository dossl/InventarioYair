using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InventarioOtro.Models;
using InventarioOtro.ViewModels;
using PagedList;

namespace InventarioOtro.Controllers
{
    public class ReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Report
        public ActionResult ReporteMensajeria()
        {
            ViewBag.Mensaje = false;
            RptMensajeriaViewModel r = new RptMensajeriaViewModel();
            r.Inicio=DateTime.Today;
            r.Fin = DateTime.Today;
           /* Seguridad s = new Seguridad();
                if (!s.ValidarSeguridad())
                {
                    return View("Error");
                }*/
                return View(r);
                
        }
        [HttpPost]
        public ActionResult ReporteMensajeria([Bind(Include = "MensajeroId,Nombre,Inicio,Fin")]  RptMensajeriaViewModel model)
        {
            ViewBag.Mensaje = false;
            if (model.Inicio>model.Fin)
            {
                ViewBag.Mensaje = true;
                return View();
            }
            return RedirectToAction("Mensajeria",model);

        }

        [HttpGet]
        public ActionResult Mensajeria(RptMensajeriaViewModel model)
        {
            var rptInfo = new ReportInfo();
            rptInfo.ReportDescription = "Reporte de Mensajerías";
            rptInfo.ReportName = "mensajeria";
            rptInfo.p1 = model.Inicio.ToString("dd/MM/yyyy");
            rptInfo.p2 = model.Fin.ToString("dd/MM/yyyy");
            rptInfo.p3 = model.MensajeroId;
            rptInfo.p4 = model.Nombre;
            rptInfo.ReportURL = String.Format("../Reportes/Mensajeria.aspx?screen=/{0}", rptInfo.ReportName);
            rptInfo.Width = 100;
            rptInfo.Height = 600;

            rptInfo.ReportURL = String.Format("{0}&p1={1}&p2={2}&p3={3}&p4={4}", rptInfo.ReportURL, rptInfo.p1, rptInfo.p2, rptInfo.p3, rptInfo.p4);

            var sessionName = string.Format("report{0}", rptInfo.ReportName);
            return PartialView(rptInfo);
        }


        public ActionResult ReporteVentas()
        {
            ViewBag.Mensaje = false;
            RptVentasViewModel r = new RptVentasViewModel();
            r.Inicio = DateTime.Today;
            r.Fin = DateTime.Today;
            /*Seguridad s = new Seguridad();
            if (!s.ValidarSeguridad())
            {
                return View("Error");
            }*/
            return View(r);

        }
        [HttpPost]
        public ActionResult ReporteVentas([Bind(Include = "Cliente,Producto,Inicio,Fin")]  RptVentasViewModel model)
        {
            ViewBag.Mensaje = false;
            if (model.Inicio > model.Fin)
            {
                ViewBag.Mensaje = true;
                return View();
            }
            if (string.IsNullOrEmpty(model.Producto))
            {
                model.Producto = "";
            }
            if (string.IsNullOrEmpty(model.Cliente))
            {
                model.Cliente = "";
            }
            return RedirectToAction("Ventas", model);

        }

        [HttpGet]
        public ActionResult Ventas(RptVentasViewModel model)
        {
            var rptInfo = new ReportInfo();
            rptInfo.ReportDescription = "Reporte de Ventas";
            rptInfo.ReportName = "ventas";
            
            rptInfo.p1 = model.Inicio.ToString("dd/MM/yyyy");
            rptInfo.p2 = model.Fin.ToString("dd/MM/yyyy");
            rptInfo.p3 = model.Cliente;
            rptInfo.p4 = model.Producto;
            rptInfo.ReportURL = String.Format("../Reportes/Ventas.aspx?screen=/{0}", rptInfo.ReportName);
            rptInfo.Width = 100;
            rptInfo.Height = 600;

            rptInfo.ReportURL = String.Format("{0}&p1={1}&p2={2}&p3={3}&p4={4}", rptInfo.ReportURL, rptInfo.p1, rptInfo.p2, rptInfo.p3, rptInfo.p4);

            var sessionName = string.Format("report{0}", rptInfo.ReportName);
            return PartialView(rptInfo);
        }


        public ActionResult ReporteGastos()
        {
            ViewBag.Mensaje = false;
            RptGastosViewModel r = new RptGastosViewModel();
            r.Inicio = DateTime.Today;
            r.Fin = DateTime.Today;
            /*Seguridad s = new Seguridad();
            if (!s.ValidarSeguridad())
            {
                return View("Error");
            }*/
            return View(r);

        }
        [HttpPost]
        public ActionResult ReporteGastos([Bind(Include = "Inicio,Fin")]  RptGastosViewModel model)
        {
            ViewBag.Mensaje = false;
            if (model.Inicio > model.Fin)
            {
                ViewBag.Mensaje = true;
                return View();
            }
            return RedirectToAction("Gastos", model);

        }

        [HttpGet]
        public ActionResult Gastos(RptGastosViewModel model)
        {
            var rptInfo = new ReportInfo();
            rptInfo.ReportDescription = "Reporte de Gastos";
            rptInfo.ReportName = "gastos";
            rptInfo.p1 = model.Inicio.ToString("dd/MM/yyyy");
            rptInfo.p2 = model.Fin.ToString("dd/MM/yyyy");
           
            rptInfo.ReportURL = String.Format("../Reportes/Gastos.aspx?screen=/{0}", rptInfo.ReportName);
            rptInfo.Width = 100;
            rptInfo.Height = 600;

            rptInfo.ReportURL = String.Format("{0}&p1={1}&p2={2}", rptInfo.ReportURL, rptInfo.p1, rptInfo.p2);

            var sessionName = string.Format("report{0}", rptInfo.ReportName);
            return PartialView(rptInfo);
        }

        [HttpGet]
        public ActionResult Salario(int Id)
        {
            var rptInfo = new ReportInfo();
            rptInfo.ReportDescription = "Reporte de Salario";
            rptInfo.ReportName = "salario";
            rptInfo.p1 = Id.ToString();
            

            rptInfo.ReportURL = String.Format("../../Reportes/Salario.aspx?screen=/{0}", rptInfo.ReportName);
            rptInfo.Width = 100;
            rptInfo.Height = 600;

            rptInfo.ReportURL = String.Format("{0}&p1={1}", rptInfo.ReportURL, rptInfo.p1);

            var sessionName = string.Format("report{0}", rptInfo.ReportName);
            return PartialView(rptInfo);
        }


        public ActionResult ReporteDeudas()
        {
            ViewBag.Mensaje = false;
            RptDeudasViewModel r = new RptDeudasViewModel();
            r.Inicio = DateTime.Today;
            r.Fin = DateTime.Today;
            /*Seguridad s = new Seguridad();
            if (!s.ValidarSeguridad())
            {
                return View("Error");
            }*/
            return View(r);

        }
        [HttpPost]
        public ActionResult ReporteDeudas([Bind(Include = "Inicio,Fin")]  RptDeudasViewModel model)
        {
            ViewBag.Mensaje = false;
            if (model.Inicio > model.Fin)
            {
                ViewBag.Mensaje = true;
                return View();
            }
            return RedirectToAction("Deudas", model);

        }

        [HttpGet]
        public ActionResult Deudas(RptDeudasViewModel model)
        {
            var rptInfo = new ReportInfo();
            rptInfo.ReportDescription = "Reporte de Deudas";
            rptInfo.ReportName = "deudas";
            rptInfo.p1 = model.Inicio.ToString("dd/MM/yyyy");
            rptInfo.p2 = model.Fin.ToString("dd/MM/yyyy");

            rptInfo.ReportURL = String.Format("../Reportes/Deudas.aspx?screen=/{0}", rptInfo.ReportName);
            rptInfo.Width = 100;
            rptInfo.Height = 600;

            rptInfo.ReportURL = String.Format("{0}&p1={1}&p2={2}", rptInfo.ReportURL, rptInfo.p1, rptInfo.p2);

            var sessionName = string.Format("report{0}", rptInfo.ReportName);
            return PartialView(rptInfo);
        }

    }
}