namespace Toptix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class temp4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderItem", "Product_ID", "dbo.Product");
            DropForeignKey("dbo.OrderItem", "EnumCalculationTypeID", "dbo.EnumCalculationType");
            DropForeignKey("dbo.OrderItem", "OrderID", "dbo.Order");
            DropIndex("dbo.OrderItem", new[] { "Product_ID" });
            DropIndex("dbo.OrderItem", new[] { "EnumCalculationTypeID" });
            DropIndex("dbo.OrderItem", new[] { "OrderID" });
            DropTable("dbo.OrderItem");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OrderItem",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductsID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        IsPartOfService = c.Boolean(nullable: false),
                        EnumCalculationTypeID = c.Int(nullable: false),
                        Discount = c.Double(nullable: false),
                        NetPrice = c.Double(nullable: false),
                        OrderID = c.Int(nullable: false),
                        Product_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.OrderItem", "OrderID");
            CreateIndex("dbo.OrderItem", "EnumCalculationTypeID");
            CreateIndex("dbo.OrderItem", "Product_ID");
            AddForeignKey("dbo.OrderItem", "OrderID", "dbo.Order", "ID");
            AddForeignKey("dbo.OrderItem", "EnumCalculationTypeID", "dbo.EnumCalculationType", "ID");
            AddForeignKey("dbo.OrderItem", "Product_ID", "dbo.Product", "ID");
        }
    }
}
