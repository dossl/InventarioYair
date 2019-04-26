namespace InventarioOtro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class agregandoGananciaTrabajador : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ventas", "GanaTrab", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.VentaDetalles", "Descuento", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VentaDetalles", "Descuento", c => c.Int(nullable: false));
            DropColumn("dbo.Ventas", "GanaTrab");
        }
    }
}
