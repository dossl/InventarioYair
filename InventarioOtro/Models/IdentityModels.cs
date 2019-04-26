using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using InventarioOtro.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace InventarioOtro.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("name=InventarioConn", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Finanzas> Finanzas { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }
        public DbSet<CategoriaProducto> CategoriaProductoes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<FotoProducto> FotoProductos { get; set; }
        public DbSet<Vendedor> Vendedores { get; set; }
        public DbSet<Mensajero> Mensajeros { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<VentaDetalle> VentaDetalles { get; set; }
        public DbSet<FotoVenta> FotoVentas { get; set; }
        public DbSet<Merma> Mermas { get; set; }
        public DbSet<PagoSalario> PagoSalarios { get; set; }
        public DbSet<Devolucion> Devoluciones { get; set; }
        public DbSet<Cierre> Cierres { get; set; }
        public DbSet<Deuda> Deudas { get; set; }

        
    }
}