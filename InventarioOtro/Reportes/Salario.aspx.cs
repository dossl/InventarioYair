using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace InventarioOtro.Reportes
{
    public partial class Salario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
                if (!IsPostBack)
                {
                    var pathReport = Request.QueryString["screen"];
                    var usr = Request.QueryString["usr"];
                    var token = Request.QueryString["t"];
                    var reportType = Request.QueryString["tr"];
                    var pathReportReal = string.Empty;


                    var param = new List<ReportParameter>();
                    param.Add(new ReportParameter("Server", "localhost"));
                    param.Add(new ReportParameter("BD", "InventarioDB"));

                    if (string.IsNullOrEmpty(pathReport))
                        throw new Exception("Error en Parámetros");

                    switch (pathReport)
                    {
                        case "/salario":
                            pathReportReal = "/ReportInventario";
                            var p1 = Request.QueryString["p1"];
                           

                            if (string.IsNullOrEmpty(p1))
                                throw new Exception("Error en Parámetros");
                           
                            param.Add(new ReportParameter("Id", p1));
                            
                            break;

                        default:


                            throw new Exception("Error en Parámetros");
                    }

                     var url =ConfigurationManager.AppSettings["ReportServerUrl"].ToString();
                    //  var prefix = ConfigurationManager.AppSettings["ReportServerPrefix"].ToString();

                    rptViewer.ServerReport.ReportServerUrl = new Uri(url);
                    rptViewer.ServerReport.ReportServerCredentials = new ReportCredentials();
                    //rptDepreci.ServerReport.ReportServerCredentials = new ReportServerCredentials("trabajo", "*123Ventana", "servercodeas");

                    //rptViewer.ServerReport.ReportServerUrl = new Uri("http://sql5030.site4now.net/ReportServer");
                    //rptViewer.ServerReport.ReportPath = "/vagwireless-001/assignedimsi";
                    rptViewer.ServerReport.ReportPath = pathReportReal + pathReport;
                    rptViewer.ProcessingMode = ProcessingMode.Remote;
                    rptViewer.ShowParameterPrompts = false;

                    if (param.Count > 0)
                        rptViewer.ServerReport.SetParameters(param);

                    rptViewer.ServerReport.Refresh();
                }
            }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), this.ClientID, ";", true);
        }
    }
}