namespace InventarioOtro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class claculodeganancia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ventas", "CalcSalarioTrab", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ventas", "CalcSalarioTrab");
        }
    }
}
