using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InventarioOtro.Startup))]
namespace InventarioOtro
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
