namespace InventarioOtro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCantDiasLaborado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PagoSalarios", "CantidadLaborada", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PagoSalarios", "CantidadLaborada");
        }
    }
}
