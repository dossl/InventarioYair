namespace InventarioOtro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class quitandoestadopagosalario : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PagoSalarios", "Estado");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PagoSalarios", "Estado", c => c.Int(nullable: false));
        }
    }
}
