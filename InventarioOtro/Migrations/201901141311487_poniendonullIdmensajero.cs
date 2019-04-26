namespace InventarioOtro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class poniendonullIdmensajero : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ventas", "MensajeroId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ventas", "MensajeroId", c => c.Int(nullable: false));
        }
    }
}
