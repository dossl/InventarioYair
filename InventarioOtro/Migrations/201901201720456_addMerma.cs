namespace InventarioOtro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMerma : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Mermas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductoId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Productoes", t => t.ProductoId, cascadeDelete: true)
                .Index(t => t.ProductoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Mermas", "ProductoId", "dbo.Productoes");
            DropIndex("dbo.Mermas", new[] { "ProductoId" });
            DropTable("dbo.Mermas");
        }
    }
}
