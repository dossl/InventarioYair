namespace InventarioOtro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cierre : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cierres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GananciasPeriodo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CantidadArticulos = c.Int(nullable: false),
                        ValorTotalVentas = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CantArticulosDev = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Cierres");
        }
    }
}
