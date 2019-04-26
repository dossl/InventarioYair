namespace InventarioOtro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDeudas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Deudas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VentaId = c.Int(nullable: false),
                        ValorInicial = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValorActual = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FechaCreacion = c.DateTime(nullable: false),
                        UltimoPago = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ventas", t => t.VentaId, cascadeDelete: true)
                .Index(t => t.VentaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Deudas", "VentaId", "dbo.Ventas");
            DropIndex("dbo.Deudas", new[] { "VentaId" });
            DropTable("dbo.Deudas");
        }
    }
}
