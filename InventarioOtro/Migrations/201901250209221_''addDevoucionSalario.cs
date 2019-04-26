namespace InventarioOtro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDevoucionSalario : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Devolucions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Notas = c.String(),
                        VentaId = c.Int(nullable: false),
                        DescuentoSalario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Fecha = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ventas", t => t.VentaId, cascadeDelete: true)
                .Index(t => t.VentaId);
            
            CreateTable(
                "dbo.PagoSalarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Trabajador = c.String(),
                        Monto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Estado = c.Int(nullable: false),
                        FechaPago = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Devolucions", "VentaId", "dbo.Ventas");
            DropIndex("dbo.Devolucions", new[] { "VentaId" });
            DropTable("dbo.PagoSalarios");
            DropTable("dbo.Devolucions");
        }
    }
}
