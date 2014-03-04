namespace Toptix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cascadeOnDeleteFalse : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Charge", "OrderID", "dbo.Order");
            DropForeignKey("dbo.OrderItem", "OrderID", "dbo.Order");
            DropIndex("dbo.Charge", new[] { "OrderID" });
            DropIndex("dbo.OrderItem", new[] { "OrderID" });
            AddForeignKey("dbo.Charge", "OrderID", "dbo.Order", "ID");
            AddForeignKey("dbo.OrderItem", "OrderID", "dbo.Order", "ID");
            CreateIndex("dbo.Charge", "OrderID");
            CreateIndex("dbo.OrderItem", "OrderID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.OrderItem", new[] { "OrderID" });
            DropIndex("dbo.Charge", new[] { "OrderID" });
            DropForeignKey("dbo.OrderItem", "OrderID", "dbo.Order");
            DropForeignKey("dbo.Charge", "OrderID", "dbo.Order");
            CreateIndex("dbo.OrderItem", "OrderID");
            CreateIndex("dbo.Charge", "OrderID");
            AddForeignKey("dbo.OrderItem", "OrderID", "dbo.Order", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Charge", "OrderID", "dbo.Order", "ID", cascadeDelete: true);
        }
    }
}
