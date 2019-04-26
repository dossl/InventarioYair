namespace InventarioOtro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modificandoTablaMov : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movimientoes", "IdProCompra", c => c.Int(nullable: false));
            AddColumn("dbo.Movimientoes", "IdVenta", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movimientoes", "IdVenta");
            DropColumn("dbo.Movimientoes", "IdProCompra");
        }
    }
}
