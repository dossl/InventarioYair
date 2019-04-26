namespace InventarioOtro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDineroExtraido : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cierres", "GananciaExtraida", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cierres", "GananciaExtraida");
        }
    }
}
