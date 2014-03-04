namespace Toptix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class temp5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderItem",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        IsPartOfService = c.Boolean(nullable: false),
                        EnumCalculationTypeID = c.Int(nullable: false),
                        Discount = c.Double(nullable: false),
                        NetPrice = c.Double(nullable: false),
                        OrderID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Product", t => t.ProductID)
                .ForeignKey("dbo.EnumCalculationType", t => t.EnumCalculationTypeID)
                .ForeignKey("dbo.Order", t => t.OrderID)
                .Index(t => t.ProductID)
                .Index(t => t.EnumCalculationTypeID)
                .Index(t => t.OrderID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.OrderItem", new[] { "OrderID" });
            DropIndex("dbo.OrderItem", new[] { "EnumCalculationTypeID" });
            DropIndex("dbo.OrderItem", new[] { "ProductID" });
            DropForeignKey("dbo.OrderItem", "OrderID", "dbo.Order");
            DropForeignKey("dbo.OrderItem", "EnumCalculationTypeID", "dbo.EnumCalculationType");
            DropForeignKey("dbo.OrderItem", "ProductID", "dbo.Product");
            DropTable("dbo.OrderItem");
        }
    }
}
