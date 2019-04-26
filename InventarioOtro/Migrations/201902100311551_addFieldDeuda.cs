namespace InventarioOtro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFieldDeuda : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Deudas", "CostoProductoAPagar", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Deudas", "GanaTrabAPagar", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Deudas", "GanaTrabAPagar");
            DropColumn("dbo.Deudas", "CostoProductoAPagar");
        }
    }
}
