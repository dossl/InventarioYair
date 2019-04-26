namespace InventarioOtro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrabajadorReq : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PagoSalarios", "Trabajador", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PagoSalarios", "Trabajador", c => c.String());
        }
    }
}
