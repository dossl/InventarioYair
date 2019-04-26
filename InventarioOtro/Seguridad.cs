using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;

namespace InventarioOtro
{
    public class Seguridad
    {
        public bool ValidarSeguridad()
        {
            
            
            string line;
            string urlFichEx = HttpContext.Current.Server.MapPath("~/App_Readme/lic.key");
            StreamReader file;
            try
            {
                 file =new StreamReader(urlFichEx);
            }
            catch (Exception e)
            {
                return false;
            }
            


            while ((line = file.ReadLine()) != null)
            {
                IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                
                foreach (NetworkInterface adapter in nics)
                {
                    if (NetworkInterfaceType.Ethernet.Equals(adapter.NetworkInterfaceType))
                    {
                        if (adapter.GetPhysicalAddress().ToString().Equals(line))
                            return true;
                    }
                }

            }
            return false;

        }
    }
}