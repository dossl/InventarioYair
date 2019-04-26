namespace InventarioOtro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addVentaDescripcion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ventas", "Descripcion", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ventas", "Descripcion");
        }
    }
}
