namespace InventarioOtro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCantDiasDescontados : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PagoSalarios", "DiasDescontado", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PagoSalarios", "DiasDescontado");
        }
    }
}
