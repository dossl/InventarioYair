namespace InventarioOtro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class creandoBD : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoriaProductoes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Cnombre = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Clastname = c.String(maxLength: 100),
                        Cfirstname = c.String(nullable: false, maxLength: 100),
                        Ccidentidad = c.String(nullable: false, maxLength: 11),
                        Cnumtelefono = c.String(),
                        Lactivo = c.Boolean(nullable: false),
                        Cdireccion = c.String(maxLength: 255),
                        Cnotas = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Finanzas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ValorCapital = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValorProductos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValorEfectivo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Ganancias = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.FotoProductoes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Bfotos = c.Binary(),
                        Title = c.String(),
                        Description = c.String(),
                        ImageContent = c.Binary(),
                        ImageMimeType = c.String(),
                        ImageName = c.String(),
                        FileMimeType = c.String(),
                        FileName = c.String(),
                        ProductoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Productoes", t => t.ProductoId, cascadeDelete: true)
                .Index(t => t.ProductoId);
            
            CreateTable(
                "dbo.Productoes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Carticulo = c.String(nullable: false, maxLength: 50),
                        Cdescripcion = c.String(nullable: false, maxLength: 255),
                        CategoriaProductoId = c.Int(nullable: false),
                        Ncantidad = c.Int(nullable: false),
                        Npreccosto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Nprecmayoris = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Nprecminoris = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Ngananctrab = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Clote = c.String(nullable: false),
                        Ndescuento = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Ndisponibilidad = c.Int(nullable: false),
                        Lactivo = c.Boolean(nullable: false),
                        Linversion = c.Boolean(nullable: false),
                        Dfechacreacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CategoriaProductoes", t => t.CategoriaProductoId, cascadeDelete: true)
                .Index(t => t.CategoriaProductoId);
            
            CreateTable(
                "dbo.FotoVentas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Bfotos = c.Binary(),
                        Title = c.String(),
                        Description = c.String(),
                        ImageContent = c.Binary(),
                        ImageMimeType = c.String(),
                        ImageName = c.String(),
                        FileMimeType = c.String(),
                        FileName = c.String(),
                        VentaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Ventas", t => t.VentaId, cascadeDelete: true)
                .Index(t => t.VentaId);
            
            CreateTable(
                "dbo.Ventas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FechaVenta = c.DateTime(nullable: false),
                        MensajeroId = c.Int(nullable: false),
                        VendedorId = c.Int(nullable: false),
                        ClienteId = c.Int(nullable: false),
                        GanaMensajero = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Descuento = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CostoTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GanaTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrecioTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CantTotalProductos = c.Int(nullable: false),
                        Estado = c.Int(nullable: false),
                        Usuario = c.String(),
                        TipoVenta = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Clientes", t => t.ClienteId, cascadeDelete: true)
                .ForeignKey("dbo.Vendedors", t => t.VendedorId, cascadeDelete: true)
                .Index(t => t.VendedorId)
                .Index(t => t.ClienteId);
            
            CreateTable(
                "dbo.VentaDetalles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductoId = c.Int(nullable: false),
                        VentaId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        Garantia = c.Int(nullable: false),
                        Descuento = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Productoes", t => t.ProductoId, cascadeDelete: true)
                .ForeignKey("dbo.Ventas", t => t.VentaId, cascadeDelete: true)
                .Index(t => t.ProductoId)
                .Index(t => t.VentaId);
            
            CreateTable(
                "dbo.Vendedors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Mensajeroes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Movimientoes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Monto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TipoMovimiento = c.Int(nullable: false),
                        Usuario = c.String(),
                        FechaMovimiento = c.DateTime(nullable: false),
                        Descripcion = c.String(),
                        IdProducto = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Ventas", "VendedorId", "dbo.Vendedors");
            DropForeignKey("dbo.FotoVentas", "VentaId", "dbo.Ventas");
            DropForeignKey("dbo.VentaDetalles", "VentaId", "dbo.Ventas");
            DropForeignKey("dbo.VentaDetalles", "ProductoId", "dbo.Productoes");
            DropForeignKey("dbo.Ventas", "ClienteId", "dbo.Clientes");
            DropForeignKey("dbo.FotoProductoes", "ProductoId", "dbo.Productoes");
            DropForeignKey("dbo.Productoes", "CategoriaProductoId", "dbo.CategoriaProductoes");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.VentaDetalles", new[] { "VentaId" });
            DropIndex("dbo.VentaDetalles", new[] { "ProductoId" });
            DropIndex("dbo.Ventas", new[] { "ClienteId" });
            DropIndex("dbo.Ventas", new[] { "VendedorId" });
            DropIndex("dbo.FotoVentas", new[] { "VentaId" });
            DropIndex("dbo.Productoes", new[] { "CategoriaProductoId" });
            DropIndex("dbo.FotoProductoes", new[] { "ProductoId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Movimientoes");
            DropTable("dbo.Mensajeroes");
            DropTable("dbo.Vendedors");
            DropTable("dbo.VentaDetalles");
            DropTable("dbo.Ventas");
            DropTable("dbo.FotoVentas");
            DropTable("dbo.Productoes");
            DropTable("dbo.FotoProductoes");
            DropTable("dbo.Finanzas");
            DropTable("dbo.Clientes");
            DropTable("dbo.CategoriaProductoes");
        }
    }
}
