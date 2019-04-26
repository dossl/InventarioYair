using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using Microsoft.Reporting.WebForms;

namespace InventarioOtro.Reportes
{
    public class ReportCredentials : IReportServerCredentials
    {
        public WindowsIdentity ImpersonationUser
        {
            get
            {
                return null;
            }
        }

        public ICredentials NetworkCredentials
        {
            get
            {

                var user = ConfigurationManager.AppSettings["ReportServerUser"].ToString();
                var pwd = ConfigurationManager.AppSettings["ReportServerPwd"].ToString(); 
                var domain = ConfigurationManager.AppSettings["ReportServerDomain"].ToString();
                return new NetworkCredential(user, pwd, domain);
            }
        }

        public bool GetFormsCredentials(out Cookie authCookie, out string userName, out string password, out string authority)
        {
            authCookie = null;
            userName = null;
            password = null;
            authority = null;

            // Not using form credentials
            return false;
        }


    }
}