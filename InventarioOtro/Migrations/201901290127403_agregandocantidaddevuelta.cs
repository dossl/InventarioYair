namespace InventarioOtro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class agregandocantidaddevuelta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Devolucions", "Cantidad", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Devolucions", "Cantidad");
        }
    }
}
